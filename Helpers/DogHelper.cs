using ErpToolkit.Helpers.Db;
using Google.Protobuf.Reflection;
using Microsoft.Extensions.Primitives;
using NLog.LayoutRenderers.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.Xml;
using System.Text;

namespace ErpToolkit.Helpers
{
    public static class DogHelper
    {

        private const string DB_FORMAT_DATE = "yyyy/MM/dd"; //formato stringa di memorizzazione della data nel DB

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
                return (new string (new char[] { Readonly, Visible }));
            }
            public FieldAttr(string attr) { setAttr(attr); }
            public FieldAttr(bool readOnly, bool visible) { if(readOnly) Readonly = 'Y'; if (!visible) Visible = 'N'; }
            public static string strAttr(bool readOnly, bool visible) { return new FieldAttr(readOnly, visible).getAttr();  }
        }


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
                    //xx//if (propertyName == "AttrFields") continue; // SGANCIO DAL MODELLO IL CONCETTO DI VISIBILITA'
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
                    // >>> verifica DateRange
                    if (propertyValue is DateRange dateRange)
                    {
                        if (dateRange.StartDate == default && dateRange.EndDate == default) continue; //entrambe le date sono null
                    }
                    //---
                    numPreCond++; //condizione prevista
                    //---
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
                                        string fieldOptions = ((ErpDogFieldAttribute)attribute).SqlFieldOptions?.ToString() ?? "";
                                        if (propertyValue is string str)
                                        {
                                            if (fieldOptions.Contains("[UID]")) sb.AppendLine($" {attrPropValue} = '{str.Trim()}' and ");
                                            else if (fieldOptions.Contains("[XID]")) sb.AppendLine($" {attrPropValue} = '{str.Trim()}' and ");
                                            else sb.AppendLine($" {attrPropValue} LIKE '%{str}%' and ");
                                        }
                                        else if (propertyValue is List<string> strList) sb.Append($" {attrPropValue} in (").Append(string.Join(", ", strList.Select(str => $"'{str.Trim()}'"))).AppendLine($") and");
                                        else if (propertyValue is List<long> lngList) sb.Append($" {attrPropValue} in (").Append(string.Join(", ", lngList)).AppendLine($") and");
                                        else if (propertyValue is DateRange dateRng)
                                        {
                                            if (dateRng.StartDate == default) sb.AppendLine($" {attrPropValue} <= '{dateRng.EndDate.ToString(DB_FORMAT_DATE)}' and");
                                            else if (dateRng.EndDate == default) sb.AppendLine($" {attrPropValue} >= '{dateRng.StartDate.ToString(DB_FORMAT_DATE)}' and");
                                            else sb.AppendLine($" ({attrPropValue} BETWEEN '{dateRng.StartDate.ToString(DB_FORMAT_DATE)}' and '{dateRng.EndDate.ToString(DB_FORMAT_DATE)}') and");
                                        }
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

        public static object? getPropertyValue(object selModel, string propName)
        {
            if (selModel == null) { throw new ArgumentNullException(nameof(selModel)); }
            //ciclo sulle proprietà
            Type type = selModel.GetType(); PropertyInfo[] properties = type.GetProperties();
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
                                if (propName == propertyName+"["+i.ToString()+"]") return strList[i];
                            }
                            catch (Exception ex) { }  //skip exceptions
                    }
                    else if (propertyValue is List<long> lngList) {
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
        public static bool setPropertyValue(object selModel, string propName, string? propValue)
        {
            if (selModel == null) { throw new ArgumentNullException(nameof(selModel)); }
            if (propName == null) { throw new ArgumentNullException(nameof(propName)); }
            try
            {
                //ciclo sulle proprietà
                Type type = selModel.GetType(); PropertyInfo[] properties = type.GetProperties();
                foreach (var property in properties)
                {
                    string propertyName = property.Name; // Get property name and value
                    string attribXref = ((ErpDogFieldAttribute)(property.GetCustomAttribute(typeof(ErpDogFieldAttribute))))?.Xref ?? "";
                    object? propertyValue = property.GetValue(selModel); //sb.AppendLine($"Property: {propertyName}, Value: {propertyValue}");
                    if (propName.StartsWith(propertyName + "[") || propName.StartsWith(attribXref + "["))
                    {
                        if (propValue != null)
                        {
                            Type argumentType = propertyValue.GetType().GenericTypeArguments[0];
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
                                Type argumentType = propertyValue.GetType().GenericTypeArguments[0];
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
                    //xx// SGANCIO DAL MODELLO IL CONCETTO DI VISIBILITA'
                    //xx//else if ((propName == propertyName + "_Attr" || propName == attribXref + "_Attr") && propValue != null)
                    //xx//{
                    //xx//    PropertyInfo? propertyAttrFields = type.GetProperty("AttrFields");
                    //xx//    if (propertyAttrFields != null) {
                    //xx//        Dictionary<string, FieldAttr>? attrFields = (Dictionary<string, FieldAttr>?)propertyAttrFields.GetValue(selModel);
                    //xx//        if (attrFields != null) { 
                    //xx//            if (attrFields.ContainsKey(propertyName)) attrFields[propertyName].setAttr(propValue);
                    //xx//            else attrFields[propertyName] = new DogHelper.FieldAttr(propValue);
                    //xx//            propertyAttrFields.SetValue(selModel, attrFields); }
                    //xx//    }
                    //xx//}
                    //// imposta gli attributi dei campi  ===> NON FUNZIONA XCHE' GLI ATTRIBUTI POSSONO ESSERE SOLO LETTI
                    //else if ( (propName == propertyName + "_Attr" || propName == attribXref + "_Attr") && propValue != null )
                    //{
                    //    Char[] attrVal = propValue.ToCharArray(); Attribute? attr = null;
                    //    for (int i = 0; i < attrVal.Length; i++)
                    //    {
                    //        switch (i)
                    //        {
                    //            case 0: // 0) ReadOnly
                    //                attr = ((ErpDogFieldAttribute)(property.GetCustomAttribute(typeof(ErpDogFieldAttribute), false)));
                    //                if (attr != null && attrVal[i] == 'Y') ((ErpDogFieldAttribute)attr).Readonly = true;
                    //                else if (attr != null && attrVal[i] == 'N') ((ErpDogFieldAttribute)attr).Readonly = false;
                    //                break;
                    //            case 1: // 1) Visible
                    //                attr = ((ErpDogFieldAttribute)(property.GetCustomAttribute(typeof(ErpDogFieldAttribute), false)));
                    //                if (attr != null && attrVal[i] == 'Y') ((ErpDogFieldAttribute)attr).Visible = true;
                    //                else if (attr != null && attrVal[i] == 'N') ((ErpDogFieldAttribute)attr).Visible = false;
                    //                break;
                    //         }
                    //    }
                    //    PropertyInfo property2 = selModel.GetType().GetProperty("EpIdTipoEpisodio");
                    //    bool testRes = ((ErpDogFieldAttribute)(property2.GetCustomAttribute(typeof(ErpDogFieldAttribute), false))).Readonly;
                    //    return true;
                    //}
                    else continue;
                }
            }
            catch (Exception ex) { }  //skip exceptions
            return false;
        }



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
