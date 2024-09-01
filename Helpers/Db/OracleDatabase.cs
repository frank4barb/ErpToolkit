using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;
using static ErpToolkit.Helpers.ErpError;

namespace ErpToolkit.Helpers.Db
{
    public class OracleDatabase : IDatabase
    {
        private string _connectionString;
        private OracleTransaction _transaction = null;
        private static readonly object _lock = new object();

        public OracleDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        //Gestione connessione
        private OracleConnection OpenConnection()
        {
            OracleConnection connection = null;
            lock (_lock)
            {
                connection = new OracleConnection(_connectionString);
                connection.Open();
                return connection;
            }
        }
        private void CloseConnection(OracleConnection connection)
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
            if (_transaction == null) CloseConnection((OracleConnection)connection);
        }

        public IDbCommand NewCommand(string sql, IDbConnection connection)
        {
            OracleCommand command = new OracleCommand(sql, (OracleConnection)connection);
            if (_transaction != null) command.Transaction = _transaction;
            return (IDbCommand)command;
        }
        public DataTable QueryReader(IDbCommand command, int maxRecords)
        {
            using (OracleDataAdapter adapter = new OracleDataAdapter((OracleCommand)command))
            {
                DataTable result = new DataTable();
                adapter.Fill(0, maxRecords, result); // restituisce maxRecords righe  //adapter.Fill(result);  
                return result;
            }
        }

        //*******************************************************************************************************

        // Gestione transazioni
        public void BeginTransaction(string transactionName)
        {
            OracleConnection connection = OpenConnection();
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
            OracleConnection connection = _transaction.Connection;
            try { _transaction.Commit(); _transaction.Dispose(); _transaction = null; }
            finally { CloseConnection(connection); }
        }
        public void RollbackTransaction(string transactionName)
        {
            if (_transaction == null) throw new DatabaseException(ERR_DB_TRANSACTION, "RollbackTransaction attempted for the wrong transaction ({transactionName}).");
            OracleConnection connection = _transaction.Connection;
            try { _transaction.Rollback(); _transaction.Dispose(); _transaction = null; }
            finally { CloseConnection(connection); }
        }

        //*******************************************************************************************************

        //errori per cui conviene fare un retry
        public bool IsTransient(Exception ex)
        {
            if (ex is OracleException oracleEx)  //??????????????????????????????????????????????
            {
                //return oracleEx.Number == -2 || oracleEx.Number == 1205; // Timeout or Deadlock
                //// Errori transitori tipici di Oracle
                //case 4068:  // SQL package state reset
                //case 1033:  // ORA-01033: ORACLE initialization or shutdown in progress
                //case 1034:  // ORA-01034: ORACLE not available
                return oracleEx.Number == 4068 || oracleEx.Number == 1033 
                    || oracleEx.Number == 1034 || oracleEx.Number == 12170; 
            }
            return false;
        }

        // decodifica errore per sqlserver
        public bool HandleException(Exception ex)
        {
            if (ex is OracleException oracleEx)
            {
                switch (oracleEx.Number)  //?????????????????????????????????????
                {
                    case 1017: // ORA-01017: invalid username/password; logon denied
                        throw new DatabaseException(ERR_DB_USE, "Invalid username or password.", ex);
                    case 12170: // ORA-12170: TNS:Connect timeout occurred
                        throw new DatabaseException(ERR_DB_TIMEOUT, "Connection timeout.", ex);
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


        public void BulkInsertDataTable(string tableName, DataTable dataTable)
        {
            throw new DatabaseException(ERR_DB_DUPLICATION, "BulkInsertDataTable non supportato.", null);
        }


    }
}




