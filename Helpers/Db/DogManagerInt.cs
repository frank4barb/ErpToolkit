using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;
using System.Collections;
using static ErpToolkit.Helpers.Db.DogManager;
using static ErpToolkit.Helpers.Db.DatabaseFactory;
using System.Linq;


namespace ErpToolkit.Helpers.Db
{
    public static class DogManagerInt
    {
        private const bool IS_NULLABLE_ID = false; //=true se posso inserire NULL in tutti gli identifificatori univoci (serve per velocizzare gli indici)
        private const bool IS_NULLABLE_NUM = false; //=true se posso inserire NULL sui valori numerici
        private const bool IS_NULLABLE_INDEX = false; //=true se posso definire indici univoci con campi NULL

        //configura NLog per la classe
        public static NLog.Config.LoggingConfiguration GetNLogConfig()
        {
            var config = new NLog.Config.LoggingConfiguration();
            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "backupclientlogfile_backupservice.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            // Rules for mapping loggers to targets            
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);
            return config;
        }

        //******************************************************************************************************************

        //crea SELECT per l'oggetto del modello 'objModel'
        internal static string sqlSelect(DogManager dogMng, object objModel, ref IDictionary<string, object> parameters)
        {
            if (objModel == null) { throw new ArgumentNullException(nameof(objModel)); }
            StringBuilder sb = new StringBuilder("select ");
            //ciclo sulle proprietà
            System.Type type = objModel.GetType(); 
            foreach (var property in type.GetProperties())
            {
                try
                {
                    string propertyName = property.Name; // Get property name and value
                    var sqlFieldNameExt = dogMng.tabProperties[propertyName]?.SqlFieldNameExt?.Trim() ?? "";
                    if (sqlFieldNameExt != "") { sb.AppendLine($" {sqlFieldNameExt} as {propertyName},"); }
                }
                catch (Exception ex) { }  //skip exceptions
            }
            // terminatore di select
            sb.AppendLine($" 0 as ErpTerm ");
            return sb.ToString();
        }
        //crea FROM per l'oggetto del modello 'objModel'
        internal static string sqlFrom(DogManager dogMng, object objModel, ref IDictionary<string, object> parameters)
        {
            if (objModel == null) { throw new ArgumentNullException(nameof(objModel)); }
            System.Type type = objModel.GetType();
            var sqlTableNameExt = dogMng.tabTypes[type]?.SqlTableNameExt?.Trim() ?? "";
            return $"from {sqlTableNameExt} \n";
        }
        //crea WHERE per l'oggetto del modello 'objModel' in base all'oggetto di selezione 'selModel'
        internal static string sqlWhere(DogManager dogMng, object selModel, ref IDictionary<string, object> parameters)
        {
            // init
            int numCond = 0, numPreCond = 0;
            if (selModel == null) { throw new ArgumentNullException(nameof(selModel)); }
            StringBuilder sb = new StringBuilder("where ");
            //ciclo sulle proprietà
            System.Type type = selModel.GetType();
            foreach (var property in type.GetProperties())
            {
                try
                {
                    string propertyName = property.Name; // Get property name and value
                    var sqlFieldNameExt = dogMng.selProperties[propertyName]?.SqlFieldNameExt?.Trim() ?? ""; //per applicare la condizione la proprietà deve avere un attributo [ErpDogField(..)]
                    object propertyValue = property.GetValue(selModel);
                    if (propertyValue == null) continue;
                    // >>> verifica List
                    if (typeof(IEnumerable<object>).IsAssignableFrom(propertyValue.GetType()))
                    {
                        IEnumerable<object> ienum = (IEnumerable<object>)propertyValue;
                        List<object> list = ienum.Where(item => item != null && !(item is string str && string.IsNullOrWhiteSpace(str))).ToList();  // elimina elementi null e strighe vuote
                        if (list.Count() == 0) continue;
                        if (list[0] is string) propertyValue = (List<string>)list.Select(i => i.ToString()).ToList();
                        else if (list[0] is sbyte || list[0] is byte || list[0] is short || list[0] is ushort || list[0] is int || list[0] is uint
                             || list[0] is long || list[0] is ulong) propertyValue = (List<long>)list.Select(i => Convert.ToInt64(i)).ToList();
                        else throw new ErpException($"{propertyName}: Tipo Lista non supportato (solo stinga e intero)");
                    }
                    // >>> verifica DateRange
                    if (propertyValue is DateRange dateRange)
                    {
                        if (dateRange.StartDate == default && dateRange.EndDate == default) continue; //entrambe le date sono null
                    }
                    //---
                    numPreCond++; //condizione prevista
                    //---
                    // esiste una condizione
                    if (sqlFieldNameExt != "")
                    {
                        try
                        {
                            //string fieldOptions = ((ErpDogFieldAttribute)attribute).SqlFieldOptions?.ToString() ?? "";
                            DogManager.DogField fld = dogMng.selProperties[propertyName];
                            if (propertyValue is string str)
                            {
                                if (fld.optUID) sb.AppendLine($" {sqlFieldNameExt} = {DogManager.addParam(str.TrimEnd(), ref parameters)} and ");   //sb.AppendLine($" {sqlFieldNameExt} = '{str.TrimEnd()}' and ");
                                else if (fld.optXID) sb.AppendLine($" {sqlFieldNameExt} = {DogManager.addParam(str.TrimEnd(), ref parameters)} and "); //sb.AppendLine($" {sqlFieldNameExt} = '{str.TrimEnd()}' and ");
                                else sb.AppendLine($" {sqlFieldNameExt} LIKE {DogManager.addParam($"%{str.TrimEnd()}%", ref parameters)} and "); //sb.AppendLine($" {sqlFieldNameExt} LIKE '%{str}%' and ");
                            }
                            else if (propertyValue is DateTime dattim)  // DateOnly.FromDateTime()
                            {
                                if (fld.optDATE) sb.AppendLine($" {sqlFieldNameExt} = {DogManager.addParam(DateOnly.FromDateTime(dattim), ref parameters)} and ");  //sb.AppendLine($" {sqlFieldNameExt} = '{dattim.ToString(DogManager.DB_FORMAT_DATE)}' and ");
                                else if (fld.optTIME) sb.AppendLine($" {sqlFieldNameExt} = {DogManager.addParam(TimeOnly.FromDateTime(dattim), ref parameters)} and ");  //sb.AppendLine($" {sqlFieldNameExt} = '{dattim.ToString(DogManager.DB_FORMAT_TIME)}' and ");
                                else if (fld.optDATETIME) sb.AppendLine($" {sqlFieldNameExt} = {DogManager.addParam(dattim, ref parameters)} and ");  //sb.AppendLine($" {sqlFieldNameExt} = '{dattim.ToString(DogManager.DB_FORMAT_DATETIME)}' and ");
                                else throw new ErpException($"{propertyName}: DateTime fa riferimento ad un campo non data ora");
                            }
                            else if (propertyValue is DateOnly dat)
                            {
                                if (fld.optDATE) sb.AppendLine($" {sqlFieldNameExt} = {DogManager.addParam(dat, ref parameters)} and ");  //sb.AppendLine($" {sqlFieldNameExt} = '{dat.ToString(DogManager.DB_FORMAT_DATE)}' and ");
                            }
                            else if (propertyValue is TimeOnly tim)
                            {
                                if (fld.optTIME) sb.AppendLine($" {sqlFieldNameExt} = {DogManager.addParam(tim, ref parameters)} and ");  //sb.AppendLine($" {sqlFieldNameExt} = '{tim.ToString(DogManager.DB_FORMAT_TIME)}' and ");
                            }
                            else if (propertyValue is List<string> strList) sb.Append($" {sqlFieldNameExt} in (").Append(string.Join(", ", DogManager.addListParam(strList.Select(str => str.TrimEnd()).ToList<object>(), ref parameters))).AppendLine($") and");  //sb.Append($" {sqlFieldNameExt} in (").Append(string.Join(", ", strList.Select(str => $"'{str.Trim()}'"))).AppendLine($") and");
                            else if (propertyValue is List<long> lngList) sb.Append($" {sqlFieldNameExt} in (").Append(string.Join(", ", DogManager.addListParam(lngList.Select(u => (object)u).ToList(), ref parameters))).AppendLine($") and");  //sb.Append($" {sqlFieldNameExt} in (").Append(string.Join(", ", lngList)).AppendLine($") and");
                            else if (propertyValue is DateRange dateRng)
                            {
                                if (dateRng.StartDate == default)
                                {
                                    if (fld.optDATE) sb.AppendLine($" {sqlFieldNameExt} <= '{DogManager.addParam(DateOnly.FromDateTime(dateRng.EndDate), ref parameters)}' and");  //sb.AppendLine($" {sqlFieldNameExt} <= '{dateRng.EndDate.ToString(DogManager.DB_FORMAT_DATE)}' and");
                                    else if (fld.optTIME) sb.AppendLine($" {sqlFieldNameExt} <= '{DogManager.addParam(TimeOnly.FromDateTime(dateRng.EndDate), ref parameters)}' and");  //sb.AppendLine($" {sqlFieldNameExt} <= '{dateRng.EndDate.ToString(DogManager.DB_FORMAT_DATE)}' and");
                                    else if (fld.optDATETIME) sb.AppendLine($" {sqlFieldNameExt} <= '{DogManager.addParam(dateRng.EndDate, ref parameters)}' and");  //sb.AppendLine($" {sqlFieldNameExt} <= '{dateRng.EndDate.ToString(DogManager.DB_FORMAT_DATE)}' and");
                                    else throw new ErpException($"{propertyName}: DateRange fa riferimento ad un campo non data ora (1)");
                                }
                                else if (dateRng.EndDate == default)
                                {
                                    if (fld.optDATE) sb.AppendLine($" {sqlFieldNameExt} >= '{DogManager.addParam(DateOnly.FromDateTime(dateRng.StartDate), ref parameters)}' and");  //sb.AppendLine($" {sqlFieldNameExt} >= '{dateRng.StartDate.ToString(DogManager.DB_FORMAT_DATE)}' and");
                                    else if (fld.optTIME) sb.AppendLine($" {sqlFieldNameExt} >= '{DogManager.addParam(TimeOnly.FromDateTime(dateRng.StartDate), ref parameters)}' and");  //sb.AppendLine($" {sqlFieldNameExt} >= '{dateRng.StartDate.ToString(DogManager.DB_FORMAT_DATE)}' and");
                                    else if (fld.optDATETIME) sb.AppendLine($" {sqlFieldNameExt} >= '{DogManager.addParam(dateRng.StartDate, ref parameters)}' and");  //sb.AppendLine($" {sqlFieldNameExt} >= '{dateRng.StartDate.ToString(DogManager.DB_FORMAT_DATE)}' and");
                                    else throw new ErpException($"{propertyName}: DateRange fa riferimento ad un campo non data ora (2)");
                                }
                                else
                                {
                                    if (fld.optDATE) sb.AppendLine($" ({sqlFieldNameExt} BETWEEN '{DogManager.addParam(DateOnly.FromDateTime(dateRng.StartDate), ref parameters)}' and '{DogManager.addParam(DateOnly.FromDateTime(dateRng.EndDate), ref parameters)}') and");  //sb.AppendLine($" ({sqlFieldNameExt} BETWEEN '{dateRng.StartDate.ToString(DogManager.DB_FORMAT_DATE)}' and '{dateRng.EndDate.ToString(DogManager.DB_FORMAT_DATE)}') and");
                                    else if (fld.optTIME) sb.AppendLine($" ({sqlFieldNameExt} BETWEEN '{DogManager.addParam(TimeOnly.FromDateTime(dateRng.StartDate), ref parameters)}' and '{DogManager.addParam(TimeOnly.FromDateTime(dateRng.EndDate), ref parameters)}') and");  //sb.AppendLine($" ({sqlFieldNameExt} BETWEEN '{dateRng.StartDate.ToString(DogManager.DB_FORMAT_DATE)}' and '{dateRng.EndDate.ToString(DogManager.DB_FORMAT_DATE)}') and");
                                    else if (fld.optDATETIME) sb.AppendLine($" ({sqlFieldNameExt} BETWEEN '{DogManager.addParam(dateRng.StartDate, ref parameters)}' and '{DogManager.addParam(dateRng.EndDate, ref parameters)}') and");  //sb.AppendLine($" ({sqlFieldNameExt} BETWEEN '{dateRng.StartDate.ToString(DogManager.DB_FORMAT_DATE)}' and '{dateRng.EndDate.ToString(DogManager.DB_FORMAT_DATE)}') and");
                                    else throw new ErpException($"{propertyName}: DateRange fa riferimento ad un campo non data ora (3)");
                                }
                            }
                            else continue;
                            numCond++; //condizione applicata correttamente
                        }
                        catch (Exception ex) { }  //skip exceptions
                    }
                }
                catch (Exception ex) { }  //skip exceptions
            }
            // Verifica condizioni
            if (numCond == 0) throw new ErpException("Nessuna condizione inserita");
            if (numCond != numPreCond) throw new ErpException("Errore nell'applicazione delle condizioni di filtro. Previste {" + numPreCond.ToString() + "}, applicate " + numCond.ToString() + "");
            // terminatore di where
            var sqlPrefixExt = dogMng.selTypes[type]?.SqlPrefixExt?.Trim() ?? "";
            sb.AppendLine($" {sqlPrefixExt}_DELETED = {DogManager.addParam("N", ref parameters)} ");
            return sb.ToString();
        }


