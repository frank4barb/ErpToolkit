using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using static ErpToolkit.Helpers.ErpError;

namespace ErpToolkit.Helpers.Db
{
    public class SQLiteDatabase : IDatabase
    {
        private string _connectionString;
        private SQLiteTransaction _transaction = null;
        private static readonly object _lock = new object();

        public SQLiteDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        //Gestione connessione
        private SQLiteConnection OpenConnection()
        {
            SQLiteConnection connection = null;
            lock (_lock)
            {
                connection = new SQLiteConnection(_connectionString);
                connection.Open();
                return connection;
            }
        }
        private void CloseConnection(SQLiteConnection connection)
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
            if (_transaction == null) CloseConnection((SQLiteConnection)connection);
        }

        public IDbCommand NewCommand(string sql, IDbConnection connection)
        {
            return (IDbCommand)new SQLiteCommand(sql, (SQLiteConnection)connection, _transaction);
        }
        public DataTable QueryReader(IDbCommand command, int maxRecords)
        {
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter((SQLiteCommand)command))
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
            SQLiteConnection connection = OpenConnection();
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
            SQLiteConnection connection = _transaction.Connection;
            try { _transaction.Commit(); _transaction.Dispose(); _transaction = null; }
            finally { CloseConnection(connection); }
        }
        public void RollbackTransaction(string transactionName)
        {
            if (_transaction == null) throw new DatabaseException(ERR_DB_TRANSACTION, "RollbackTransaction attempted for the wrong transaction ({transactionName}).");
            SQLiteConnection connection = _transaction.Connection;
            try { _transaction.Rollback(); _transaction.Dispose(); _transaction = null; }
            finally { CloseConnection(connection); }
        }

        //*******************************************************************************************************

        //errori per cui conviene fare un retry
        public bool IsTransient(Exception ex)
        {
            if (ex is SQLiteException sqliteEx)  //?????????????????????????????????
            {
                //return sqliteEx.ErrorCode == -2 || sqliteEx.ErrorCode == 1205; // Timeout or Deadlock
                switch (sqliteEx.ErrorCode)
                {
                    case (int)SQLiteErrorCode.Busy: // Database busy
                    case (int)SQLiteErrorCode.Locked: // Database locked
                        return true;
                }
            }
            return false;
        }

        // decodifica errore per sqlserver
        public bool HandleException(Exception ex)
        {
            if (ex is SQLiteException sqliteEx)
            {
                switch (sqliteEx.ErrorCode) //?????????????????????????????????
                {
                    case (int)SQLiteErrorCode.Error: // Generic error
                        throw new DatabaseException(ERR_DB_UNKNOWN, "SQLite generic error.", ex);
                    case (int)SQLiteErrorCode.IoErr: // IO error
                        throw new DatabaseException(ERR_DB_ERROR, "SQLite IO error.", ex);
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

