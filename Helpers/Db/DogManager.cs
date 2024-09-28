
using Amazon.Runtime.Internal.Transform;
using Google.Protobuf.Reflection;
using Npgsql;
using System.Data;
using System.Reflection;
using System.Text;
using static ErpToolkit.Helpers.ErpError;

namespace ErpToolkit.Helpers.Db
{
    // Funzioni di gestione accesso al Database, con il supporto del Data Model 
    public static class DogManager
    {
        private static string _dbType; // = "SqlServer";
        private static string _connectionStringName; // = "#connectionString_SQLSLocal";
        private static NLog.ILogger _logger;

        private static DatabaseManager _getDbMg() { return ErpContext.Instance.DbFactory.GetDatabase(_dbType, _connectionStringName); }

        // Proprietà configurabili
        public static string DatabaseType { get { return _getDbMg().DatabaseType; } }
        public static int PageSize { get { return _getDbMg().PageSize; } set { _getDbMg().PageSize = value; } }  
        public static int MaxRetries { get { return _getDbMg().MaxRetries; } set { _getDbMg().MaxRetries = value; } }
        public static int DelayBetweenRetriesMs { get { return _getDbMg().DelayBetweenRetriesMs; } set { _getDbMg().DelayBetweenRetriesMs = value; } }
        public static int TimeoutSeconds { get { return _getDbMg().TimeoutSeconds; } set { _getDbMg().TimeoutSeconds = value; } }
        public static int TransactionTimeoutSeconds { get { return _getDbMg().TransactionTimeoutSeconds; } set { _getDbMg().TransactionTimeoutSeconds = value; } }
        public static int MaxRecords { get { return _getDbMg().MaxRecords; } set { _getDbMg().MaxRecords = value; } }
        public static bool EnableTrace { get { return _getDbMg().EnableTrace; } set { _getDbMg().EnableTrace = value; } }
        public static int MaxFileLengthBytes { get { return _getDbMg().MaxFileLengthBytes; } set { _getDbMg().MaxFileLengthBytes = value; } }

        //private DogManager()
        //{
        //    //la classe non può essere istanziata
        //}

        //***************************************************************************************************************************************************
        //*** INIT
        //***************************************************************************************************************************************************


        public const string BASE_MODEL = "ErpToolkit.Models";

        public class DogTable
        {
            public string tableName;
            public Type tableTpy;
            public List<DogField> fields = new List<DogField>();
            //--
            public string Description ;
            public string SqlTableName ;
            public string SqlTableNameExt;
            public string SqlRowIdName;
            public string SqlRowIdNameExt;
            public string SqlPrefix;
            public string SqlPrefixExt;
            public string SqlXdataTableName;
            public string SqlXdataTableNameExt;
            public string DATAMODEL; //Data Model Name of the Class
            public int INTCODE; //Internal Table Code
            public string TBAREA; //Table Area
            public string PREFIX; //Table Prefix
            public string LIVEDESC; //Table type: Live or Description
            public string IS_RELTABLE; //Is Relation Table: Yes or No
        }
        public class DogField
        {
            public string fieldName;
            public Type fieldTyp;
            public DogTable table;
            //--
            public string SqlFieldName;  // eg: AV_CODICE
            public string SqlFieldProperties; // eg: prop() xref() xdup(ATTIVITA.AV__ICODE[AV__ICODE] {AV_CODICE=' '}) multbxref()
            public string SqlFieldOptions;  // [UID] [XID] codice univoco utente e esterno
            public string SqlFieldNameExt;  // AY_CODE
            public string Xref;  // external reference (if any) eg: Pa1Icode
            //--
            public bool optUID = false;
            public bool optXID = false;
            public bool optDATE = false;
            public bool optTIME = false;
            public bool optDATETIME = false;
        }

