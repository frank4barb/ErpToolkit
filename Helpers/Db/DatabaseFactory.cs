
using K4os.Compression.LZ4.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Data.Entity;
using System.Reflection.Metadata;

namespace ErpToolkit.Helpers.Db
{
    // Funzioni di gestione accesso al Database, indipendentemente dal DBMS
    public class DatabaseFactory : IDisposable
    {

        public enum DbTyp { SqlServer, Sybase, MySql, PostgreSql, SQLite, Oracle, IRIS, MongoDb }


        private IDictionary<string, DatabaseManager> _itemsDB = new Dictionary<string, DatabaseManager>();  //

        public DatabaseFactory()
        {
        }
        ~DatabaseFactory()
        {
            Dispose();
        }
        public void Dispose()
        {
            // Rilascia risorse non gestite
            if (_itemsDB != null)
            {
                foreach (var key in _itemsDB.Keys) { _itemsDB[key].Dispose(); _itemsDB.Remove(key); }
                _itemsDB.Clear(); _itemsDB = null;
            }
            //.........
            GC.SuppressFinalize(this);
        }

        private static readonly object _lockObject = new object();

        public DatabaseManager GetDatabase(DbTyp dbType, string connectionStringName, string databaseName = "")
        {
            if (String.IsNullOrWhiteSpace(connectionStringName)) throw new ArgumentException("Errore: GetDatabase: connectionStringName vuota.");
            string key = dbType.ToString() + "***" + connectionStringName;
            lock (_lockObject)
            {

                DatabaseManager db = null; if (_itemsDB.ContainsKey(key)) db = _itemsDB[key];
                if (db == null)
                {
                    IDatabase idb = null;
                    string connectionString = ErpContext.Instance.GetString(connectionStringName);
                    if (connectionString == "") throw new ArgumentException("Errore: connectionString vuota (" + connectionStringName + ") ");
                    switch (dbType)
                    {
                        case DbTyp.SqlServer:
                            idb = new SqlServerDatabase(connectionString);
                            break;
                        case DbTyp.Sybase:
                            idb = new SybaseDatabase(connectionString);
                            break;
                        case DbTyp.MySql:
                            idb = new MySqlDatabase(connectionString);
                            break;
                        case DbTyp.PostgreSql:
                            idb = new PostgreSqlDatabase(connectionString);
                            break;
                        case DbTyp.SQLite:
                            idb = new SQLiteDatabase(connectionString);
                            break;
                        case DbTyp.Oracle:
                            idb = new OracleDatabase(connectionString);
                            break;
                        case DbTyp.IRIS:
                            idb = new IRISDatabase(connectionString);
                            break;
                        case DbTyp.MongoDb:
                            idb = new MongoDbDatabase(connectionString, databaseName);
                            break;
                        // Aggiungi altri DBMS qui
                        default:
                            throw new ArgumentException("Tipo di database {dbType} non supportato");
                    }
                    if (idb == null) throw new ArgumentException("Errore: impossibile creare db {dbType} {databaseName}  ({connectionString}) ");
                    _itemsDB[key] = new DatabaseManager(dbType, idb);
                }
                return _itemsDB[key];
            }
        }

    }
}