        //crea WHERE per l'oggetto del modello 'objModel' in base all'icode
        internal static string sqlWhere(DogManager dogMng, object objModel, string icode, ref IDictionary<string, object> parameters)
        {
            if (objModel == null) { throw new ArgumentNullException(nameof(objModel)); }
            System.Type type = objModel.GetType();
            var sqlRowIdNameExt = dogMng.tabTypes[type]?.SqlRowIdNameExt?.Trim() ?? "";
            return $"where {sqlRowIdNameExt} = {DogManager.addParam(icode, ref parameters)} ";
        }

        //crea INSERT, UPDATE, DELETE(logico) per l'oggetto del modello 'tabModel' (SOLO PER TABELLE)
        internal static string sqlMantain(DogManager dogMng, object tabModel, ref IDictionary<string, object> parameters, ref List<DogResult> results, string options = "")
        {
            // init
            int numParam = 0;
            if (tabModel == null) { throw new ArgumentNullException(nameof(tabModel)); }
            StringBuilder sb = new StringBuilder(), sbValues = new StringBuilder();
            //ciclo sulle proprietà
            System.Type type = tabModel.GetType();
            DogManager.DogTable tab = dogMng.tabTypes[type];
            if (tab == null) throw new ArgumentNullException(nameof(tabModel));
            //campi di sistema
            var sqlPrefixExt = dogMng.tabTypes[type]?.SqlPrefixExt?.Trim() ?? "";
            if (sqlPrefixExt == "") throw new ArgumentNullException(nameof(sqlPrefixExt));
            List<string> sysFieldList = new List<string>() { "_ICODE","_DELETED","_TIMESTAMP", "_CDATE","_CTIME","_CAGENT","_CUNIT", "_MDATE","_MTIME","_MAGENT","_MUNIT" };

            string icode = ""; byte[] oldTimestamp = new byte[8], newTimestamp = DatabaseManager.GenerateTimestamp();
            string dateNow = DateTime.Now.ToString(DogManager.DB_FORMAT_DATE), timeNow = DateTime.Now.ToString(DogManager.DB_FORMAT_TIME);
            string agent = ErpContext.Instance.UserId, unit = ErpContext.Instance.UnitId;

            string _db_cdate = dateNow, _db_ctime = timeNow, _db_cagent = agent, _db_cunit = unit;
            string _db_mdate = dateNow, _db_mtime = timeNow, _db_magent = agent, _db_munit = unit;

            if (options.Contains("*noSys*"))
            {
                IDictionary<string, string>? opts = (IDictionary<string, string>)type.GetProperty("options",typeof(IDictionary<string, string>)).GetValue(tabModel);  //carico opzioni
                if (opts != null)
                {
                    if (opts.ContainsKey("_db_cdate")) _db_cdate = opts["_db_cdate"];
                    if (opts.ContainsKey("_db_ctime")) _db_cdate = opts["_db_ctime"];
                    if (opts.ContainsKey("_db_cagent")) _db_cdate = opts["_db_cagent"];
                    if (opts.ContainsKey("_db_cunit")) _db_cdate = opts["_db_cunit"];
                    //--
                    if (opts.ContainsKey("_db_mdate")) _db_cdate = opts["_db_mdate"];
                    if (opts.ContainsKey("_db_mtime")) _db_cdate = opts["_db_mtime"];
                    if (opts.ContainsKey("_db_magent")) _db_cdate = opts["_db_magent"];
                    if (opts.ContainsKey("_db_munit")) _db_cdate = opts["_db_munit"];
                }
            }

            //gestione action
            char? action = (char)type.GetProperty("action",typeof(char?)).GetValue(tabModel);  //può assumere solo A[dd], M[odify], D[elete]
            if (action == null || "AMD".Contains((char)action) == false) throw new ArgumentOutOfRangeException(nameof(action));
            if (action == 'A') { sb.AppendLine($"insert into {tab.SqlTableNameExt} ("); sbValues.AppendLine("("); } else { sb.AppendLine($"update {tab.SqlTableNameExt} set "); }
            if (action != 'D')
            {
                foreach (var property in type.GetProperties())
                {
                    string propertyName = property.Name; // Get property name and value
                    if (!dogMng.tabProperties.ContainsKey(propertyName)) continue; //per applicare la condizione la proprietà deve avere un attributo [ErpDogField(..)]
                    object propertyValue = property.GetValue(tabModel);

                    //string fieldOptions = ((ErpDogFieldAttribute)attribute).SqlFieldOptions?.ToString() ?? "";
                    DogManager.DogField fld = dogMng.tabProperties[propertyName];

                    //fill propertyObject
                    object? propertyObject = null;
                    if (action == 'A' && propertyValue == null)
                    {
                        //DEFAULT VALUES
                        if (type == typeof(System.String))
                        {
                            if (fld.optUID || fld.optXID || fld.optXREF)
                            {
                                if (IS_NULLABLE_ID) { propertyObject = DBNull.Value; } else { propertyObject = DogManager.DB_STRING_EMPTY; }
                            }
                            else { propertyObject = DogManager.DB_STRING_EMPTY; }
                        }
                        else if (type == typeof(System.DateOnly?)) { propertyObject = DogManager.DB_DATE_MIN; }
                        else if (type == typeof(System.TimeOnly?)) { propertyObject = DogManager.DB_TIME_EMPTY; }
                        else if (type == typeof(System.DateTime?)) { propertyObject = DogManager.DB_DATETIME_EMPTY; }
                        else if (type == typeof(System.Int16?)) { if (IS_NULLABLE_ID) { propertyObject = DBNull.Value; } else { propertyObject = DogManager.DB_SHORT_EMPTY; } }  //short
                        else if (type == typeof(System.Int64?)) { if (IS_NULLABLE_ID) { propertyObject = DBNull.Value; } else { propertyObject = DogManager.DB_LONG_EMPTY; } }  //long
                        else if (type == typeof(System.Double?)) { if (IS_NULLABLE_ID) { propertyObject = DBNull.Value; } else { propertyObject = DogManager.DB_DOUBLE_EMPTY; } }  //double
                        else if (type == typeof(System.Byte[])) { propertyObject = DBNull.Value; }  //byte[]   ?????????????????????????????
                        else continue;  //  <<<<<<<<<<<<<<<<<<<<<< SALTO I CAMPI NULL
                    }
                    else
                    {
                        if (propertyValue == null) continue;  //  <<<<<<<<<<<<<<<<<<<<<< SALTO I CAMPI NULL

                        // esiste una condizione
                        var sqlFieldNameExt = dogMng.tabProperties[propertyName]?.SqlFieldNameExt?.Trim() ?? "";
                        if (sqlFieldNameExt != "")
                        {
                            //recupero icode
                            if (sqlFieldNameExt == $"{sqlPrefixExt}_ICODE") { icode = (string)propertyValue; }
                            //recupero timestamp
                            if (sqlFieldNameExt == $"{sqlPrefixExt}_TIMESTAMP") { oldTimestamp = (byte[])propertyValue; }
                            //escludo campi di sistema
                            if (sysFieldList.Contains(sqlFieldNameExt.Substring(3))) continue;

                            //gestione tipo campi
                            if (propertyValue is string str)
                            {
                                if (String.IsNullOrEmpty(str) && (fld.optUID || fld.optXID || fld.optXREF))
                                {
                                    if (IS_NULLABLE_ID) { propertyObject = DBNull.Value; } else { propertyObject = DogManager.DB_STRING_EMPTY; }
                                }
                                else propertyObject = (string)str.TrimEnd();
                            }
                            else if (propertyValue is DateTime dattim)  // DateOnly.FromDateTime()
                            {
                                if (fld.optDATE) propertyObject = DateOnly.FromDateTime(dattim);
                                else if (fld.optTIME) propertyObject = TimeOnly.FromDateTime(dattim);
                                else if (fld.optDATETIME) propertyObject = (DateTime)dattim;
                                else throw new ErpException($"{propertyName}: DateTime fa riferimento ad un campo non data ora");
                            }
                            else if (propertyValue is DateOnly dat)
                            {
                                if (fld.optDATE) propertyObject = (DateOnly)dat;
                                else throw new ErpException($"{propertyName}: DateOnly fa riferimento ad un campo non data");
                            }
                            else if (propertyValue is TimeOnly tim)
                            {
                                if (fld.optTIME) propertyObject = (TimeOnly)tim;
                                else throw new ErpException($"{propertyName}: TimeOnly fa riferimento ad un campo non ora");
                            }
                            else if (propertyValue is byte[] bty)
                            {
                                propertyObject = (byte[])bty;
                            }
                            else if (propertyValue is short shr)
                            {
                                if (shr == DogManager.DB_SHORT_EMPTY) continue;  //  <<<<<<<<<<<<<<<<<<<<<< SALTO I CAMPI NULL
                                propertyObject = (short)shr;
                            }
                            else if (propertyValue is long lng)
                            {
                                if (lng == DogManager.DB_LONG_EMPTY) continue;  //  <<<<<<<<<<<<<<<<<<<<<< SALTO I CAMPI NULL
                                propertyObject = (long)lng;
                            }
                            else if (propertyValue is double dbl)
                            {
                                if (dbl == DogManager.DB_DOUBLE_EMPTY) continue;  //  <<<<<<<<<<<<<<<<<<<<<< SALTO I CAMPI NULL
                                propertyObject = (double)dbl;
                            }
                            else throw new ErpException($"{propertyName}: {propertyValue.GetType().Name} non è un tipo consentito"); ;

                        }

                        // Costruzione SQL
                        if (action == 'A') { sb.AppendLine($"{sqlFieldNameExt}, "); sbValues.AppendLine($"{DogManager.addParam(propertyObject, ref parameters)}, "); }
                        else if (action == 'M') { sb.AppendLine($"{sqlFieldNameExt} = {DogManager.addParam(propertyObject, ref parameters)}, "); }
                        else throw new ArgumentOutOfRangeException(nameof(action));
                        numParam++; //condizione applicata correttamente
                    }
                }
            }
            else
            {
                //DELETED
                sb.AppendLine($"{sqlPrefixExt}_DELETED = {DogManager.addParam("Y", ref parameters)}, ");
                if (IS_NULLABLE_INDEX && IS_NULLABLE_ID)
                {  //se cancello il record elimino i campi chiave per evitare problemi di integrita referenziale
                    foreach (var property in type.GetProperties())
                    {
                        string propertyName = property.Name; // Get property name and value
                        if (!dogMng.tabProperties.ContainsKey(propertyName)) continue; //per applicare la condizione la proprietà deve avere un attributo [ErpDogField(..)]
                        DogManager.DogField fld = dogMng.tabProperties[propertyName];
                        var sqlFieldNameExt = dogMng.tabProperties[propertyName]?.SqlFieldNameExt?.Trim() ?? "";
                        if (sqlFieldNameExt != "")
                        {
                            if (fld.optUID || fld.optXID || fld.optXREF) { sb.AppendLine($"{sqlFieldNameExt} = {DogManager.addParam(DBNull.Value, ref parameters)}, "); }
                        }
                    }
                }
            }
            // Verifica condizioni
            if (numParam == 0) throw new ErpException("Nessuna parametro inserito");
            // terminatore di insert update
            if (action == 'A')
            {
                //--
                if (dogMng.DatabaseType != DbTyp.SqlServer && dogMng.DatabaseType != DbTyp.Sybase)
                {
                    sb.AppendLine($"{sqlPrefixExt}_TIMESTAMP, ");
                    sbValues.AppendLine($"{DogManager.addParam(newTimestamp, ref parameters)}, ");
                }
                sb.AppendLine($"{sqlPrefixExt}_ICODE, {sqlPrefixExt}_DELETED, {sqlPrefixExt}_HOME, ");
                sbValues.AppendLine($"{DogManager.addParam(icode, ref parameters)}, {DogManager.addParam("N", ref parameters)}, {DogManager.addParam(dogMng.DbHome, ref parameters)}, ");
                //---
                sb.AppendLine($"{sqlPrefixExt}_CDATE, {sqlPrefixExt}_CTIME, {sqlPrefixExt}_CAGENT, {sqlPrefixExt}_CUNIT, ");
                sbValues.AppendLine($"{DogManager.addParam(_db_cdate, ref parameters)}, {DogManager.addParam(_db_ctime, ref parameters)}, {DogManager.addParam(_db_cagent, ref parameters)}, {DogManager.addParam(_db_cunit, ref parameters)}, ");
                //--
                sb.AppendLine($"{sqlPrefixExt}_MDATE, {sqlPrefixExt}_MTIME, {sqlPrefixExt}_MAGENT, {sqlPrefixExt}_MUNIT");
                sbValues.AppendLine($"{DogManager.addParam(_db_mdate, ref parameters)}, {DogManager.addParam(_db_mtime, ref parameters)}, {DogManager.addParam(_db_magent, ref parameters)}, {DogManager.addParam(_db_munit, ref parameters)}");
                //---
                sb.AppendLine(") values (").Append(sbValues.ToString()).Append(") ");
            }
            else  // modify or delete
            {
                if (dogMng.DatabaseType != DbTyp.SqlServer && dogMng.DatabaseType != DbTyp.Sybase)
                {
                    sb.AppendLine($"{sqlPrefixExt}_TIMESTAMP = {DogManager.addParam(newTimestamp, ref parameters)}, ");
                }
                //--
                sb.AppendLine($"{sqlPrefixExt}_MDATE = {DogManager.addParam(_db_mdate, ref parameters)}, {sqlPrefixExt}_MTIME = {DogManager.addParam(_db_mtime, ref parameters)}, ");
                sb.AppendLine($"{sqlPrefixExt}_MAGENT = {DogManager.addParam(_db_magent, ref parameters)}, {sqlPrefixExt}_MUNIT = {DogManager.addParam(_db_munit, ref parameters)}");
                //--
                sb.AppendLine($" where {sqlPrefixExt}_ICODE = {DogManager.addParam(icode, ref parameters)} and {sqlPrefixExt}_DELETED = {DogManager.addParam('N', ref parameters)}");
                if (options.Contains("*noTms*") == false) sb.Append($" and {sqlPrefixExt}_TIMESTAMP = {DogManager.addParam(oldTimestamp, ref parameters)}");
            }

            //result
            results.Add(new DogResult(type, (char)action, icode, newTimestamp));
            return sb.ToString();
        }

