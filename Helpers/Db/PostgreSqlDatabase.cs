using System.Data;
using System.Data.Common;
using Npgsql;
using NpgsqlTypes;
using static ErpToolkit.Helpers.ErpError;

namespace ErpToolkit.Helpers.Db
{
    public class PostgreSqlDatabase : IDatabase
    {
        private string _connectionString;
        private NpgsqlTransaction _transaction = null;
        private static readonly object _lock = new object();

        public PostgreSqlDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        //Gestione connessione
        private NpgsqlConnection OpenConnection()
        {
            NpgsqlConnection connection = null;
            lock (_lock)
            {
                connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                return connection;
            }
        }
        private void CloseConnection(NpgsqlConnection connection)
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
            if (_transaction == null) CloseConnection((NpgsqlConnection)connection);
        }

        public IDbCommand NewCommand(string sql, IDbConnection connection)
        {
            return (IDbCommand)new NpgsqlCommand(sql, (NpgsqlConnection)connection, _transaction);
        }
        public DataTable QueryReader(IDbCommand command, int maxRecords)
        {
            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter((NpgsqlCommand)command))
            {
                DataTable result = new DataTable();
                if (maxRecords < 0) adapter.Fill(result);
                else adapter.Fill(0, maxRecords, result); // restituisce maxRecords righe  
                return result;
            }
        }

        //*******************************************************************************************************

        // Gestione transazioni
        public void BeginTransaction(string transactionName)
        {
            NpgsqlConnection connection = OpenConnection();
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
            NpgsqlConnection connection = _transaction.Connection;
            try { _transaction.Commit(); _transaction.Dispose(); _transaction = null; }
            finally { CloseConnection(connection); }
        }
        public void RollbackTransaction(string transactionName)
        {
            if (_transaction == null) throw new DatabaseException(ERR_DB_TRANSACTION, "RollbackTransaction attempted for the wrong transaction ({transactionName}).");
            NpgsqlConnection connection = _transaction.Connection;
            try { _transaction.Rollback(); _transaction.Dispose(); _transaction = null; }
            finally { CloseConnection(connection); }
        }

        //*******************************************************************************************************

        //errori per cui conviene fare un retry
        public bool IsTransient(Exception ex)
        { 
            if (ex is NpgsqlException npgsqlEx)  //???????????????????????????????????
            {
                //return npgsqlEx.ErrorCode == -2 || npgsqlEx.ErrorCode == 1205; // Timeout or Deadlock

                //case "40001": // Serialization failure
                //case "40P01": // Deadlock detected
                return npgsqlEx.SqlState == "40001" || npgsqlEx.SqlState == "40P01";
                }
                return false;
        }

        // decodifica errore per sqlserver
        public bool HandleException(Exception ex)
        {
            if (ex is NpgsqlException npgsqlEx)  //???????????????????????????????????
            {
                switch (npgsqlEx.SqlState)
                {
                    case "42P01": // Undefined table
                        throw new DatabaseException(ERR_DB_UNKNOWN, "Undefined table.", ex);
                    case "57014": // Query canceled
                        throw new DatabaseException(ERR_DB_TIMEOUT, "Query canceled by user.", ex);
                    //case 2601:
                    //case 2627:
                    //    throw new DatabaseException(ERR_DB_DUPLICATION, "Unique constraint violated.", ex);
                    //case 547:
                    //    throw new DatabaseException(ERR_DB_DEPENDENCY, "Cannot delete or update due to foreign key constraint.", ex);
                    //case 1205:
                    //    throw new DatabaseException(ERR_DB_DEADLOCK, "Deadlock encountered.", ex);
                    //case 208:
                    //    throw new DatabaseException(ERR_DB_UNKNOWN, "Invalid object name.", ex);
                    //case -2:
                    //    throw new DatabaseException(ERR_DB_TIMEOUT, "Timeout expired.", ex);
                    default:
                        throw new DatabaseException(ERR_DB_ERROR, "An SQL error occurred.", ex);
                }
            }
            else return false;
        }


        //*******************************************************************************************************



    }
}



