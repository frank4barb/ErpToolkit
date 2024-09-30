
using CsvHelper.Configuration;
using CsvHelper;
using System.Data;
using System.Globalization;
using System.Data.Common;
using static ErpToolkit.Helpers.ErpError;
using System.Text;

namespace ErpToolkit.Helpers.Db
{
    // Funzioni di gestione accesso al Database, indipendentemente dal DBMS
    public class DatabaseManager : IDisposable
    {
        private readonly string _databaseType = "";
        private readonly IDatabase _database;
        private static NLog.ILogger _logger;

        private Stack<string> _transactionStack = new Stack<string>();
        private Timer _transactionTimeoutTimer;
        private string _transactionId = null;


        // Proprietà configurabili
        public string DatabaseType { get { return _databaseType; } }  
        public int PageSize { get; set; } = 1000;  //ReadBlob, WriteBlob
        public int MaxRetries { get; set; } = 3;
        public int DelayBetweenRetriesMs { get; set; } = 1000;
        public int TimeoutSeconds { get; set; } = 30;
        public int TransactionTimeoutSeconds { get; set; } = 60;
        public int MaxRecords { get; set; } = 10000;
        public bool EnableTrace { get; set; } = false;
        public int MaxFileLengthBytes { get; set; } = 1024 * 1024 * 1024;  // 1 Gb

        internal DatabaseManager(string databaseType, IDatabase database)
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
            //set database
            _databaseType = databaseType;
            _database = database;
        }
        ~DatabaseManager()
        {
            Dispose();
        }
        public void Dispose()
        {
            _database.Dispose(); CleanupTransaction();
            GC.SuppressFinalize(this);
        }

        //***************************************************************************************************************************************************
        //*** TRANSAZIONI
        //***************************************************************************************************************************************************

        //public

        public string BeginTransaction(string transactionId, string transactionName = "")
        {
            if (String.IsNullOrEmpty(transactionName) || _transactionStack.Contains(transactionName)) transactionName = $"SAVEPOINT_{_transactionStack.Count}";
            if (_transactionStack.Count == 0)
            {
                _database.BeginTransaction(transactionName);
                _transactionId = Guid.NewGuid().ToString();
                _transactionTimeoutTimer = new Timer(TransactionTimeoutCallback, null, TransactionTimeoutSeconds * 1000, Timeout.Infinite);
            }
            else
            {
                if (_transactionId != transactionId) RollBackDefaulTransaction("BeginTransaction");
                _database.SavePointTransaction(transactionName);
            }
            _transactionStack.Push(transactionName);
            return _transactionId;
        }
        public void CommitTransaction(string transactionId, string transactionName = "")
        {
            if (String.IsNullOrEmpty(transactionName)) transactionName = $"SAVEPOINT_{_transactionStack.Count}";
            if (_transactionStack.Count == 0 || _transactionId != transactionId || _transactionStack.Peek() != transactionName) RollBackDefaulTransaction("CommitTransaction");

            _transactionStack.Pop();  //elimina savepoint in coda
            if (_transactionStack.Count == 0)
            {
                _database.CommitTransaction(transactionName);
                CleanupTransaction();
            }
        }
        public void RollbackTransaction(string transactionId, string transactionName = "")
        {
            if (String.IsNullOrEmpty(transactionName)) transactionName = $"SAVEPOINT_{_transactionStack.Count}";
            if (_transactionStack.Count == 0 || _transactionId != transactionId || _transactionStack.Peek() != transactionName) RollBackDefaulTransaction("RollbackTransaction");

            _transactionStack.Pop();   //elimina savepoint in coda
            if (_transactionStack.Count > 0)
            {
                _database.RollbackSavePoint(transactionName);
            }
            else
            {
                _database.RollbackTransaction(transactionName);
                CleanupTransaction();
            }
        }

        //private

        private void CleanupTransaction()
        {
            _transactionStack.Clear(); _transactionId = null;
            _transactionTimeoutTimer?.Dispose(); _transactionTimeoutTimer = null;
        }
        private void RollBackDefaulTransaction(string action)
        {
            _database.RollbackTransaction("Transaction_Default");
            CleanupTransaction();
            throw new DatabaseException(ERR_DB_BADTRAN, "{action} attempted for the wrong transaction.");
        }
        private void TransactionTimeoutCallback(object state)
        {
            _database.RollbackTransaction("Transaction_Timeout"); 
            throw new DatabaseException(ERR_DB_TIMEOUT, "Transaction timeout reached.");
        }

