
using System.Data;
using System.Data.Common;
using InterSystems.Data.IRISClient;
using static ErpToolkit.Helpers.ErpError;

namespace ErpToolkit.Helpers.Db
{
    public class IRISDatabase : IDatabase, IDisposable
    {
        private string _connectionString;
        private IRISTransaction _transaction = null;
        private static readonly object _lock = new object();

        public IRISDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }
        ~IRISDatabase()
        {
            Dispose();
        }
        public void Dispose()
        {
            try { RollbackTransaction("Dispose"); } catch (Exception ex) { /*skip*/ }
            GC.SuppressFinalize(this);
        }

        //Gestione connessione
        private IRISConnection OpenConnection()
        {
            IRISConnection connection = null;
            lock (_lock)
            {
                connection = new IRISConnection(_connectionString);
                connection.Open();
                return connection;
            }
        }
        private void CloseConnection(IRISConnection connection)
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
            if (_transaction == null) CloseConnection((IRISConnection)connection);
        }

        public IDbCommand NewCommand(string sql, IDbConnection connection)
        {
            return (IDbCommand)new IRISCommand(sql, (IRISConnection)connection, _transaction);
        }
        public DataTable QueryReader(IDbCommand command, int maxRecords)
        {
            using (IRISDataAdapter adapter = new IRISDataAdapter((IRISCommand)command))
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
            IRISConnection connection = OpenConnection();
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
            IRISConnection connection = (IRISConnection)_transaction.Connection;
            try { _transaction.Commit(); _transaction.Dispose(); _transaction = null; }
            finally { CloseConnection(connection); }
        }
        public void RollbackTransaction(string transactionName)
        {
            if (_transaction == null) throw new DatabaseException(ERR_DB_TRANSACTION, "RollbackTransaction attempted for the wrong transaction ({transactionName}).");
            IRISConnection connection = (IRISConnection)_transaction.Connection;
            try { _transaction.Rollback(); _transaction.Dispose(); _transaction = null; }
            finally { CloseConnection(connection); }
        }

        //*******************************************************************************************************

        //errori per cui conviene fare un retry
        public bool IsTransient(Exception ex)
        {
            if (ex is IRISException irisEx) 
            {
                return irisEx.ErrorCode == -140 || irisEx.ErrorCode == -121; // Timeout or Deadlock
            }
            return false;
        }

        // decodifica errore per IRIS
        //public bool HandleException(Exception ex)
        //{
        //    if (ex is IRISException irisEx)
        //    {
        //        switch (irisEx.ErrorCode)  //??????????????????????????????
        //        {
        //            case 101: // Invalid object name
        //                throw new DatabaseException(ERR_DB_UNKNOWN, "Invalid object name.", ex);
        //            case -2:  // Timeout expired
        //                throw new DatabaseException(ERR_DB_TIMEOUT, "Timeout expired.", ex);
        //            //case 2601:
        //            //case 2627:
        //            //    throw new DatabaseException(ERR_DB_DUPLICATION, "Unique constraint violated.", ex);
        //            //case 547:
        //            //    throw new DatabaseException(ERR_DB_DEPENDENCY, "Cannot delete or update due to foreign key constraint.", ex);
        //            //case 1205:
        //            //    throw new DatabaseException(ERR_DB_DEADLOCK, "Deadlock encountered.", ex);
        //            //case 208:
        //            //    throw new DatabaseException(ERR_DB_UNKNOWN, "Invalid object name.", ex);
        //            //case -2:
        //            //    throw new DatabaseException(ERR_DB_TIMEOUT, "Timeout expired.", ex);
        //            default:
        //                throw new DatabaseException(ERR_DB_BADCOLUMN, "An SQL error occurred.", ex);
        //        }
        //    }
        //    else return false;
        //}
        public bool HandleException(Exception ex)
        {
            if (ex is IRISException irisEx)
            {
                // IRIS di InterSystems - personalizzazione necessaria per SQLCODE specifici.
                switch (irisEx.ErrorCode) //switch (irisEx.Sqlcode)
                {
                    case -119:
                        throw new DatabaseException(ERR_DB_DUPLICATION, "Violazione del vincolo univoco.", ex);
                    case -120:
                        throw new DatabaseException(ERR_DB_DEPENDENCY, "Violazione del vincolo di chiave esterna.", ex);
                    case -121:
                        throw new DatabaseException(ERR_DB_DEADLOCK, "Deadlock.", ex);
                    case -100:
                        throw new DatabaseException(ERR_DB_UNKNOWN, "Tabella non esistente.", ex);
                    case -138:
                        throw new DatabaseException(ERR_DB_BADCOLUMN, "Colonna non esistente.", ex); // Nome campo inesistente
                    case -140:
                        throw new DatabaseException(ERR_DB_TIMEOUT, "Timeout.", ex);
                    default:
                        throw new DatabaseException(ERR_DB_ERROR, "Errore IRIS.", ex);
                }
            }
            else return false;
        }


        //*******************************************************************************************************


    }
}