        //crea select per rileggere icode e timestamp dei record presenti nell'elenco result
        //  ...serve quando il timestamp viene generato da DB e non in fase di insert/update (SqlServer/Sybase)
        internal static string sqlSelectIcodeTimestamp(DogManager dogMng, List<DogResult> results, ref IDictionary<string, object> parameters, string options = "")
        {
            //divido per tabelle
            IDictionary<System.Type, List<string>> tabList = new Dictionary<System.Type, List<string>>();
            foreach (DogResult result in results) 
            {
                if (!tabList.ContainsKey(result.TabType)) tabList.Add(result.TabType, new List<string>());
                tabList[result.TabType].Add(result.Icode); 
            }
            //scrivo query
            StringBuilder sb = new StringBuilder(); 
            foreach (System.Type tpy in tabList.Keys) {
                DogManager.DogTable tab = dogMng.tabTypes[tpy];
                if (sb.Length != 0) sb.AppendLine(" union ");
                sb.Append($"select {tab.SqlPrefix}_ICODE as ICODE, {tab.SqlPrefix}_TIMESTAMP as TIMESTAMP from {tab.SqlTableName} where {tab.SqlPrefix}_ICODE in (")
                    .Append(string.Join(", ", DogManager.addListParam(tabList[tpy].Select(str => str.TrimEnd()).ToList<object>(), ref parameters)))
                    .AppendLine(") "); ;
            }
            return sb.ToString();
        }