        //***************************************************************************************************************************************************
        //*** QUERY - MANTAIN
        //***************************************************************************************************************************************************

        //public

        // ExecuteScalar
        public bool RecordExists(string tableName, string keyField, object keyValue, string transactionId = null)
        {
            if (_transactionId != transactionId) RollBackDefaulTransaction("RecordExists");
            string sql = $"SELECT COUNT(1) FROM {tableName} WHERE {keyField} = @keyValue";
            IDbConnection connection = _database.NewConnection();
            try
            {
                using (IDbCommand command = _database.NewCommand(sql, connection)) // la transazione viene passate nel NewCommand
                {
                    command.CommandTimeout = TimeoutSeconds;
                    IDbDataParameter parameter = command.CreateParameter(); //command.Parameters.AddWithValue("@keyValue", keyValue);
                    parameter.ParameterName = "@keyValue"; parameter.Value = keyValue;
                    command.Parameters.Add(parameter);
                    return (int)ExecuteScalarWithRetry(command) > 0;
                }
            }
            finally { _database.ReleaseConnection(connection); } // la connessione viene chiusa se non c'è transazione
        }
        public byte[] ReadBlob(string tableName, string keyField, object keyValue, string blobField, int pageNumber, string transactionId = null)
        {
            if (_transactionId != transactionId) RollBackDefaulTransaction("ReadBlob");
            int offset = pageNumber * PageSize;
            string sql = $"SELECT SUBSTRING({blobField}, {offset + 1}, {PageSize}) FROM {tableName} WHERE {keyField} = @keyValue";
            IDbConnection connection = _database.NewConnection();
            try
            {
                using (IDbCommand command = _database.NewCommand(sql, connection)) // la transazione viene passate nel NewCommand
                {
                    command.CommandTimeout = TimeoutSeconds;
                    IDbDataParameter parameter = command.CreateParameter(); //command.Parameters.AddWithValue("@keyValue", keyValue);
                    parameter.ParameterName = "@keyValue"; parameter.Value = keyValue;
                    command.Parameters.Add(parameter);
                    return ExecuteScalarWithRetry(command) as byte[];
                }
            }
            finally { _database.ReleaseConnection(connection); } // la connessione viene chiusa se non c'è transazione
        }
        public void WriteBlob(string tableName, string keyField, object keyValue, string blobField, byte[] data, int pageNumber, string transactionId = null)
        {
            if (_transactionId != transactionId) RollBackDefaulTransaction("WriteBlob");
            int offset = pageNumber * PageSize;
            int length = Math.Min(PageSize, data.Length - offset);
            string sql = $"UPDATE {tableName} SET {blobField}.WRITE(@data, {offset}, {length}) WHERE {keyField} = @keyValue";
            IDbConnection connection = _database.NewConnection();
            try
            {
                using (IDbCommand command = _database.NewCommand(sql, connection)) // la transazione viene passate nel NewCommand
                {
                    command.CommandTimeout = TimeoutSeconds;
                    IDbDataParameter parameter1 = command.CreateParameter(); //command.Parameters.AddWithValue("@data", data.Skip(offset).Take(length).ToArray());
                    parameter1.ParameterName = "@data"; parameter1.Value = data.Skip(offset).Take(length).ToArray();
                    command.Parameters.Add(parameter1);
                    IDbDataParameter parameter2 = command.CreateParameter(); //command.Parameters.AddWithValue("@keyValue", keyValue);
                    parameter2.ParameterName = "@keyValue"; parameter2.Value = keyValue;
                    command.Parameters.Add(parameter2);
                    int affectedRows = ExecuteNonQueryWithRetry(command);
                }
            }
            finally { _database.ReleaseConnection(connection); } // la connessione viene chiusa se non c'è transazione
        }

        //ExecuteQuery
        public DataTable ExecuteQuery(string sql, IDictionary<string, object> parameters, int maxRecords = 10000, string transactionId = null)
        {
            if (_transactionId != transactionId) RollBackDefaulTransaction("ExecuteQuery");
            IDbConnection connection = _database.NewConnection();
            try
            {
                using (IDbCommand command = _database.NewCommand(sql, connection)) // la transazione viene passate nel NewCommand
                {
                    command.CommandTimeout = TimeoutSeconds;
                    AddParametersToCommand(command, parameters);
                    return ExecuteReaderWithRetry(command, maxRecords);
                }
            }
            finally { _database.ReleaseConnection(connection); } // la connessione viene chiusa se non c'è transazione
        }

