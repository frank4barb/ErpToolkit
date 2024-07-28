using Microsoft.VisualBasic;
using NLog;
using System.Data;



using InterSystems.Data.IRISClient;
using Quartz;
using System.Reflection;



namespace ErpToolkit.Helpers.Db
{
    public class IRISHelper
    {

        public enum ErrType : byte
        {
            Success,
            Internal,
            Timeout
        }

        public int MaxRecords = 1001;
        private string connectionString = "";
        private static NLog.ILogger _logger;

        public IRISHelper(string connStringName)
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
            //set connection string
            connectionString = ErpContext.Instance.GetString(connStringName); //string connectionString = ErpContext.Instance.GetParam("#connectionString_IRISLive");
            if (connectionString == "") _logger.Error("Errore: connectionString vuota (" + connStringName + ") ");
            connectionString = UtilHelper.DecryptData(connectionString);
        }

        public DataTable mntIRIS_REF(string sql, ref string action)
        {
            DataTable dt = new DataTable();
            IRISConnection connection = new IRISConnection(connectionString);

            IRISDataAdapter adapter = new IRISDataAdapter();

            try
            {
                connection.Open();
                switch (Strings.UCase(action))
                {
                    case "A":
                        {
                            adapter.InsertCommand = new IRISCommand(sql, connection);
                            adapter.InsertCommand.ExecuteNonQuery();
                            break;
                        }

                    case "U":
                        {
                            adapter.UpdateCommand = new IRISCommand(sql, connection);
                            adapter.UpdateCommand.ExecuteNonQuery();
                            break;
                        }

                    case "D":
                        {
                            IRISCommand command = new IRISCommand(sql, connection);
                            // Dim parameter As IRISParameter = command.Parameters.Add("@ID", IRISDbType.BigInt, 20, "ID")
                            adapter.DeleteCommand = command;
                            adapter.DeleteCommand.ExecuteNonQuery();

                            IRISCommandBuilder builder = new IRISCommandBuilder();
                            break;
                        }

                    default:
                        {
                            _logger.Error("Errore " + "action not correct" + " in execQuery: " + sql);
                            return null/* TODO Change to default(_) if this is not a reference type */;
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Errore " + ex.Message + " in execQuery: " + sql);
            }
            finally
            {
                // always close connection.
                adapter.Dispose();
                connection.Close();
            }

            return dt;
        }

        public DataTable execQuery(string sql, ref string errMsg, ref ErrType errTpy)
        {
            errMsg = "";  errTpy = ErrType.Success;
            DataTable dt = new DataTable();
            IRISConnection connection = new IRISConnection(connectionString);
            try
            {
                string traceALL = ErpContext.Instance.GetString("#traceDbALL");
                if (traceALL == "1")
                    _logger.Info("Info: in execQuery: " + sql);

                // Dim command As New IRISCommand(sql, connection)
                // connection.Open()
                // Dim reader As IRISDataReader = command.ExecuteReader()
                // Try
                // dt.Load(reader)
                // Finally
                // ' always call Close when done reading.
                // reader.Close()
                // End Try
                IRISDataAdapter adapter = new IRISDataAdapter(sql, connection);
                adapter.SelectCommand.CommandTimeout = 60; // Default Is 60 seconds
                adapter.TableMappings.Add("Table", "QUERY");
                IRISCommandBuilder icb = new IRISCommandBuilder(adapter);
                DataSet ds = new DataSet();
                adapter.Fill(ds, 0, MaxRecords, "QUERY");
                dt = ds.Tables["QUERY"];
            }
            catch (Exception ex)
            {
                _logger.Error("execQueryIRIS_Live: Catch Exception");
                // --gestione tipo errore
                errTpy = ErrType.Internal;
                try
                {
                    var irisEx = (InterSystems.Data.IRISClient.IRISException)ex.GetBaseException();
                    if (irisEx == null == false && irisEx.NativeError == 450)
                        errTpy = ErrType.Timeout;
                }
                catch (Exception xx)
                {
                }
                // -----------------------
                errMsg = ex.Message;
                DataRow[] err = dt.GetErrors();
                System.Text.StringBuilder msg = new System.Text.StringBuilder();
                foreach (var e in err)
                    msg.Append(" - " + e.RowError);
                errMsg += msg.ToString();
                _logger.Error("Errore " + errMsg + " in execQuery: " + sql);
                dt = null/* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {
                // always close connection.
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
            return Strings.Trim((string?)obj);
        }


        //######################
        public static List<T> ConvertDataTable<T>(DataTable dt, string optCastArray = "")
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row, optCastArray);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr, string optCastArray = "")
        {
            string[] opts = Strings.Split(optCastArray, ",");

            Type temp = typeof(T);

            if (temp == typeof(System.Object[]))
            {
                object[] objArr = new object[dr.Table.Columns.Count];
                //foreach (DataColumn column in dr.Table.Columns)
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    DataColumn column = dr.Table.Columns[i];
                    string optCast = ""; if (i < opts.Length) optCast = opts[i].Trim();
                    objArr[i] = DBCast(dr, column.ColumnName, optCast);
                }
                return (T)(Object)objArr;
            }

            T obj = Activator.CreateInstance<T>();
            //foreach (DataColumn column in dr.Table.Columns)
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                DataColumn column = dr.Table.Columns[i];
                string optCast = ""; if (i < opts.Length) optCast = opts[i].Trim();
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (pro.PropertyType == typeof(System.String)) pro.SetValue(obj, DBCast(dr, column.ColumnName, optCast), null);
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
