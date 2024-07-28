using ErpToolkit.Helpers.Db;
using Microsoft.Extensions.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace ErpToolkit.Helpers
{
    public static class DogHelper
    {
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

        //carica list oggetti con il contenuto del DB in base alla struttura in selezione  
        public static List<T> ExecQuery<T>(string dbConnectionString, string sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            T objModel = (T)Activator.CreateInstance(typeof(T)); // create an instance of that type
            //access DB
            DataTable dt = ErpContext.Instance.getSQLSERVERHelper(dbConnectionString).execQuery(sql);  //eg: dbConnectionString="#connectionString_SQLSLocal"
            return SQLSERVERHelper.ConvertDataTable<T>(dt, "");
        }


        //carica list oggetti con il contenuto del DB in base alla struttura in selezione  
        public static List<T> List<T>(string dbConnectionString, object selModel)
        {
            if (selModel == null) { throw new ArgumentNullException(nameof(selModel)); }
            T objModel = (T)Activator.CreateInstance(typeof(T)); // create an instance of that type
            StringBuilder sb = new StringBuilder()
                .Append(sqlSelect(objModel))
                .Append(sqlFrom(objModel))
                .Append(sqlWhere(selModel));
            //access DB
            DataTable dt = ErpContext.Instance.getSQLSERVERHelper(dbConnectionString).execQuery(sb.ToString());  //eg: dbConnectionString="#connectionString_SQLSLocal"
            return SQLSERVERHelper.ConvertDataTable<T>(dt, "");
        }
        //carica row con il contenuto del DB in base all'icode'  
        public static T Row<T>(string dbConnectionString, string icode)
        {
            T objModel = (T)Activator.CreateInstance(typeof(T)); // create an instance of that type
            StringBuilder sb = new StringBuilder()
                .Append(sqlSelect(objModel))
                .Append(sqlFrom(objModel))
                .Append(sqlWhere(objModel, icode));
            //access DB
            DataTable dt = ErpContext.Instance.getSQLSERVERHelper(dbConnectionString).execQuery(sb.ToString()); //eg: dbConnectionString="#connectionString_SQLSLocal"
            if (dt.Rows.Count > 0)
            {
                objModel = SQLSERVERHelper.GetItemDataTable<T>(dt.Rows[0], "");
            }
            return objModel;
        }

        //crea SELECT per l'oggetto del modello 'objModel'
        private static string sqlSelect(object objModel)
        {
            if (objModel == null) { throw new ArgumentNullException(nameof(objModel)); }
            StringBuilder sb = new StringBuilder("select ");
            //ciclo sulle proprietà
            Type type = objModel.GetType(); PropertyInfo[] properties = type.GetProperties();
            foreach (var property in properties)
            {
                try
                {
                    string propertyName = property.Name; // Get property name and value
                    object propertyValue = property.GetValue(objModel); //sb.AppendLine($"Property: {propertyName}, Value: {propertyValue}");
                    var attributes = property.GetCustomAttributes(); // Get custom attributes for the property
                    foreach (var attribute in attributes)
                    {
                        var attributeProperties = attribute.GetType().GetProperties();  // Get attribute properties and their values

                        foreach (var attrProp in attributeProperties) //sb.AppendLine($"    Attribute: {attribute.GetType().Name}");
                        {
                            if (attrProp.CanRead && attrProp.GetIndexParameters().Length == 0) // Some properties of attributes might be methods or indexers, we skip those
                            {
                                try
                                {
                                    var attrPropValue = attrProp.GetValue(attribute); //sb.AppendLine($"        {attrProp.Name}: {attrPropValue}");
                                    if (attribute.GetType().Name == "ErpDogFieldAttribute" && attrProp.Name == "SqlFieldNameExt" && !String.IsNullOrWhiteSpace(attrPropValue.ToString()))
                                    {
                                        sb.AppendLine($" {attrPropValue} as {propertyName},");
                                    }
                                }
                                catch (Exception ex) { }  //skip exceptions
                            }
                        }
                    }
                }
                catch (Exception ex) { }  //skip exceptions
            }
            // terminatore di select
            sb.AppendLine($" ' ' as ErpTerm ");
            return sb.ToString();
        }
        //crea FROM per l'oggetto del modello 'objModel'
        private static string sqlFrom(object objModel)
        {
            if (objModel == null) { throw new ArgumentNullException(nameof(objModel)); }
            string tableName = "";
            Type type = objModel.GetType(); PropertyInfo[] properties = type.GetProperties();
            // Recupera tutte le costanti
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (var field in fields)
            {
                if (field.IsLiteral && !field.IsInitOnly)
                {
                    string constantName = field.Name; // Recupera il nome della costante e il valore
                    object constantValue = field.GetRawConstantValue(); //sb.AppendLine($"Constant: {constantName}, Value: {constantValue}");
                    if (constantName == "SqlTableNameExt") tableName = constantValue.ToString();
                }
            }
            return $"from {tableName} \n";
        }
        //crea WHERE per l'oggetto del modello 'objModel' in base all'oggetto di selezione 'selModel'
        private static string sqlWhere(object selModel)
        {
            int numCond = 0, numPreCond = 0;
            if (selModel == null) { throw new ArgumentNullException(nameof(selModel)); }
            StringBuilder sb = new StringBuilder("where ");
            //ciclo sulle proprietà
            Type type = selModel.GetType(); PropertyInfo[] properties = type.GetProperties();
            foreach (var property in properties)
            {
                try
                {
                    string propertyName = property.Name; // Get property name and value
                    object propertyValue = property.GetValue(selModel); //sb.AppendLine($"Property: {propertyName}, Value: {propertyValue}");
                    if (propertyValue == null) continue;
                    if (typeof(IEnumerable<object>).IsAssignableFrom(propertyValue.GetType()))
                    {
                        IEnumerable<object> ienum = (IEnumerable<object>)propertyValue;
                        List<object> list = ienum.Where(item => item != null && !(item is string str && string.IsNullOrWhiteSpace(str))).ToList();  // elimina elementi null e strighe vuote
                        if (list.Count() == 0) continue;
                        numPreCond++; //condizione prevista
                        if (list[0] is string) propertyValue = (List<string>)list.Select(i => i.ToString()).ToList();
                        else if (list[0] is sbyte || list[0] is byte || list[0] is short || list[0] is ushort || list[0] is int || list[0] is uint
                             || list[0] is long || list[0] is ulong) propertyValue = (List<long>)list.Select(i => Convert.ToInt64(i)).ToList();
                        else throw new ErpException("Tipo Lista non supportato (solo stinga e intero)");
                    }
                    //if (typeof(IEnumerable<object>).IsAssignableFrom(propertyValue.GetType()) && ((IEnumerable<object>)propertyValue).Count == 0) == null) continue;
                    // esiste una condizione
                    var attributes = property.GetCustomAttributes(); // Get custom attributes for the property
                    foreach (var attribute in attributes)
                    {
                        var attributeProperties = attribute.GetType().GetProperties();  // Get attribute properties and their values
                        foreach (var attrProp in attributeProperties) //sb.AppendLine($"    Attribute: {attribute.GetType().Name}");
                        {
                            if (attrProp.CanRead && attrProp.GetIndexParameters().Length == 0) // Some properties of attributes might be methods or indexers, we skip those
                            {
                                try
                                {
                                    var attrPropValue = attrProp.GetValue(attribute); //sb.AppendLine($"        {attrProp.Name}: {attrPropValue}");
                                    if (attribute.GetType().Name == "ErpDogFieldAttribute" && attrProp.Name == "SqlFieldNameExt" && !String.IsNullOrWhiteSpace(attrPropValue.ToString()))
                                    {
                                        if (propertyValue is string str) sb.AppendLine($" {attrPropValue} LIKE '%{propertyValue}%' and ");
                                        else if (propertyValue is List<string> strList) sb.Append($" {attrPropValue} in (").Append(string.Join(", ", strList.Select(str => $"'{str.Trim()}'"))).AppendLine($") and");
                                        else if (propertyValue is List<long> lngList) sb.Append($" {attrPropValue} in (").Append(string.Join(", ", lngList)).AppendLine($") and");
                                        else continue;
                                        numCond++; //condizione applicata correttamente
                                    }
                                }
                                catch (Exception ex) { }  //skip exceptions
                            }
                        }
                    }
                }
                catch (Exception ex) { }  //skip exceptions
            }
            // Verifica condizioni
            if (numCond == 0) throw new ErpException("Nessuna condizione inserita");
            if (numCond != numPreCond) throw new ErpException("Errore nell'applicazione delle condizioni di filtro. Previste {"+numPreCond.ToString()+"}, applicate "+numCond.ToString()+"");
            // Recupera tutte le costanti
            string prefix = "";
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (var field in fields)
            {
                if (field.IsLiteral && !field.IsInitOnly)
                {
                    string constantName = field.Name; // Recupera il nome della costante e il valore
                    object constantValue = field.GetRawConstantValue(); //sb.AppendLine($"Constant: {constantName}, Value: {constantValue}");
                    if (constantName == "SqlPrefixExt") prefix = constantValue.ToString();
                }
            }

            // terminatore di where
            sb.AppendLine($" {prefix}_DELETED='N' ");
            return sb.ToString();
        }
        //crea WHERE per l'oggetto del modello 'objModel' in base all'icode
        private static string sqlWhere(object objModel, string icode)
        {
            if (objModel == null) { throw new ArgumentNullException(nameof(objModel)); }
            string icodeName = "";
            Type type = objModel.GetType(); PropertyInfo[] properties = type.GetProperties();
            // Recupera tutte le costanti
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (var field in fields)
            {
                if (field.IsLiteral && !field.IsInitOnly)
                {
                    string constantName = field.Name; // Recupera il nome della costante e il valore
                    object constantValue = field.GetRawConstantValue(); //sb.AppendLine($"Constant: {constantName}, Value: {constantValue}");
                    if (constantName == "SqlRowIdNameExt") icodeName = constantValue.ToString();
                }
            }
            return $"where {icodeName}='{icode}' ";
        }

        //******************************************************************************************************************

        //----------------------------------------------------------------------------------------------------------------------------------------

        ////carica oggetto con il contenuto del DB per l'icode 
        //public static string Row(object objModel, string icode)
        //{
        //    //if (objModel == null) { throw new ArgumentNullException(nameof(objModel)); }
        //    if (objModel == null) return "";
        //    StringBuilder sb = new StringBuilder("select ");
        //    //ciclo sulle proprietà
        //    Type type = objModel.GetType(); PropertyInfo[] properties = type.GetProperties();
        //    string tableName = "", icodeName = "";
        //    foreach (var property in properties)
        //    {
        //        try
        //        {
        //            string propertyName = property.Name; // Get property name and value
        //            object propertyValue = property.GetValue(objModel); //sb.AppendLine($"Property: {propertyName}, Value: {propertyValue}");
        //            var attributes = property.GetCustomAttributes(); // Get custom attributes for the property
        //            foreach (var attribute in attributes)
        //            {
        //                var attributeProperties = attribute.GetType().GetProperties();  // Get attribute properties and their values

        //                foreach (var attrProp in attributeProperties) //sb.AppendLine($"    Attribute: {attribute.GetType().Name}");
        //                {
        //                    if (attrProp.CanRead && attrProp.GetIndexParameters().Length == 0) // Some properties of attributes might be methods or indexers, we skip those
        //                    {
        //                        try
        //                        {
        //                            var attrPropValue = attrProp.GetValue(attribute); //sb.AppendLine($"        {attrProp.Name}: {attrPropValue}");
        //                            if (attribute.GetType().Name == "ErpDogFieldAttribute" && attrProp.Name == "SqlFieldNameExt" && !String.IsNullOrWhiteSpace(attrPropValue.ToString()))
        //                            {
        //                                sb.AppendLine($" {attrPropValue} as {propertyName},");
        //                            }
        //                        }
        //                        catch (Exception ex) { }  //skip exceptions
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex) { }  //skip exceptions
        //    }
        //    // Recupera tutte le costanti
        //    FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        //    foreach (var field in fields)
        //    {
        //        if (field.IsLiteral && !field.IsInitOnly)
        //        {

        //            string constantName = field.Name; // Recupera il nome della costante e il valore
        //            object constantValue = field.GetRawConstantValue(); //sb.AppendLine($"Constant: {constantName}, Value: {constantValue}");
        //            if (constantName == "SqlTableNameExt") tableName = constantValue.ToString();
        //            if (constantName == "SqlRowIdNameExt") icodeName = constantValue.ToString();
        //        }
        //    }

        //    // terminatore di select
        //    sb.AppendLine($" ' ' as ErpTerm from {tableName} where {icodeName}='{icode}' ");


        //    return sb.ToString();
        //}


    }
}