        //ExecNonQuery
        public void DeleteRecord(string tableName, string keyField, IDictionary<string, object> fields, string transactionId = null)
        {
            if (_transactionId != transactionId) RollBackDefaulTransaction("DeleteRecord");
            string sql = $"DELETE FROM {tableName} WHERE {keyField} = @keyField";
            var parameters = new Dictionary<string, object>
                    {
                        { keyField, fields[keyField] }
                    };
            int affectedRows = ExecuteNonQuery(sql, parameters, transactionId);
        }

        //private

        private object ExecuteScalarWithRetry(IDbCommand command)
        {
            return ExecuteWithRetry(() => command.ExecuteScalar());
        }
        private int ExecuteNonQueryWithRetry(IDbCommand command)
        {
            return ExecuteWithRetry(() => command.ExecuteNonQuery());
        }
        //---
        private void AddParametersToCommand(IDbCommand command, IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                if (EnableTrace)
                {
                    _logger.Trace($"Executing SQL: {command.CommandText} with parameters: null");
                }
                return;
            }
            foreach (var param in parameters)
            {
                IDbDataParameter parameter = command.CreateParameter(); //command.Parameters.AddWithValue($"@{param.Key}", param.Value ?? DBNull.Value);
                parameter.ParameterName = $"@{param.Key}"; parameter.Value = param.Value ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
            if (EnableTrace)
            {
                _logger.Trace($"Executing SQL: {command.CommandText} with parameters: {string.Join(", ", parameters.Select(p => $"{p.Key}={p.Value}"))}");
            }
        }
        private DataTable ExecuteReaderWithRetry(IDbCommand command, int maxRecords)
        {
            return ExecuteWithRetry(() =>
            {
                DataTable result = _database.QueryReader(command, maxRecords);
                if (result.Rows.Count > maxRecords)
                {
                    throw new InvalidOperationException($"Query returned more than the allowed {maxRecords} records.");
                }
                return result;
            });
        }

