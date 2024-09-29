
using Google.Protobuf.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using static ErpToolkit.Helpers.ErpError;

namespace ErpToolkit.Helpers.Db
{
    //------------------- 
    //Data Object Gateway
    //-------------------
    // Funzioni di gestione accesso al Database, con il supporto del Data Model 
    public class DogManager
    {
        private string _modelName; // = "SIO";
        private string _databaseType; // = "SqlServer";
        private string _connectionStringName; // = "#connectionString_SQLSLocal";
        private NLog.ILogger _logger;

        private DatabaseManager _getDbMg() { return ErpContext.Instance.DbFactory.GetDatabase(_databaseType, _connectionStringName); }

        // Proprietà configurabili
        public string DatabaseType { get { return _getDbMg().DatabaseType; } }
        public int PageSize { get { return _getDbMg().PageSize; } set { _getDbMg().PageSize = value; } }  
        public int MaxRetries { get { return _getDbMg().MaxRetries; } set { _getDbMg().MaxRetries = value; } }
        public int DelayBetweenRetriesMs { get { return _getDbMg().DelayBetweenRetriesMs; } set { _getDbMg().DelayBetweenRetriesMs = value; } }
        public int TimeoutSeconds { get { return _getDbMg().TimeoutSeconds; } set { _getDbMg().TimeoutSeconds = value; } }
        public int TransactionTimeoutSeconds { get { return _getDbMg().TransactionTimeoutSeconds; } set { _getDbMg().TransactionTimeoutSeconds = value; } }
        public int MaxRecords { get { return _getDbMg().MaxRecords; } set { _getDbMg().MaxRecords = value; } }
        public bool EnableTrace { get { return _getDbMg().EnableTrace; } set { _getDbMg().EnableTrace = value; } }
        public int MaxFileLengthBytes { get { return _getDbMg().MaxFileLengthBytes; } set { _getDbMg().MaxFileLengthBytes = value; } }

        //private DogManager()
        //{
        //    //la classe non può essere istanziata
        //}

        //***************************************************************************************************************************************************
        //*** INIT
        //***************************************************************************************************************************************************

        public readonly Dictionary<string, DogTable> tables = new Dictionary<string, DogTable>();
        public readonly Dictionary<string, DogTable> prefixes = new Dictionary<string, DogTable>();
        public readonly Dictionary<int, DogTable> intcodes = new Dictionary<int, DogTable>();
        public readonly Dictionary<string, DogField> fields = new Dictionary<string, DogField>();

        public class DogTable
        {
            public string tableName = "";
            public Type tableTpy;
            public List<DogField> fields = new List<DogField>();
            //--
            public string Description = "";
            public string SqlTableName = "";
            public string SqlTableNameExt = "";
            public string SqlRowIdName = "";
            public string SqlRowIdNameExt = "";
            public string SqlPrefix = "";
            public string SqlPrefixExt = "";
            public string SqlXdataTableName = "";
            public string SqlXdataTableNameExt = "";
            public string DATAMODEL = ""; //Data Model Name of the Class
            public int INTCODE = 0; //Internal Table Code
            public string TBAREA = ""; //Table Area
            public string PREFIX = ""; //Table Prefix
            public string LIVEDESC = ""; //Table type: Live or Description
            public string IS_RELTABLE = ""; //Is Relation Table: Yes or No
        }
        public class DogField
        {
            public string fieldName = "";
            public Type fieldTyp;
            public DogTable table;
            //--
            public string SqlFieldName = "";  // eg: AV_CODICE
            public string SqlFieldProperties = ""; // eg: prop() xref() xdup(ATTIVITA.AV__ICODE[AV__ICODE] {AV_CODICE=' '}) multbxref()
            public string SqlFieldOptions = "";  // [UID] [XID] codice univoco utente e esterno
            public string SqlFieldNameExt = "";  // AY_CODE
            public string Xref = "";  // external reference (if any) eg: Pa1Icode
            //--
            public bool optUID = false;
            public bool optXID = false;
            public bool optDATE = false;
            public bool optTIME = false;
            public bool optDATETIME = false;
            //--
            public string Description;  
            public object? DefaultValue = null;  
            public int? StringLength = null;
        }

        private const string BASE_MODEL = "ErpToolkit.Models";

        internal DogManager(string modelName, string databaseType, string connectionStringName)
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
            //set dog
            _modelName = databaseType;
            _databaseType = databaseType;
            _connectionStringName = connectionStringName;

