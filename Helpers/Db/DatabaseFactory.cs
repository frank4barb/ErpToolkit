
namespace ErpToolkit.Helpers.Db
{
    // Funzioni di gestione accesso al Database, indipendentemente dal DBMS
    public class DatabaseFactory
    {

        private IDictionary<string, IDatabase> _itemsDB = new Dictionary<string, IDatabase>();  //

        public DatabaseFactory()
        {
        }


        public IDatabase GetDatabase(string dbType, string connectionStringName)
        {
            string key = dbType + connectionStringName;
            IDatabase db = null; if (_itemsDB.ContainsKey(key)) db = _itemsDB[key];
            if (db == null)
            {
                string connectionString = ErpContext.Instance.GetString(connectionStringName); 
                if (connectionString == "") throw new ArgumentException("Errore: connectionString vuota (" + connectionStringName + ") ");
                switch (dbType)
                {
                    case "SqlServer":
                        db = new SqlServerDatabase(connectionString);
                        break;
                    case "Sybase":
                        db = new SybaseDatabase(connectionString);
                        break;
                    case "MySql":
                        db = new MySqlDatabase(connectionString);
                        break;
                    case "PostgreSql":
                        db = new PostgreSqlDatabase(connectionString);
                        break;
                    case "SQLite":
                        db = new SQLiteDatabase(connectionString);
                        break;
                    case "Oracle":
                        db = new OracleDatabase(connectionString);
                        break;
                    // Aggiungi altri DBMS qui
                    default:
                        throw new ArgumentException("Tipo di database non supportato");
                }
                if (db == null) throw new ArgumentException("Errore: impossibile creare db (" + connectionStringName + ") ");
                _itemsDB[key] = db;
            }
            return db;
        }

    }
}