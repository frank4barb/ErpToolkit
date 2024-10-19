using System.Reflection;
using Microsoft.VisualBasic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System;
using System.Security.Cryptography.Xml;
using System.Globalization;



namespace ErpToolkit.Helpers.Db
{
    public class SQLSERVERHelper
    {
        private string connectionString = "";

        public int MaxRecords = 1001;

        private static NLog.ILogger _logger;

        public SQLSERVERHelper(string connStringName)
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
            //set connection string
            connectionString = ErpContext.Instance.GetString(connStringName); //string connectionString = ErpContext.Instance.GetParam("#connectionString_IRISLive");
            if (connectionString == "") _logger.Error("Errore: connectionString vuota ("+ connStringName + ") ");
        }


        public DataTable execQuery(string sql)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    dt.Load(reader);
                }
                finally
                {
                    // always call Close when done reading.
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Errore " + ex.Message + " in execQuery: " + sql);
                dt = null/* TODO Change to default(_) if this is not a reference type */;
                throw new ErpDbException("DBMS: execQuery: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static string DBCast(DataRow row, string name, string opt = "")
        {
            object obj = row[name];
            if (obj == null)
                return null;
            if (Information.IsDBNull(obj))
                return null;
            if (obj.GetType() == typeof(System.DateTime))
            {
                if (opt == "DATE")
                    return string.Format("{0:yyyy-MM-dd}", obj);
                if (opt == "TIME")
                    return string.Format("{0:HH.mm.ss}", obj);
                if (opt == "DATETIME")
                    string.Format("{0:yyyy-MM-ddTHH.mm.ss}", obj);
                if (opt == "HL7DATE")
                    return string.Format("{0:yyyyMMdd}", obj);
                if (opt == "HL7TIME")
                    return string.Format("{0:HHmmss}", obj);
                if (opt == "HL7DATETIME")
                    return string.Format("{0:yyyyMMddHHmmss}", obj);
                return string.Format("{0:yyyy-MM-ddTHH.mm.ss}", obj);
            }
            if (obj.GetType() == typeof(System.TimeSpan))
            {
                if (opt == "HL7TIME")
                    return string.Format("{0:hhmmss}", obj);
                return string.Format(@"{0:hh\.mm\.ss}", obj); // Return String.Format("{0:HH\:mm\:ss}", obj)
            }
            if (obj.GetType() == typeof(System.String)) return Strings.RTrim((string)obj);
            return null;
        }


        //######################
        public static List<T> ConvertDataTable<T>(DataTable dt, string optCastArray = "")
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItemDataTable<T>(row, optCastArray);
                data.Add(item);
            }
            return data;
        }
        public static T GetItemDataTable<T>(DataRow dr, string optCastArray = "")
        {
            string[] opts = Strings.Split(optCastArray, ",");

            System.Type temp = typeof(T);

            if (temp == typeof(System.Object[])) {
                object[] objArr = new object[dr.Table.Columns.Count];
                //foreach (DataColumn column in dr.Table.Columns)
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    DataColumn column = dr.Table.Columns[i];
                    string optCast = ""; if (i < opts.Length) optCast = opts[i].Trim();
                    objArr[i] = DBCast(dr, column.ColumnName, optCast);
                }
                return (T) (Object) objArr;
            }

            T obj = Activator.CreateInstance<T>();
            //foreach (DataColumn column in dr.Table.Columns)
            for (int i = 0; i< dr.Table.Columns.Count; i++)
            {
                DataColumn column = dr.Table.Columns[i];
                string optCast = ""; if(i < opts.Length) optCast = opts[i].Trim();
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        string fieldOptions = pro?.GetCustomAttribute<ErpDogFieldAttribute>(false)?.SqlFieldOptions?.ToString() ?? "";
                        string strVal = DBCast(dr, column.ColumnName, optCast);

                        //if (dr[column.ColumnName] == null || Information.IsDBNull(dr[column.ColumnName])) pro.SetValue(obj, null, null);
                        if (pro.PropertyType == typeof(System.String)) pro.SetValue(obj, strVal, null);
                        else if (pro.PropertyType == typeof(System.DateTime?))
                        {
                            if (String.IsNullOrEmpty(strVal) || strVal.Trim() == "/  /" || strVal.Trim() == ":  :") pro.SetValue(obj, null, null);
                            else if (fieldOptions.Contains("[DATE]")) pro.SetValue(obj, DateTime.ParseExact(strVal, "yyyy/MM/dd", CultureInfo.InvariantCulture), null);
                            else if (fieldOptions.Contains("[TIME]")) pro.SetValue(obj, DateTime.ParseExact(strVal, "HH:mm:ss", CultureInfo.InvariantCulture), null);
                            else if (fieldOptions.Contains("[DATETIME]")) pro.SetValue(obj, DateTime.ParseExact(strVal, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture), null);
                        }
                        else if (pro.PropertyType == typeof(System.DateOnly?))
                        {
                            if (String.IsNullOrEmpty(strVal) || strVal.Trim() == "/  /" || strVal.Trim() == ":  :") pro.SetValue(obj, null, null);
                            else if (fieldOptions.Contains("[DATE]")) pro.SetValue(obj, DateOnly.ParseExact(strVal, "yyyy/MM/dd", CultureInfo.InvariantCulture), null);
                        }
                        else if (pro.PropertyType == typeof(System.TimeOnly?))
                        {
                            if (String.IsNullOrEmpty(strVal) || strVal.Trim() == "/  /" || strVal.Trim() == ":  :") pro.SetValue(obj, null, null);
                            else if (fieldOptions.Contains("[TIME]")) pro.SetValue(obj, TimeOnly.ParseExact(strVal, "HH:mm:ss", CultureInfo.InvariantCulture), null);
                        }
                        else if (pro.PropertyType == typeof(System.Double?)) { if (dr[column.ColumnName] == null) pro.SetValue(obj, null, null); else pro.SetValue(obj, Convert.ToDouble(dr[column.ColumnName]), null); }
                        else pro.SetValue(obj, dr[column.ColumnName], null); 
                    }
                    else
                        continue;
                }
            }
            return obj;
        }
        //######################


    }
}
