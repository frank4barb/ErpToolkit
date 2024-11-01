using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class SelComune : ModelErp {
public const string Description = "Comuni";
public const string SqlTableName = "COMUNE";
public const string SqlTableNameExt = "COMUNE";
public const string SqlRowIdName = "CM__ID";
public const string SqlRowIdNameExt = "CM__ICODE";
public const string SqlPrefix = "CM_";
public const string SqlPrefixExt = "CM_";
public const string SqlXdataTableName = "CM_XDATA";
public const string SqlXdataTableNameExt = "CM_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "SEL"; //Data Model Name of the Class
public const int INTCODE = 55; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Cm"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice nazionale della città", Prompt="")]
[ErpDogField("CM_CODICE", SqlFieldNameExt="CM_CODICE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelCmCodice  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Nome esteso", Prompt="")]
[ErpDogField("CM_NOME", SqlFieldNameExt="CM_NOME", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelCmNome  { get; set; }

[Display(Name = "Cod Istat", ShortName="", Description = "Codice statistico per la città", Prompt="")]
[ErpDogField("CM_COD_ISTAT", SqlFieldNameExt="CM_COD_ISTAT", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelCmCodIstat  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note testuali", Prompt="")]
[ErpDogField("CM_NOTE", SqlFieldNameExt="CM_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelCmNote  { get; set; }

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
    return new List<string>() { "sioCm1Icode|K|CM__ICODE","sioCm1RecDate|N|CM__MDATE,CM__CDATE"
        ,"sioCmNome|N|CM_NOME"
    };
}
}
}