        public static Dictionary<string, DogTable> tables = new Dictionary<string, DogTable>();
        public static Dictionary<string, DogTable> prefixes = new Dictionary<string, DogTable>();
        public static Dictionary<int, DogTable> intcodes = new Dictionary<int, DogTable>();
        public static Dictionary<string, DogField> fields = new Dictionary<string, DogField>();


        public static void Init(string initFileName, string dbType, string connectionStringName)
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();

            _dbType = dbType; _connectionStringName = connectionStringName;

            string modelName = "SIO";

            //-----------------------
            //Load Default Data Model
            //-----------------------
            Type modelType = Type.GetType(BASE_MODEL + "." + modelName + ".BaseModel");
            object objModel = Activator.CreateInstance(modelType); // create an instance of that type
            List<Type> Tabelle = (List<Type>)modelType.GetProperty("Tabelle").GetValue(objModel);
            foreach (var tabellaType in Tabelle)
            {
                DogTable tab = new DogTable();
                tab.tableName = tabellaType.Name;
                tab.tableTpy = tabellaType;
                //--
                tab.Description = tabellaType.GetField("Description")?.GetRawConstantValue()?.ToString() ?? "";
                tab.SqlTableName = tabellaType.GetField("SqlTableName")?.GetRawConstantValue()?.ToString() ?? "";
                tab.SqlTableNameExt = tabellaType.GetField("SqlTableNameExt")?.GetRawConstantValue()?.ToString() ?? "";
                tab.SqlRowIdName = tabellaType.GetField("SqlRowIdName")?.GetRawConstantValue()?.ToString() ?? "";
                tab.SqlRowIdNameExt = tabellaType.GetField("SqlRowIdNameExt")?.GetRawConstantValue()?.ToString() ?? "";
                tab.SqlPrefix = tabellaType.GetField("SqlPrefix")?.GetRawConstantValue()?.ToString() ?? "";
                tab.SqlPrefixExt = tabellaType.GetField("SqlPrefixExt")?.GetRawConstantValue()?.ToString() ?? "";
                tab.SqlXdataTableName = tabellaType.GetField("SqlXdataTableName")?.GetRawConstantValue()?.ToString() ?? "";
                tab.SqlXdataTableNameExt = tabellaType.GetField("SqlXdataTableNameExt")?.GetRawConstantValue()?.ToString() ?? "";
                tab.DATAMODEL = tabellaType.GetField("DATAMODEL")?.GetRawConstantValue()?.ToString() ?? ""; 
                tab.INTCODE = Convert.ToInt32(tabellaType.GetField("INTCODE")?.GetRawConstantValue()); 
                tab.TBAREA = tabellaType.GetField("TBAREA")?.GetRawConstantValue()?.ToString() ?? ""; 
                tab.PREFIX = tabellaType.GetField("PREFIX")?.GetRawConstantValue()?.ToString() ?? ""; 
                tab.LIVEDESC = tabellaType.GetField("LIVEDESC")?.GetRawConstantValue()?.ToString() ?? ""; 
                tab.IS_RELTABLE = tabellaType.GetField("IS_RELTABLE")?.GetRawConstantValue()?.ToString() ?? ""; 
                //---------
                foreach (var property in tabellaType.GetProperties())
                {
                    ErpDogFieldAttribute? erpDogFieldAttribute = property.GetCustomAttribute(typeof(ErpDogFieldAttribute)) as ErpDogFieldAttribute;
                    if (erpDogFieldAttribute != null)
                    {
                        //---------
                        DogField fld = new DogField();
                        fld.fieldName = property.Name;
                        fld.fieldTyp = property.PropertyType;
                        fld.table = tab;
                        //--
                        fld.SqlFieldName = erpDogFieldAttribute.SqlFieldName?.ToString() ?? "";
                        fld.SqlFieldProperties = erpDogFieldAttribute.SqlFieldProperties?.ToString() ?? "";
                        fld.SqlFieldOptions = erpDogFieldAttribute.SqlFieldOptions?.ToString() ?? "";
                        fld.SqlFieldNameExt = erpDogFieldAttribute.SqlFieldNameExt?.ToString() ?? "";
                        fld.Xref = erpDogFieldAttribute.Xref?.ToString() ?? "";
                        //---------
                        fld.optUID = fld.SqlFieldOptions.Contains("[UID]");
                        fld.optXID = fld.SqlFieldOptions.Contains("[XID]");
                        fld.optDATE = fld.SqlFieldOptions.Contains("[DATE]");
                        fld.optTIME = fld.SqlFieldOptions.Contains("[TIME]");
                        fld.optDATETIME = fld.SqlFieldOptions.Contains("[DATETIME]");
                        //---------
                        tab.fields.Add(fld);
                        fields.Add(fld.SqlFieldName, fld);
                    }
                }
                //-------
                tables.Add(tab.SqlTableName, tab);
                prefixes.Add(tab.SqlPrefix, tab);
                intcodes.Add(tab.INTCODE, tab);
            }
        }





