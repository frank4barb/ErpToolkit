
using System.Data;
using System.Data.Common;
using AdoNetCore.AseClient;
using static ErpToolkit.Helpers.ErpError;

namespace ErpToolkit.Helpers.Db
{
    public class SybaseDatabase : IDatabase
    {
        private string _connectionString;
        private AseTransaction _transaction = null;
        private static readonly object _lock = new object();

        public SybaseDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        //Gestione connessione
        private AseConnection OpenConnection()
        {
            AseConnection connection = null;
            lock (_lock)
            {
                connection = new AseConnection(_connectionString);
                connection.Open();
                return connection;
            }
        }
        private void CloseConnection(AseConnection connection)
        {
            lock (_lock)
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                    connection = null;
                }
            }
        }

        //Gestione connessione comando reader
        public IDbConnection NewConnection()
        {
            //return (IDbConnection)new SqlConnection(_connectionString);
            if (_transaction != null) return _transaction.Connection;
            else return (IDbConnection)OpenConnection();
        }
        public void ReleaseConnection(IDbConnection connection)
        {
            //return (IDbConnection)new SqlConnection(_connectionString);
            if (_transaction == null) CloseConnection((AseConnection)connection);
        }

        public IDbCommand NewCommand(string sql, IDbConnection connection)
        {
            return (IDbCommand)new AseCommand(sql, (AseConnection)connection, _transaction);
        }
        public DataTable QueryReader(IDbCommand command, int maxRecords)
        {
            //using (SqlDataAdapter adapter = new SqlDataAdapter((AseCommand)command))
            //{
            //    DataTable result = new DataTable();
            //    adapter.Fill(0, maxRecords, result); // restituisce maxRecords righe  //adapter.Fill(result);  
            //    return result;
            //}

            using (var reader = command.ExecuteReader())
            {
                var dataTable = new DataTable();
                dataTable.Load(reader);  //?????????????? manca filtro maxRecords
                return dataTable;
            }

        }

        //*******************************************************************************************************

        // Gestione transazioni
        public void BeginTransaction(string transactionName)
        {
            AseConnection connection = OpenConnection();
            try
            {
                _transaction = connection.BeginTransaction();
                if (_transaction == null) throw new DatabaseException(ERR_DB_TRANSACTION, "BeginTransaction attempted for the wrong transaction ({transactionName}).");
            }
            finally
            {
                if (_transaction == null) CloseConnection(connection);
            }
        }
        public void SavePointTransaction(string savePointName)
        {
            if (_transaction == null) throw new DatabaseException(ERR_DB_TRANSACTION, "SavePointTransaction attempted for the wrong transaction ({savePointName}).");
            _transaction.Save(savePointName);
        }
        public void RollbackSavePoint(string savePointName)
        {
            if (_transaction == null) throw new DatabaseException(ERR_DB_TRANSACTION, "RollbackSavePoint attempted for the wrong transaction ({savePointName}).");
            _transaction.Rollback(savePointName);
        }
        public void CommitSavePoint(string savePointName)
        {
            if (_transaction == null) throw new DatabaseException(ERR_DB_TRANSACTION, "CommitSavePoint attempted for the wrong transaction ({savePointName}).");
        }
        public void CommitTransaction(string transactionName)
        {
            if (_transaction == null) throw new DatabaseException(ERR_DB_TRANSACTION, "CommitTransaction attempted for the wrong transaction ({transactionName}).");
            AseConnection connection = _transaction.Connection;
            try { _transaction.Commit(); _transaction.Dispose(); _transaction = null; }
            finally { CloseConnection(connection); }
        }
        public void RollbackTransaction(string transactionName)
        {
            if (_transaction == null) throw new DatabaseException(ERR_DB_TRANSACTION, "RollbackTransaction attempted for the wrong transaction ({transactionName}).");
            AseConnection connection = _transaction.Connection;
            try { _transaction.Rollback(); _transaction.Dispose(); _transaction = null; }
            finally { CloseConnection(connection); }
        }

        //*******************************************************************************************************

        //errori per cui conviene fare un retry
        public bool IsTransient(Exception ex)
        {
            if (ex is AseException aseEx)  //uguali a sqlserver
            {
                return aseEx.HResult == -2 || aseEx.HResult == 1205; // Timeout or Deadlock
            }
            return false;
        }

        // decodifica errore per sqlserver
        public bool HandleException(Exception ex)
        {
            if (ex is AseException aseEx)
            {
                switch (aseEx.HResult)  //uguali a sqlserver
                {
                    case 2601:
                    case 2627:
                        throw new DatabaseException(ERR_DB_DUPLICATION, "Unique constraint violated.", ex);
                    case 547:
                        throw new DatabaseException(ERR_DB_DEPENDENCY, "Cannot delete or update due to foreign key constraint.", ex);
                    case 1205:
                        throw new DatabaseException(ERR_DB_DEADLOCK, "Deadlock encountered.", ex);
                    case 208:
                        throw new DatabaseException(ERR_DB_UNKNOWN, "Invalid object name.", ex);
                    case -2:
                        throw new DatabaseException(ERR_DB_TIMEOUT, "Timeout expired.", ex);
                    default:
                        throw new DatabaseException(ERR_DB_ERROR, "An SQL error occurred.", ex);
                }
            }
            else return false;
        }


        //*******************************************************************************************************


        public void BulkInsertDataTable(string tableName, DataTable dataTable)
        {
            throw new DatabaseException(ERR_DB_DUPLICATION, "BulkInsertDataTable non supportato.", null);
        }


    }
}