using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using NLog;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Linq;
using System.Formats.Asn1;
using static ErpToolkit.Helpers.ErpError;

namespace ErpToolkit.Helpers.Db
{
    public class SqlServerDatabase : IDatabase
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        private static readonly object _lock = new object();
        private static NLog.ILogger _logger;

        // Proprietà configurabili
        public int PageSize { get; set; } = 1000;
        public int MaxRetries { get; set; } = 3;
        public int DelayBetweenRetriesMs { get; set; } = 1000;
        public int TimeoutSeconds { get; set; } = 30;
        public int MaxRecords { get; set; } = 10000;
        public bool EnableTrace { get; set; } = false;

        public SqlServerDatabase(string connStringName)
        {

            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
            //set connection string
            _connectionString = ErpContext.Instance.GetString(connStringName); //string connectionString = ErpContext.Instance.GetParam("#connectionString_IRISLive");
            if (_connectionString == "") HandleException(null, ERR_DB_ERROR, "Errore: connectionString vuota (" + connStringName + ") "); 
            OpenConnection();
        }

        private void OpenConnection()
        {
            lock (_lock)
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                {
                    _connection = new SqlConnection(_connectionString);
                    try
                    {
                        _connection.Open();
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex, ERR_DB_USE, "Failed to open connection.");
                    }
                }
            }
        }

        private void EnsureConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                OpenConnection();
            }
        }

        private void HandleException(Exception ex, int errorCode, string defaultMessage)
        {
            string errorMessage = defaultMessage;

            if (ex is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2601: // Violation of unique index
                    case 2627: // Violation of primary key
                        errorCode = ERR_DB_DUPLICATION;
                        errorMessage = "Unique key violation.";
                        break;
                    case 547: // Foreign key violation
                        errorCode = ERR_DB_UNK_REF;
                        errorMessage = "Referential integrity violation.";
                        break;
                    case -2: // Timeout
                        errorCode = ERR_DB_TIMEOUT;
                        errorMessage = "Database timeout occurred.";
                        break;
                    case 1205: // Deadlock
                        errorCode = ERR_DB_DEADLOCK;
                        errorMessage = "Deadlock encountered.";
                        break;
                    default:
                        errorCode = ERR_DB_ERROR;
                        errorMessage = "A database error occurred.";
                        break;
                }
            }

            _logger.Error(ex, $"{errorMessage} (Error code: {errorCode})");
            throw new DatabaseException(errorCode, errorMessage, ex);
        }

        public void ExecuteNonQuery(string sql, IDbTransaction transaction = null)
        {
            EnsureConnection();
            if (EnableTrace)
            {
                _logger.Trace($"Executing SQL: {sql}");
            }

            using (var command = new SqlCommand(sql, _connection))
            {
                command.CommandTimeout = TimeoutSeconds;
                command.Transaction = (SqlTransaction)transaction;

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    HandleException(ex, ERR_DB_SYNTAX, "Failed to execute non-query SQL command.");
                }
            }
        }

        public DataTable ExecQuery(string sql, IDbTransaction transaction = null, int maxRecords = 10000)
        {
            EnsureConnection();
            if (EnableTrace)
            {
                _logger.Trace($"Executing SQL: {sql}");
            }

            using (var command = new SqlCommand(sql, _connection))
            {
                command.CommandTimeout = TimeoutSeconds;
                command.Transaction = (SqlTransaction)transaction;

                using (var adapter = new SqlDataAdapter(command))
                {
                    var result = new DataTable();
                    try
                    {
                        adapter.Fill(0, maxRecords, result);
                    }
                    catch (Exception ex)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                        HandleException(ex, ERR_DB_SYNTAX, "Failed to execute SQL query.");
                    }
                    return result;
                }
            }
        }

        public void InsertOrUpdateRecord(Dictionary<string, object> fields, string tableName, string keyField, string timestampField, bool isInsert, IDbTransaction transaction = null)
        {
            EnsureConnection();
            string sql = string.Empty;
            try
            {
                if (isInsert)
                {
                    var columns = string.Join(",", fields.Keys);
                    var values = string.Join(",", fields.Values.Select(v => $"'{v}'"));
                    sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
                }
                else
                {
                    if (!fields.ContainsKey(keyField) || !fields.ContainsKey(timestampField))
                    {
                        throw new ArgumentException("Key field or timestamp field not found in the provided data.");
                    }

                    var setClause = string.Join(",", fields.Where(f => f.Key != keyField && f.Key != timestampField).Select(f => $"{f.Key}='{f.Value}'"));
                    sql = $"UPDATE {tableName} SET {setClause}, {timestampField}=NEWID() WHERE {keyField}='{fields[keyField]}' AND {timestampField}='{fields[timestampField]}'";

                    int affectedRows = ExecuteNonQueryWithResult(sql, (SqlTransaction)transaction);

                    if (affectedRows == 0)
                    {
                        throw new DatabaseException(ERR_DB_TIMESTAMP, "Timestamp validation failed, record has been modified by another process.");
                    }
                }
                ExecuteNonQuery(sql, (SqlTransaction)transaction);
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                HandleException(ex, ERR_DB_SYNTAX, "Insert or update failed.");
            }
        }

        private int ExecuteNonQueryWithResult(string sql, SqlTransaction transaction)
        {
            EnsureConnection();
            if (EnableTrace)
            {
                _logger.Trace($"Executing SQL: {sql}");
            }

            using (var command = new SqlCommand(sql, _connection))
            {
                command.CommandTimeout = TimeoutSeconds;
                command.Transaction = transaction;

                try
                {
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }
                    HandleException(ex, ERR_DB_SYNTAX, "Failed to execute non-query SQL command with result.");
                    return 0; // This won't be reached, but needed to compile
                }
            }
        }

        public void DeleteRecord(string tableName, Dictionary<string, object> fields, string keyField, IDbTransaction transaction = null)
        {
            EnsureConnection();
            try
            {
                if (!fields.ContainsKey(keyField))
                {
                    throw new ArgumentException("Key field not found in the provided data.");
                }

                string sql = $"DELETE FROM {tableName} WHERE {keyField}='{fields[keyField]}'";

                ExecuteNonQuery(sql, (SqlTransaction)transaction);
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                HandleException(ex, ERR_DB_SYNTAX, "Failed to delete record.");
            }
        }

        public void ExportTableToCsv(string tableName, string filePath, string whereClause = null, int chunkSize = 10000)
        {
            int chunkIndex = 0;
            int offset = 0;

            while (true)
            {
                string chunkFilePath = $"{filePath}_chunk{chunkIndex}.csv";
                string query = $"SELECT * FROM {tableName}";

                if (!string.IsNullOrEmpty(whereClause))
                {
                    query += $" WHERE {whereClause}";
                }

                query += $" ORDER BY (SELECT NULL) OFFSET {offset} ROWS FETCH NEXT {chunkSize} ROWS ONLY";

                DataTable chunkData = ExecQuery(query);

                if (chunkData.Rows.Count == 0)
                {
                    break; // Fine dei dati
                }

                WriteDataTableToCsv(chunkData, chunkFilePath);
                chunkIndex++;
                offset += chunkSize;
            }
        }

        public void ImportCsvToTable(string tableName, string filePath, IDbTransaction transaction = null)
        {
            int chunkIndex = 0;

            while (true)
            {
                string chunkFilePath = $"{filePath}_chunk{chunkIndex}.csv";

                if (!File.Exists(chunkFilePath))
                {
                    break; // Nessun altro chunk trovato
                }

                DataTable chunkData = ReadCsvToDataTable(chunkFilePath);
                BulkInsertDataTable(tableName, chunkData, (SqlTransaction)transaction);
                chunkIndex++;
            }
        }

        private void WriteDataTableToCsv(DataTable dataTable, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            using (CsvWriter csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    csv.WriteField(column.ColumnName);
                }
                csv.NextRecord();

                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        csv.WriteField(row[column]);
                    }
                    csv.NextRecord();
                }
            }
        }

        private DataTable ReadCsvToDataTable(string filePath)
        {
            DataTable dataTable = new DataTable();

            using (StreamReader reader = new StreamReader(filePath))
            using (CsvReader csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                using (var dr = new CsvDataReader(csv))
                {
                    dataTable.Load(dr);
                }
            }

            return dataTable;
        }

        private void BulkInsertDataTable(string tableName, DataTable dataTable, SqlTransaction transaction = null)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BatchSize = dataTable.Rows.Count;

                foreach (DataColumn column in dataTable.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                try
                {
                    bulkCopy.WriteToServer(dataTable);
                }
                catch (Exception ex)
                {
                    HandleException(ex, ERR_DB_BADDATA, "Failed to bulk insert data.");
                }
            }
        }
    }
}