            //-----------------------
            //Load Default Data Model
            //-----------------------
            Type modelType = Type.GetType(BASE_MODEL + "." + modelName + ".BaseModel");
            object objModel = Activator.CreateInstance(modelType); // create an instance of that type
            List<Type> Tabelle = (List<Type>)modelType.GetProperty("DataObjects").GetValue(objModel);
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
                        DisplayAttribute? displaydAttribute = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                        if (displaydAttribute != null)
                        {
                            fld.Description = displaydAttribute.Description;
                        }
                        DefaultValueAttribute? defaultValueAttribute = property.GetCustomAttribute(typeof(DefaultValueAttribute)) as DefaultValueAttribute;
                        if (defaultValueAttribute != null)
                        {
                            fld.DefaultValue = defaultValueAttribute.Value;
                        }
                        StringLengthAttribute? stringLengthAttribute = property.GetCustomAttribute(typeof(StringLengthAttribute)) as StringLengthAttribute;
                        if (stringLengthAttribute != null)
                        {
                            fld.StringLength = stringLengthAttribute.MaximumLength;
                        }
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
        ~DogManager()
        {
            Dispose();
        }
        public void Dispose()
        {
            // Rilascia risorse non gestite
            if (tables != null) { tables.Clear();  }
            if (prefixes != null) { prefixes.Clear();  }
            if (intcodes != null) { intcodes.Clear();  }
            if (fields != null) { fields.Clear(); }
            GC.SuppressFinalize(this);
        }


        //***************************************************************************************************************************************************
        //*** TRANSAZIONI
        //***************************************************************************************************************************************************

        //public

        public string BeginTransaction(string transactionId, string transactionName = "") { return _getDbMg().BeginTransaction(transactionId, transactionName); }
        public void CommitTransaction(string transactionId, string transactionName = "") { _getDbMg().CommitTransaction(transactionId, transactionName); }
        public void RollbackTransaction(string transactionId, string transactionName = "") { _getDbMg().RollbackTransaction(transactionId, transactionName); }


        //***************************************************************************************************************************************************
        //*** QUERY - MANTAIN
        //***************************************************************************************************************************************************

        //public

        // ExecuteScalar
        public bool RecordExists(string tableName, string keyField, object keyValue, string transactionId = null) 
        { 
            return _getDbMg().RecordExists(tableName, keyField, keyValue, transactionId); 
        }
        public byte[] ReadBlob(string tableName, string keyField, object keyValue, string blobField, int pageNumber, string transactionId = null)
        {
            return _getDbMg().ReadBlob(tableName, keyField, keyValue, blobField, pageNumber, transactionId);
        }
        public void WriteBlob(string tableName, string keyField, object keyValue, string blobField, byte[] data, int pageNumber, string transactionId = null)
        {
            _getDbMg().WriteBlob(tableName, keyField, keyValue, blobField, data, pageNumber, transactionId);
        }

        //ExecuteQuery
        public DataTable ExecuteQuery(string sql, IDictionary<string, object> parameters, string options = "", int maxRecords = 10000, string transactionId = null)
        {
            return DecodeSpecialTable(_getDbMg().ExecuteQuery(sql, EncodeSpecialFields(parameters, options), maxRecords, transactionId), options);
        }
        public List<T> ExecuteQuery<T>(string sql, IDictionary<string, object> parameters, string options = "", int maxRecords = 10000, string transactionId = null)
        {
            return DecodeSpecialTable<T>(_getDbMg().ExecuteQuery(sql, EncodeSpecialFields(parameters, options), maxRecords, transactionId), options);
        }

        //ExecNonQuery
        public void DeleteRecord(string tableName, string keyField, IDictionary<string, object> fields, string transactionId = null)
        {
            _getDbMg().DeleteRecord(tableName, keyField, fields, transactionId);
        }


        //***************************************************************************************************************************************************
        //*** IMPORT-EXPORT CSV
        //***************************************************************************************************************************************************

        //public

        public void ExportTableToCsv(string tableName, string filePath, string whereClause = null, int chunkSize = 10000)
        {
            _getDbMg().ExportTableToCsv(tableName, filePath, whereClause, chunkSize);
        }
        public void ImportCsvToTable(string tableName, string filePath)
        {
            _getDbMg().ImportCsvToTable(tableName, filePath);
        }

        //***************************************************************************************************************************************************
        //*** MANTAIN
        //***************************************************************************************************************************************************


