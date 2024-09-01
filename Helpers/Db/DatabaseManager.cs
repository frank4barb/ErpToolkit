
using CsvHelper.Configuration;
using CsvHelper;
using System.Data;
using System.Globalization;
using System.Data.Common;
using static ErpToolkit.Helpers.ErpError;

namespace ErpToolkit.Helpers.Db
{
    // Funzioni di gestione accesso al Database, indipendentemente dal DBMS
    public class DatabaseManager
    {
        private readonly string _databaseType;
        private readonly IDatabase _database;
        private static NLog.ILogger _logger;

        private Stack<string> _transactionStack = new Stack<string>();
        private Timer _transactionTimeoutTimer;
        private string _transactionId = "";


        // Proprietà configurabili
        public int PageSize { get; set; } = 1000;  //ReadBlob, WriteBlob
        public int MaxRetries { get; set; } = 3;
        public int DelayBetweenRetriesMs { get; set; } = 1000;
        public int TimeoutSeconds { get; set; } = 30;
        public int TransactionTimeoutSeconds { get; set; } = 60;
        public int MaxRecords { get; set; } = 10000;
        public bool EnableTrace { get; set; } = false;
        public int MaxFileLengthBytes { get; set; } = 1024 * 1024 * 1024;  // 1 Gb

        public DatabaseManager(string databaseType, IDatabase database)
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
            //set database
            _databaseType = databaseType;
            _database = database;
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
            _transactionStack.Clear(); _transactionId = "";
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
        public DataTable ExecuteQuery(string sql, IDictionary<string, object> parameters, string transactionId = null, int maxRecords = 10000)
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
        public void InsertOrUpdateRecord(IDictionary<string, object> fields, string tableName, string keyField, string timestampField, bool isInsert, string transactionId = null)
        {
            if (_transactionId != transactionId) RollBackDefaulTransaction("InsertOrUpdateRecord");
            string sql;
            var parameters = new Dictionary<string, object>();

            if (!fields.ContainsKey(keyField)) throw new DatabaseException(ERR_NO_INPUT, "Identificativo univoco non presente.", null);
            if (String.IsNullOrEmpty((string)fields[keyField])) throw new DatabaseException(ERR_BAD_IDEN, "Identificativo univoco vuoto.", null);

            if (isInsert)
            {
                sql = $"INSERT INTO {tableName} ({string.Join(", ", fields.Keys)}) VALUES ({string.Join(", ", fields.Keys.Select(k => "@" + k))})";
            }
            else
            {
                if (!fields.ContainsKey(timestampField)) throw new DatabaseException(ERR_NO_INPUT, "Timestamp non presente.", null);

                sql = $"UPDATE {tableName} SET {string.Join(", ", fields.Where(f => f.Key != keyField).Select(f => $"{f.Key} = @{f.Key}"))} WHERE {keyField} = @{keyField} and {timestampField} = @oldTimestamp";
                byte[] oldTimestamp = (byte[])fields[timestampField];  // salvo valore vecchi timestamp
                fields[timestampField] = GenerateTimestamp(); // genero valore nuovo timestamp
                parameters[$"oldTimestamp"] = oldTimestamp;  // aggiungo il parametro relativo al vecchio timestamp
            }

            foreach (var field in fields)
            {
                parameters[field.Key] = EncodeSpecialFields(field.Value);
            }

            int affectedRows = ExecuteNonQuery(sql, parameters, transactionId);
            if (affectedRows != 1)
            {
                if (isInsert) throw new DatabaseException(ERR_DB_ERROR, "Record non inserito.", null);
                else throw new DatabaseException(ERR_DB_TIMESTAMP, "Timestamp non valido.", null);
            }
        }
        public void DeleteRecord(string tableName, IDictionary<string, object> fields, string keyField, string transactionId = null)
        {
            if (_transactionId != transactionId) RollBackDefaulTransaction("DeleteRecord");
            string sql = $"DELETE FROM {tableName} WHERE {keyField} = @keyField";
            var parameters = new Dictionary<string, object>
                    {
                        { keyField, fields[keyField] }
                    };
            int affectedRows = ExecuteNonQuery(sql, parameters, transactionId);
        }
        public void BulkInsertDataTable(string tableName, DataTable dataTable, string transactionId = null)
        {
            if (_transactionId != transactionId) RollBackDefaulTransaction("BulkInsertDataTable");
            try
            {
                _database.BulkInsertDataTable(tableName, dataTable);
            }
            catch (Exception ex)
            {
                HandleException(ex, ERR_DB_BADDATA, "Failed to bulk insert data.");
            }
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
        //*** INPORT-EXPORT CSV
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
                var dataTable = ExecuteQuery(sql, new Dictionary<string, object>(), null, chunkSize);
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
                    BulkInsertDataTable(tableName, dataTable, null);
                    moreFilesToProcess = LoadCsvChunkIntoDataTable(currentFilePath, ref dataTable);
                }

                fileCount++;
                currentFilePath = $"{filePath}_{fileCount}.csv";

                moreFilesToProcess = File.Exists(currentFilePath);
            }
        }

        //private

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
        //*** ENCODE-DECODE
        //***************************************************************************************************************************************************

        private object EncodeSpecialFields(object value)
        {
            if (value is DateOnly date)
                return date.ToString("yyyy/MM/dd");
            if (value is TimeOnly time)
                return time.ToString("HH:mm:ss");
            // Aggiungere altre conversioni speciali qui se necessario
            return value;
        }

        private object DecodeSpecialFields(Type type, object value)
        {
            if (type == typeof(DateOnly) && DateOnly.TryParseExact(value.ToString(), "yyyy/MM/dd", null, DateTimeStyles.None, out DateOnly date))
                return date;
            if (type == typeof(TimeOnly) && TimeOnly.TryParseExact(value.ToString(), "HH:mm:ss", null, DateTimeStyles.None, out TimeOnly time))
                return time;
            // Aggiungere altre conversioni speciali qui se necessario
            return value;
        }


    }
}