        //******************************************************************************************************************
        //******************************************************************************************************************

        internal static object? getPropertyValue(object selModel, string propName)
        {
            if (selModel == null) { throw new ArgumentNullException(nameof(selModel)); }
            //ciclo sulle proprietà
            System.Type type = selModel.GetType(); PropertyInfo[] properties = type.GetProperties();
            foreach (var property in properties)
            {
                try
                {
                    string propertyName = property.Name; // Get property name and value
                    object propertyValue = property.GetValue(selModel); //sb.AppendLine($"Property: {propertyName}, Value: {propertyValue}");
                    if (propertyValue == null) continue;
                    // >>> verifica List
                    if (typeof(IEnumerable<object>).IsAssignableFrom(propertyValue.GetType()))
                    {
                        IEnumerable<object> ienum = (IEnumerable<object>)propertyValue;
                        List<object> list = ienum.Where(item => item != null && !(item is string str && string.IsNullOrWhiteSpace(str))).ToList();  // elimina elementi null e strighe vuote
                        if (list.Count() == 0) continue;
                        if (list[0] is string) propertyValue = (List<string>)list.Select(i => i.ToString()).ToList();
                        else if (list[0] is sbyte || list[0] is byte || list[0] is short || list[0] is ushort || list[0] is int || list[0] is uint
                             || list[0] is long || list[0] is ulong) propertyValue = (List<long>)list.Select(i => Convert.ToInt64(i)).ToList();
                        else throw new ErpException("Tipo Lista non supportato (solo stinga e intero)");
                    }
                    //---
                    if (propertyValue is string str)
                    {
                        if (propName == propertyName) return str;
                    }
                    else if (propertyValue is List<string> strList)
                    {
                        for (int i = 0; i < strList.Count; i++)
                            try
                            {
                                if (propName == propertyName + "[" + i.ToString() + "]") return strList[i];
                            }
                            catch (Exception ex) { }  //skip exceptions
                    }
                    else if (propertyValue is List<long> lngList)
                    {
                        for (int i = 0; i < lngList.Count; i++)
                            try
                            {
                                if (propName == propertyName + "[" + i.ToString() + "]") return lngList[i];
                            }
                            catch (Exception ex) { }  //skip exceptions
                    }
                    else if (propertyValue is DateRange dateRng)
                    {
                        if (propName == propertyName + ".StartDate" && dateRng.StartDate != default) return dateRng.StartDate;
                        if (propName == propertyName + ".EndDate" && dateRng.EndDate != default) return dateRng.EndDate;
                    }
                    else continue;
                }
                catch (Exception ex) { }  //skip exceptions
            }
            return null;
        }

