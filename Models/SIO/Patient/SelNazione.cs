using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class SelNazione : ModelErp {
public const string Description = "Nazioni";
public const string SqlTableName = "NAZIONE";
public const string SqlTableNameExt = "NAZIONE";
public const string SqlRowIdName = "NZ__ID";
public const string SqlRowIdNameExt = "NZ__ICODE";
public const string SqlPrefix = "NZ_";
public const string SqlPrefixExt = "NZ_";
public const string SqlXdataTableName = "NZ_XDATA";
public const string SqlXdataTableNameExt = "NZ_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "SEL"; //Data Model Name of the Class
public const int INTCODE = 58; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Nz"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice ufficiale (esterno) del paese", Prompt="")]
[ErpDogField("NZ_CODICE", SqlFieldNameExt="NZ_CODICE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelNzCodice  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Nome esteso", Prompt="")]
[ErpDogField("NZ_NOME", SqlFieldNameExt="NZ_NOME", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelNzNome  { get; set; }

[Display(Name = "Cod Istat", ShortName="", Description = "Codice statistico", Prompt="")]
[ErpDogField("NZ_COD_ISTAT", SqlFieldNameExt="NZ_COD_ISTAT", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelNzCodIstat  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("NZ_NOTE", SqlFieldNameExt="NZ_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelNzNote  { get; set; }

public override bool TryValidateInt(ModelStateDictionary modelState, string? prefix = null) 
    { 
        bool isValidate = true; 
        // verifica se almeno un campo indicizzato è valorizzato (test per validazioni complesse del modello) 
        bool found = false; 
        foreach (var idx in ListIndexes()) { 
            string fldLst = idx.Split("|")[2]; 
            foreach (var fld in fldLst.Split(",")) { 
                if (DogManager.getPropertyValue(this, "Sel" + UtilHelper.field2Property(fld.Trim())) != null) found = true; 
                if (DogManager.getPropertyValue(this, "Sel" + UtilHelper.field2Property(fld.Trim()) + "[0]") != null) found = true; 
                if (DogManager.getPropertyValue(this, "Sel" + UtilHelper.field2Property(fld.Trim()) + ".StartDate") != null) found = true; 
                if (DogManager.getPropertyValue(this, "Sel" + UtilHelper.field2Property(fld.Trim()) + ".EndDate") != null) found = true; 
            } 
        } 
        if (!found) { isValidate = false;  modelState.AddModelError(prefix ?? string.Empty, "Deve essere compilato almeno un campo indicizzato."); } 
        //-- 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioNz1Icode|K|NZ__ICODE","sioNz1RecDate|N|NZ__MDATE,NZ__CDATE"
        ,"sioNzNomenz1Versionnz1Deleted|U|NZ_NOME,NZ__VERSION,NZ__DELETED"
        ,"sioNzCodicenz1Versionnz1Deleted|U|NZ_CODICE,NZ__VERSION,NZ__DELETED"
    };
}
}
}