        //***************************************************************************************************************************************************
        //*** TRANSAZIONI
        //***************************************************************************************************************************************************

        //public

        public static string BeginTransaction(string transactionId, string transactionName = "") { return _getDbMg().BeginTransaction(transactionId, transactionName); }
        public static void CommitTransaction(string transactionId, string transactionName = "") { _getDbMg().CommitTransaction(transactionId, transactionName); }
        public static void RollbackTransaction(string transactionId, string transactionName = "") { _getDbMg().RollbackTransaction(transactionId, transactionName); }


        //***************************************************************************************************************************************************
        //*** QUERY - MANTAIN
        //***************************************************************************************************************************************************

        //public

        // ExecuteScalar
        public static bool RecordExists(string tableName, string keyField, object keyValue, string transactionId = null) 
        { 
            return _getDbMg().RecordExists(tableName, keyField, keyValue, transactionId); 
        }
        public static byte[] ReadBlob(string tableName, string keyField, object keyValue, string blobField, int pageNumber, string transactionId = null)
        {
            return _getDbMg().ReadBlob(tableName, keyField, keyValue, blobField, pageNumber, transactionId);
        }
        public static void WriteBlob(string tableName, string keyField, object keyValue, string blobField, byte[] data, int pageNumber, string transactionId = null)
        {
            _getDbMg().WriteBlob(tableName, keyField, keyValue, blobField, data, pageNumber, transactionId);
        }

        //ExecuteQuery
        public static DataTable ExecuteQuery(string sql, IDictionary<string, object> parameters, int maxRecords = 10000, string transactionId = null)
        {
            return _getDbMg().ExecuteQuery(sql, parameters, maxRecords, transactionId);
        }

        //ExecNonQuery
        public static void DeleteRecord(string tableName, string keyField, IDictionary<string, object> fields, string transactionId = null)
        {
            _getDbMg().DeleteRecord(tableName, keyField, fields, transactionId);
        }


        //***************************************************************************************************************************************************
        //*** IMPORT-EXPORT CSV
        //***************************************************************************************************************************************************

        //public

        public static void ExportTableToCsv(string tableName, string filePath, string whereClause = null, int chunkSize = 10000)
        {
            _getDbMg().ExportTableToCsv(tableName, filePath, whereClause, chunkSize);
        }
        public static void ImportCsvToTable(string tableName, string filePath)
        {
            _getDbMg().ImportCsvToTable(tableName, filePath);
        }

        //***************************************************************************************************************************************************
        //*** MANTAIN
        //***************************************************************************************************************************************************


        public static void MantainRecord(char action, string tableName, string keyField, string timestampField, string deleteField, IDictionary<string, object> fields, string options, string transactionId = null)
        {
            _getDbMg().MantainRecord(action, tableName, keyField, timestampField, deleteField, fields, options, transactionId);
        }









    }
}