        //Custom Model Binder
        internal static bool setPropertyValue(object selModel, string propName, string? propValue)
        {
            if (selModel == null) { throw new ArgumentNullException(nameof(selModel)); }
            if (propName == null) { throw new ArgumentNullException(nameof(propName)); }
            try
            {
                //ciclo sulle proprietà
                System.Type type = selModel.GetType(); PropertyInfo[] properties = type.GetProperties();
                foreach (var property in properties)
                {
                    string propertyName = property.Name; // Get property name and value
                    string attribXref = ((ErpDogFieldAttribute)(property.GetCustomAttribute(typeof(ErpDogFieldAttribute))))?.Xref ?? "";
                    object? propertyValue = property.GetValue(selModel); //sb.AppendLine($"Property: {propertyName}, Value: {propertyValue}");
                    if (propName.StartsWith(propertyName + "[") || propName.StartsWith(attribXref + "["))
                    {
                        if (propValue != null)
                        {
                            System.Type argumentType = propertyValue.GetType().GenericTypeArguments[0];
                            TypeConverter typeConverter = TypeDescriptor.GetConverter(argumentType);
                            object propValueObj = typeConverter.ConvertFromString(propValue);
                            ((IList)propertyValue).Add(propValueObj); property.SetValue(selModel, propertyValue); return true;  //((ICollection<string>)propertyValue).Add(propValue); property.SetValue(selModel, propertyValue);
                        }
                    }
                    else if (propName == propertyName + ".StartDate" || propName == propertyName + ".EndDate")
                    {
                        TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(DateTime));
                        object propValueObj = typeConverter.ConvertFromString(propValue);
                        //---
                        if (propValueObj == null) propValueObj = default;
                        if (propName == propertyName + ".StartDate") ((DateRange)propertyValue).StartDate = (DateTime)propValueObj;
                        if (propName == propertyName + ".EndDate") ((DateRange)propertyValue).EndDate = (DateTime)propValueObj;
                        property.SetValue(selModel, (DateRange)propertyValue); return true;
                    }
                    else if (propName == propertyName || propName == attribXref)
                    {
                        if (typeof(IEnumerable<object>).IsAssignableFrom(propertyValue.GetType()))
                        {
                            if (propValue != null)
                            {
                                System.Type argumentType = propertyValue.GetType().GenericTypeArguments[0];
                                TypeConverter typeConverter = TypeDescriptor.GetConverter(argumentType);
                                object propValueObj = typeConverter.ConvertFromString(propValue);
                                ((IList)propertyValue).Add(propValueObj); property.SetValue(selModel, propertyValue); return true;  //((ICollection<string>)propertyValue).Add(propValue); property.SetValue(selModel, propertyValue);
                            }
                        }
                        else
                        {
                            if (propValue != null)
                            {
                                TypeConverter typeConverter = TypeDescriptor.GetConverter(propertyValue.GetType());
                                object propValueObj = typeConverter.ConvertFromString(propValue);
                                property.SetValue(selModel, propValueObj); return true;
                            }
                            else
                            {
                                property.SetValue(selModel, null); return true;
                            }
                        }
                    }
                    else continue;
                }
            }
            catch (Exception ex) { }  //skip exceptions
            return false;
        }


    }
}
