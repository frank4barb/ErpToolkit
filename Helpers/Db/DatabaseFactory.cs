
using K4os.Compression.LZ4.Internal;
using System.Data.Entity;
using System.Reflection.Metadata;

namespace ErpToolkit.Helpers.Db
{
    // Funzioni di gestione accesso al Database, indipendentemente dal DBMS
    public class DatabaseFactory : IDisposable
    {

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


        public DatabaseManager GetDatabase(string dbType, string connectionStringName, string databaseName = "")
        {
            string key = dbType + "***" + connectionStringName;
            DatabaseManager db = null; if (_itemsDB.ContainsKey(key)) db = _itemsDB[key];
            if (db == null)
            {
                IDatabase idb = null;
                string connectionString = ErpContext.Instance.GetString(connectionStringName); 
                if (connectionString == "") throw new ArgumentException("Errore: connectionString vuota (" + connectionStringName + ") ");
                switch (dbType)
                {
                    case "SqlServer":
                        idb = new SqlServerDatabase(connectionString);
                        break;
                    case "Sybase":
                        idb = new SybaseDatabase(connectionString);
                        break;
                    case "MySql":
                        idb = new MySqlDatabase(connectionString);
                        break;
                    case "PostgreSql":
                        idb = new PostgreSqlDatabase(connectionString);
                        break;
                    case "SQLite":
                        idb = new SQLiteDatabase(connectionString);
                        break;
                    case "Oracle":
                        idb = new OracleDatabase(connectionString);
                        break;
                    case "IRIS":
                        idb = new IRISDatabase(connectionString);
                        break;
                    case "MongoDb":
                        idb = new MongoDbDatabase(connectionString, databaseName);
                        break;
                    // Aggiungi altri DBMS qui
                    default:
                        throw new ArgumentException("Tipo di database {dbType} non supportato");
                }
                if (idb == null) throw new ArgumentException("Errore: impossibile creare db {dbType} {databaseName}  ({connectionString}) ");
                _itemsDB[key] = new DatabaseManager(dbType, idb) ;
            }
            return _itemsDB[key];
        }

    }
}