        public void MantainRecord(char action, string tableName, string keyField, string timestampField, string deleteField, IDictionary<string, object> parameters, string options, string transactionId = null)
        {
            _getDbMg().MantainRecord(action, tableName, keyField, timestampField, deleteField, EncodeSpecialFields(parameters, options), options, transactionId);
        }


        //***************************************************************************************************************************************************
        //*** ENCODE-DECODE
        //***************************************************************************************************************************************************

        private Dictionary<string, object> EncodeSpecialFields(IDictionary<string, object> fields, string options="")
        {
            var parameters = new Dictionary<string, object>();
            foreach (var field in fields) { parameters[field.Key] = EncodeSpecialField(field.Value, options); }
            return parameters;
        }
        private DataTable DecodeSpecialTable(DataTable dataTable, string options = "")  //DecodeSpecialFields
        {
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn column in row.Table.Columns)
                {
                    row[column] = DecodeSpecialField(null, column.ColumnName, row[column], options);
                }
            }
            return dataTable;
        }

        public List<T> DecodeSpecialTable<T>(DataTable dt, string options = "")
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = DecodeSpecialRow<T>(row, options);
                data.Add(item);
            }
            return data;
        }
        public T DecodeSpecialRow<T>(DataRow dr, string options = "")
        {
            Type temp = typeof(T);
            //decode in object array
            if (temp == typeof(System.Object[]))
            {
                object[] objArr = new object[dr.Table.Columns.Count];
                //foreach (DataColumn column in dr.Table.Columns)
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    DataColumn column = dr.Table.Columns[i];
                    objArr[i] = DecodeSpecialField(null, column.ColumnName, dr[column.ColumnName], options);
                }
                return (T)(Object)objArr;
            }
            //decode in object model
            T obj = Activator.CreateInstance<T>();
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                DataColumn column = dr.Table.Columns[i];
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        pro.SetValue(obj, DecodeSpecialField(pro.PropertyType, column.ColumnName, dr[column.ColumnName], options), null);
                    }
                }
            }
            return obj;
        }











        //-----------------------------------
        private object EncodeSpecialField(object value, string options = "")
        {
            if (value is DateOnly date)
                return date.ToString("yyyy/MM/dd");
            if (value is TimeOnly time)
                return time.ToString("HH:mm:ss");
            if (value is DateTime datetime)
                return datetime.ToString("yyyy/MM/dd HH:mm:ss");
            // Aggiungere altre conversioni speciali qui se necessario
            return value;
        }
        private object DecodeSpecialField(Type type, string colName, object value, string options = "")
        {
            if (value == null) return null;
            if (value.GetType() == typeof(string))
            {
                string strVal = ((string)value).Trim();  
                if (type == typeof(DateOnly?) || this.fields[colName]?.optDATE == true)
                {
                    if (strVal == "" || strVal == "/  /") return DateOnly.MinValue;
                    if (strVal == "9999/99/99") return DateOnly.MaxValue;
                    if (DateOnly.TryParseExact((string)value, "yyyy/MM/dd", null, DateTimeStyles.None, out DateOnly date)) return date;
                }
                if (type == typeof(TimeOnly?) || this.fields[colName]?.optTIME == true)
                {
                    if (strVal == "" || strVal == ":  :") return null;
                    if (TimeOnly.TryParseExact(value.ToString(), "HH:mm:ss", null, DateTimeStyles.None, out TimeOnly time)) return time;
                }
                if (type == typeof(DateTime?) || this.fields[colName]?.optDATETIME == true)
                {
                    if (strVal == "" || strVal == "/  /" || strVal == "/  /     :  :") return DateTime.MinValue;
                    if (this.fields[colName]?.optDATE == true && DateTime.TryParseExact(value.ToString(), "yyyy/MM/dd", null, DateTimeStyles.None, out DateTime datetimeDate)) return datetimeDate;
                    else if (this.fields[colName]?.optTIME == true && DateTime.TryParseExact(value.ToString(), "HH:mm:ss", null, DateTimeStyles.None, out DateTime datetimeTime)) return datetimeTime;
                    else if (DateTime.TryParseExact(value.ToString(), "yyyy/MM/dd HH:mm:ss", null, DateTimeStyles.None, out DateTime datetime)) return datetime;
                }
            }
            if (type == typeof(System.Double?)) { return Convert.ToDouble(value); }

            // Aggiungere altre conversioni speciali qui se necessario
            return value;
        }




    }
}