

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace ErpToolkit.Helpers.Db
{
    //------------------- 
    //Data Object Gateway
    //-------------------
    // Funzioni di gestione accesso al Database, con il supporto del Data Model 
    public class DogManager
    {
        //formati interni
        internal const string DB_FORMAT_DATE = "yyyy/MM/dd"; //formato stringa di memorizzazione della data nel DB
        internal const string DB_FORMAT_TIME = "HH:mm:ss"; //formato stringa di memorizzazione dell'ora nel DB
        internal const string DB_FORMAT_DATETIME = "yyyy/MM/dd HH:mm:ss"; //formato stringa di memorizzazione di data e ora nel DB
                                                                          //---

        //***************************************************************************************************************************************************
        //*** STRUTTURE STATICHE
        //***************************************************************************************************************************************************

        public class FieldAttr
        {
            public char Readonly { get; set; } = 'N';
            public char Visible { get; set; } = 'Y';
            public void setAttr(string attr)
            {
                char[] a = attr.ToCharArray();
                for (int i = 0; i < a.Length; i++)
                {
                    switch (i)
                    {
                        case 0: Readonly = a[i]; break; // 0) ReadOnly
                        case 1: Visible = a[i]; break; // 1) Visible
                    }
                }
            }
            public string getAttr()
            {
                return (new string(new char[] { Readonly, Visible }));
            }
            public FieldAttr(string attr) { setAttr(attr); }
            public FieldAttr(bool readOnly, bool visible) { if (readOnly) Readonly = 'Y'; if (!visible) Visible = 'N'; }
            public static string strAttr(bool readOnly, bool visible) { return new FieldAttr(readOnly, visible).getAttr(); }
        }


        public static object? getPropertyValue(object selModel, string propName) { return DogManagerInt.getPropertyValue(selModel, propName); }
        public static bool setPropertyValue(object selModel, string propName, string? propValue) { return DogManagerInt.setPropertyValue(selModel, propName, propValue);  }


        //***************************************************************************************************************************************************
        //*** INIZIO CLASSE
        //***************************************************************************************************************************************************


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


        //***************************************************************************************************************************************************
        //*** INIT
        //***************************************************************************************************************************************************

        public readonly Dictionary<string, DogTable> tables = new Dictionary<string, DogTable>();
        public readonly Dictionary<Type, DogTable> tabTypes = new Dictionary<Type, DogTable>();
        public readonly Dictionary<string, DogTable> tabPrefixes = new Dictionary<string, DogTable>();
        public readonly Dictionary<int, DogTable> tabIntcodes = new Dictionary<int, DogTable>();
        public readonly Dictionary<string, DogField> tabProperties = new Dictionary<string, DogField>();
        public readonly Dictionary<string, DogField> tabFields = new Dictionary<string, DogField>();
        //----Tabelle di selezione-------------------------
        public readonly Dictionary<string, DogTable> selfilters = new Dictionary<string, DogTable>();
        public readonly Dictionary<Type, DogTable> selTypes = new Dictionary<Type, DogTable>();
        public readonly Dictionary<string, DogField> selFields = new Dictionary<string, DogField>();


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
            public string MODEL = ""; //Nome Modello es: SIO
            public string CATEG = ""; //Categoria Oggetto es: TAB=Table, SEL=Selection, ecc.
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
            // Ottieni tutti i tipi nell'assembly corrente, il cui namespace inizia con "Models"
            var typesInNamespace = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && t.Namespace != null && t.Namespace.StartsWith(BASE_MODEL));

            foreach (var objType in typesInNamespace)
            {
                // Cerca le costanti MODELNAME e MODELTYPE
                var modName = objType.GetField("MODEL", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                var categName = objType.GetField("CATEG", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                if (modName != null && modName.IsLiteral && !modName.IsInitOnly && categName != null && categName.IsLiteral && !categName.IsInitOnly)
                {
                    string modNameVal = modName.GetValue(null)?.ToString().Trim() ?? ""; // null perché la costante è statica
                    string categNameVal = categName.GetValue(null)?.ToString().Trim() ?? ""; // null perché la costante è statica
                    if (modNameVal == modelName && categNameVal != "")
                    {
                        DogTable tab = new DogTable();
                        tab.tableName = objType.Name;
                        tab.tableTpy = objType;
                        //--
                        tab.Description = objType.GetField("Description")?.GetRawConstantValue()?.ToString() ?? "";
                        tab.SqlTableName = objType.GetField("SqlTableName")?.GetRawConstantValue()?.ToString() ?? "";
                        tab.SqlTableNameExt = objType.GetField("SqlTableNameExt")?.GetRawConstantValue()?.ToString() ?? "";
                        tab.SqlRowIdName = objType.GetField("SqlRowIdName")?.GetRawConstantValue()?.ToString() ?? "";
                        tab.SqlRowIdNameExt = objType.GetField("SqlRowIdNameExt")?.GetRawConstantValue()?.ToString() ?? "";
                        tab.SqlPrefix = objType.GetField("SqlPrefix")?.GetRawConstantValue()?.ToString() ?? "";
                        tab.SqlPrefixExt = objType.GetField("SqlPrefixExt")?.GetRawConstantValue()?.ToString() ?? "";
                        tab.SqlXdataTableName = objType.GetField("SqlXdataTableName")?.GetRawConstantValue()?.ToString() ?? "";
                        tab.SqlXdataTableNameExt = objType.GetField("SqlXdataTableNameExt")?.GetRawConstantValue()?.ToString() ?? "";
                        tab.MODEL = modNameVal;
                        tab.CATEG = categNameVal;
                        tab.INTCODE = Convert.ToInt32(objType.GetField("INTCODE")?.GetRawConstantValue());
                        tab.TBAREA = objType.GetField("TBAREA")?.GetRawConstantValue()?.ToString() ?? "";
                        tab.PREFIX = objType.GetField("PREFIX")?.GetRawConstantValue()?.ToString() ?? "";
                        tab.LIVEDESC = objType.GetField("LIVEDESC")?.GetRawConstantValue()?.ToString() ?? "";
                        tab.IS_RELTABLE = objType.GetField("IS_RELTABLE")?.GetRawConstantValue()?.ToString() ?? "";
                        //---------
                        foreach (var property in objType.GetProperties())
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
                                switch (categNameVal)
                                {
                                    case "TAB":
                                        tabProperties.Add(fld.fieldName, fld);
                                        tabFields.Add(fld.SqlFieldName, fld);
                                        break;
                                    case "SEL":
                                        selFields.Add(fld.SqlFieldName, fld);
                                        break;
                                }
                            }
                        }
                        //-------
                        switch (categNameVal)
                        {
                            case "TAB":
                                tables.Add(tab.SqlTableName, tab);
                                tabTypes.Add(tab.tableTpy, tab);
                                tabPrefixes.Add(tab.SqlPrefix, tab);
                                tabIntcodes.Add(tab.INTCODE, tab);
                                break;
                            case "SEL":
                                selfilters.Add(tab.SqlTableName, tab);
                                selTypes.Add(tab.tableTpy, tab);
                                break;
                        }
                    }
                }
            }

        }
        ~DogManager()
        {
            Dispose();
        }
        public void Dispose()
        {
            // Rilascia risorse non gestite
            if (tables != null) { tables.Clear(); }
            if (tabTypes != null) { tabTypes.Clear(); }
            if (tabPrefixes != null) { tabPrefixes.Clear();  }
            if (tabIntcodes != null) { tabIntcodes.Clear();  }
            if (tabProperties != null) { tabProperties.Clear(); }
            if (tabFields != null) { tabFields.Clear(); }
            if (selfilters != null) { selfilters.Clear(); }
            if (selTypes != null) { selTypes.Clear(); }
            if (selFields != null) { selFields.Clear(); }
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
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return DecodeSpecialTable(_getDbMg().ExecuteQuery(sql, EncodeSpecialFields(parameters, options), maxRecords, transactionId), options);
        }
        public List<T> ExecuteQuery<T>(string sql, IDictionary<string, object> parameters, string options = "", int maxRecords = 10000, string transactionId = null)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
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
            if (fields == null) return null;
            var parameters = new Dictionary<string, object>();
            foreach (var field in fields) { parameters[field.Key] = EncodeSpecialField(field.Value, options); }
            return parameters;
        }
        private DataTable DecodeSpecialTable(DataTable dataTable, string options = "")  //DecodeSpecialFields
        {
            if (dataTable == null) return null;
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
            if (dt == null) return null;
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
                return date.ToString(DB_FORMAT_DATE);
            if (value is TimeOnly time)
                return time.ToString(DB_FORMAT_TIME);
            if (value is DateTime datetime)
                return datetime.ToString(DB_FORMAT_DATETIME);
            // Aggiungere altre conversioni speciali qui se necessario
            return value;
        }
        private object DecodeSpecialField(Type type, string colName, object value, string options = "")
        {
            if (value == null) return null;
            if (value.GetType() == typeof(string))
            {
                string strVal = ((string)value).Trim();  
                if (type == typeof(DateOnly?) || (this.tabFields.ContainsKey(colName) && this.tabFields[colName]?.optDATE == true))
                {
                    if (strVal == "" || strVal == "/  /") return DateOnly.MinValue;
                    if (strVal == "9999/99/99") return DateOnly.MaxValue;
                    if (DateOnly.TryParseExact((string)value, DB_FORMAT_DATE, null, DateTimeStyles.None, out DateOnly date)) return date;
                }
                if (type == typeof(TimeOnly?) || (this.tabFields.ContainsKey(colName) && this.tabFields[colName]?.optTIME == true))
                {
                    if (strVal == "" || strVal == ":  :") return null;
                    if (TimeOnly.TryParseExact(value.ToString(), DB_FORMAT_TIME, null, DateTimeStyles.None, out TimeOnly time)) return time;
                }
                if (type == typeof(DateTime?) || (this.tabFields.ContainsKey(colName) && this.tabFields[colName]?.optDATETIME == true))
                {
                    if (strVal == "" || strVal == "/  /" || strVal == "/  /     :  :") return DateTime.MinValue;
                    if (this.tabFields[colName]?.optDATE == true && DateTime.TryParseExact(value.ToString(), DB_FORMAT_DATE, null, DateTimeStyles.None, out DateTime datetimeDate)) return datetimeDate;
                    else if (this.tabFields[colName]?.optTIME == true && DateTime.TryParseExact(value.ToString(), DB_FORMAT_TIME, null, DateTimeStyles.None, out DateTime datetimeTime)) return datetimeTime;
                    else if (DateTime.TryParseExact(value.ToString(), DB_FORMAT_DATETIME, null, DateTimeStyles.None, out DateTime datetime)) return datetime;
                }
            }
            if (type == typeof(System.Double?)) { return Convert.ToDouble(value); }

            // Aggiungere altre conversioni speciali qui se necessario
            return value;
        }


        //***************************************************************************************************************************************************
        //*** List - Row - Add - Upd
        //***************************************************************************************************************************************************


        //carica list oggetti con il contenuto del DB in base alla struttura in selezione  
        public List<T> List<T>(object selModel)
        {
            if (selModel == null) { throw new ArgumentNullException(nameof(selModel)); }
            T objModel = (T)Activator.CreateInstance(typeof(T)); // create an instance of that type
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder sb = new StringBuilder()
                .Append(DogManagerInt.sqlSelect(this, objModel, ref parameters))
                .Append(DogManagerInt.sqlFrom(this, objModel, ref parameters))
                .Append(DogManagerInt.sqlWhere(this, selModel, ref parameters));
            //access DB
            return this.ExecuteQuery<T>(sb.ToString(), null);
            //$$//
        }
        //carica row con il contenuto del DB in base all'icode'  
        public T Row<T>(string icode)
        {
            if (String.IsNullOrEmpty(icode)) { throw new ArgumentNullException(nameof(icode)); }
            T objModel = (T)Activator.CreateInstance(typeof(T)); // create an instance of that type
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            StringBuilder sb = new StringBuilder()
                .Append(DogManagerInt.sqlSelect(this, objModel, ref parameters))
                .Append(DogManagerInt.sqlFrom(this,objModel, ref parameters))
                .Append(DogManagerInt.sqlWhere(this, objModel, icode, ref parameters));
            //access DB
            DataTable dt = this.ExecuteQuery(sb.ToString(), null);
            if (dt.Rows.Count > 0) { objModel = this.DecodeSpecialRow<T>(dt.Rows[0], ""); }
            return objModel;
        }


    }
}