        //---
        private int ExecuteNonQuery(string sql, IDictionary<string, object> parameters, string transactionId = null)
        {
            if (_transactionId != transactionId) RollBackDefaulTransaction("ExecuteNonQuery");
            IDbConnection connection = _database.NewConnection();
            try
            {
                using (IDbCommand command = _database.NewCommand(sql, connection)) // la transazione viene passate nel NewCommand
                {
                    command.CommandTimeout = TimeoutSeconds;
                    AddParametersToCommand(command, parameters);
                    return ExecuteNonQueryWithRetry(command);
                }
            }
            finally { _database.ReleaseConnection(connection); } // la connessione viene chiusa se non c'è transazione
        }
        private byte[] GenerateTimestamp()
        {
            byte[] timestamp = new byte[8];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(timestamp);
            }
            return timestamp;
        }
        private T ExecuteWithRetry<T>(Func<T> operation)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    return operation();
                }
                catch (DbException ex) when (retryCount < MaxRetries && _database.IsTransient((Exception)ex))  // IsTransient = true se è errore per cui conviene fare un retry
                {
                    retryCount++;
                    Thread.Sleep(DelayBetweenRetriesMs);
                }
                catch (Exception ex)
                {
                    HandleException(ex, ERR_DB_ERROR, "Database operation failed.");
                    throw; // Rethrow to ensure we do not swallow the exception
                }
            }
        }
        private void HandleException(Exception ex, int errorCode, string message)
        {
            _logger.Error(ex, $"{message} ErrorCode: {errorCode}");
            if (!_database.HandleException(ex)) throw new DatabaseException(ERR_DB_ERROR, "{message} ({errorCode})", ex);
        }


        //***************************************************************************************************************************************************
        //*** IMPORT-EXPORT CSV
        //***************************************************************************************************************************************************

        //public

        public void ExportTableToCsv(string tableName, string filePath, string whereClause = null, int chunkSize = 10000)
        {
            int offset = 0;
            bool hasMoreData = true;
            int fileCount = 1;
            string baseFilePath = filePath;

            while (hasMoreData)
            {
                string sql = $"SELECT * FROM {tableName} {(string.IsNullOrEmpty(whereClause) ? "" : "WHERE " + whereClause)} ORDER BY (SELECT NULL) OFFSET {offset} ROWS FETCH NEXT {chunkSize} ROWS ONLY";
                var dataTable = ExecuteQuery(sql, new Dictionary<string, object>(), chunkSize, null);
                string currentFilePath = fileCount == 1 ? filePath : $"{baseFilePath}_{fileCount}.csv";

                WriteDataTableToCsv(dataTable, currentFilePath);

                if (new FileInfo(currentFilePath).Length >= MaxFileLengthBytes)
                {
                    fileCount++;
                }

                hasMoreData = dataTable.Rows.Count == chunkSize;
                offset += chunkSize;
            }
        }
        public void ImportCsvToTable(string tableName, string filePath)
        {
            int fileCount = 1;
            string currentFilePath = filePath;
            bool moreFilesToProcess = true;

            while (moreFilesToProcess)
            {
                var dataTable = new DataTable();
                moreFilesToProcess = LoadCsvChunkIntoDataTable(currentFilePath, ref dataTable);

                while (dataTable.Rows.Count > 0)
                {
                    BulkInsertDataTable(tableName, dataTable);
                    moreFilesToProcess = LoadCsvChunkIntoDataTable(currentFilePath, ref dataTable);
                }

                fileCount++;
                currentFilePath = $"{filePath}_{fileCount}.csv";

                moreFilesToProcess = File.Exists(currentFilePath);
            }
        }

        //private

        private void BulkInsertDataTable(string tableName, DataTable dataTable)
        {
            try
            {
                string[] columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
                string insertCols = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ";
                StringBuilder sql = new StringBuilder();
                var parameters = new Dictionary<string, object>();
                if (1 == 0)   // eseguo n comandi di insert
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string insertValues = $"({string.Join(",", row.ItemArray)})";
                        sql.Append(insertCols).Append(insertValues).Append("; \n");
                    }
                }
                else       // eseguo un solo comando di insert eg: INSERT INTO items (embedding) VALUES ('[1,2,3]'), ('[4,5,6]');
                {
                    sql.Append(insertCols); 
                    for(int r=0; r<dataTable.Rows.Count; r++)
                    {
                        var row = dataTable.Rows[r];
                        if (r != 0) sql.Append(',');
                        sql.Append('(');
                        for (int c = 0; c < columnNames.Length; c++)
                        {
                            if (c != 0) sql.Append(',');
                            sql.Append($"@{columnNames[c]}__{r+1}");
                            parameters[$"@{columnNames[c]}__{r + 1}"] = row[columnNames[c]];  //parameters[$"@{columnNames[c]}__{r+1}"] = EncodeSpecialFields(row[columnNames[c]]);
                        }
                        sql.Append(')');
                    }
                    sql.Append("; \n");
                }
                int affectedRows = ExecuteNonQuery(sql.ToString(), parameters, null);
                if (affectedRows != dataTable.Rows.Count) throw new DatabaseException(ERR_DB_ERROR, " {dataTable.Rows-affectedRows} records non inseriti.", null);
            }
            catch (Exception ex)
            {
                HandleException(ex, ERR_DB_BADDATA, "Failed to bulk insert data.");
            }
        }

        private bool LoadCsvChunkIntoDataTable(string filePath, ref DataTable dataTable)
        {
            const int batchSize = 5000; // Carica i dati in blocchi di 5000 righe
            using (StreamReader reader = new StreamReader(filePath))
            using (CsvReader csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                //csv.Configuration.HasHeaderRecord = dataTable.Columns.Count == 0;

                if (dataTable.Columns.Count == 0)  //if (csv.Configuration.HasHeaderRecord)
                {
                    foreach (string header in csv.Context.Reader.HeaderRecord)
                    {
                        dataTable.Columns.Add(header);
                    }
                }

                var records = csv.GetRecords<dynamic>().Take(batchSize);
                foreach (var record in records)
                {
                    var row = dataTable.NewRow();
                    foreach (var field in record)
                    {
                        row[field.Key] = field.Value;
                    }
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable.Rows.Count == batchSize;
        }
        private void WriteDataTableToCsv(DataTable dataTable, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            using (CsvWriter csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                if (new FileInfo(filePath).Length == 0) // Se il file è vuoto, scrivi l'intestazione
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        csv.WriteField(column.ColumnName);
                    }
                    csv.NextRecord();
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column in row.Table.Columns)
                    {
                        csv.WriteField(row[column]);
                    }
                    csv.NextRecord();
                }
            }
        }



        //***************************************************************************************************************************************************
        //*** MANTAIN
        //***************************************************************************************************************************************************


        public void MantainRecord(char action, string tableName, string keyField, string timestampField, string deleteField, IDictionary<string, object> fields, string options, string transactionId = null)
        {
            int recNum = 1;
            VerifyTransactionId("MantainRecord", transactionId) ;
            string sql = SqlMantain(recNum, action, tableName, keyField, timestampField, deleteField, ref fields, options);
            var parameters = ParametersMantain(recNum, fields, options);

            int affectedRows = ExecuteNonQuery(sql, parameters, transactionId);
            //if (affectedRows != recNum) throw new DatabaseException(ERR_DB_TIMESTAMP, "Timestamp non valido o errore in insert/update.", null);
            if (affectedRows != 1)
            {
                if (action == 'A') throw new DatabaseException(ERR_DB_ERROR, "Record non inserito.", null);
                else throw new DatabaseException(ERR_DB_TIMESTAMP, "Timestamp non valido.", null);
            }
        }
        private void VerifyTransactionId(string funcName, string transactionId)
        {
            if (_transactionId != transactionId) RollBackDefaulTransaction(funcName);
        }
        private string SqlMantain(int recNum, char action, string tableName, string keyField, string timestampField, string deleteField, ref IDictionary<string, object> fields, string options)
        {
            string sql;

            if (!("AMD").Contains(action)) throw new DatabaseException(ERR_BAD_INPUT, "Valore azione errato.", null);
            if (string.IsNullOrEmpty(tableName)) throw new DatabaseException(ERR_NO_INPUT, "Nome tabella non presente.", null);
            if (!fields.ContainsKey(keyField)) throw new DatabaseException(ERR_NO_INPUT, "Identificativo univoco non presente.", null);
            if (String.IsNullOrEmpty((string)fields[keyField])) throw new DatabaseException(ERR_BAD_IDEN, "Identificativo univoco vuoto.", null);

            if (action == 'A')  //Add
            {
                sql = $"INSERT INTO {tableName} ({string.Join(", ", fields.Keys)}) VALUES ({string.Join(", ", fields.Keys.Select(k => "@{k}__{recNum}"))})";
            }
            else if (action == 'M')  //Modify
            {
                if (!fields.ContainsKey(timestampField)) throw new DatabaseException(ERR_NO_INPUT, "Timestamp non presente.", null);
                if (fields.ContainsKey($"OldTimestamp")) throw new DatabaseException(ERR_BAD_INPUT, "Il campo OldTimestamp non è consentito.", null);

                sql = $"UPDATE {tableName} SET {string.Join(", ", fields.Where(f => f.Key != keyField).Select(f => $"{f.Key} = @{f.Key}__{recNum}"))} WHERE {keyField} = @{keyField}__{recNum} and {timestampField} = @OldTimestamp__{recNum}";
                byte[] oldTimestamp = (byte[])fields[timestampField];  // salvo valore vecchi timestamp
                fields[timestampField] = GenerateTimestamp(); // genero valore nuovo timestamp
                fields[$"OldTimestamp"] = oldTimestamp;  // aggiungo il parametro relativo al vecchio timestamp
            }
            else if (action == 'D')  //Delete ==> Delete logico non fisico.
                                     //La cancellazione logica consente di replicare in modo asincrono l'azione su altri DB.
                                     //Per non vincolare l'integrità referenziale devo cancellare dal record tutte le chiavi esterne.  Assumo che queste cancellazioni siano passate nei fields 
            {
                if (!fields.ContainsKey(deleteField)) throw new DatabaseException(ERR_NO_INPUT, "Timestamp non presente.", null);
                if (fields.ContainsKey($"OldTimestamp")) throw new DatabaseException(ERR_BAD_INPUT, "Il campo OldTimestamp non è consentito.", null);

                sql = $"UPDATE {tableName} SET {deleteField} = 'Y', {string.Join(", ", fields.Where(f => f.Key != keyField && f.Key != deleteField).Select(f => $"{f.Key} = @{f.Key}__{recNum}"))} WHERE {keyField} = @{keyField}__{recNum} and {timestampField} = @OldTimestamp__{recNum}";
                byte[] oldTimestamp = (byte[])fields[timestampField];  // salvo valore vecchi timestamp
                fields[timestampField] = GenerateTimestamp(); // genero valore nuovo timestamp
                fields[$"OldTimestamp"] = oldTimestamp;  // aggiungo il parametro relativo al vecchio timestamp
            }
            else throw new DatabaseException(ERR_BAD_INPUT, "Azione non presente.", null);
            return sql;
        }
        private Dictionary<string, object> ParametersMantain(int recNum, IDictionary<string, object> fields, string options)
        {
            var parameters = new Dictionary<string, object>();

            foreach (var field in fields)
            {
                parameters[$"@{field.Key}__{recNum}"] = field.Value;    // parameters[$"@{field.Key}__{recNum}"] = EncodeSpecialFields(field.Value);
            }
            return parameters;
        }









    }
}