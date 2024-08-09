using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class SelNazione {
public const string Description = "Nazioni";
public const string SqlTableName = "NAZIONE";
public const string SqlTableNameExt = "NAZIONE";
public const string SqlRowIdName = "NZ__ID";
public const string SqlRowIdNameExt = "NZ__ICODE";
public const string SqlPrefix = "NZ_";
public const string SqlPrefixExt = "NZ_";
public const string SqlXdataTableName = "NZ_XDATA";
public const string SqlXdataTableNameExt = "NZ_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 58; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Nz"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice ufficiale (esterno) del paese", Prompt="")]
[ErpDogField("NZ_CODICE", SqlFieldNameExt="NZ_CODICE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? NzCodice  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Nome esteso", Prompt="")]
[ErpDogField("NZ_NOME", SqlFieldNameExt="NZ_NOME", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? NzNome  { get; set; }

[Display(Name = "Cod Istat", ShortName="", Description = "Codice statistico", Prompt="")]
[ErpDogField("NZ_COD_ISTAT", SqlFieldNameExt="NZ_COD_ISTAT", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? NzCodIstat  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("NZ_NOTE", SqlFieldNameExt="NZ_NOTE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? NzNote  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        // verifica se almeno un campo indicizzato è valorizzato (test per validazioni complesse del modello) 
        bool found = false; 
        foreach (var idx in ListIndexes()) { 
            string fldLst = idx.Split("|")[2]; 
            foreach (var fld in fldLst.Split(",")) { 
                if (DogHelper.getPropertyValue(this, fld.Trim()) != null) found = true; 
                if (DogHelper.getPropertyValue(this, fld.Trim() + "[0]") != null) found = true; 
                if (DogHelper.getPropertyValue(this, fld.Trim() + ".StartDate") != null) found = true; 
                if (DogHelper.getPropertyValue(this, fld.Trim() + ".EndDate") != null) found = true; 
            } 
        } 
        if (!found) { isValidate = false;  modelState.AddModelError(string.Empty, "Deve essere compilato almeno un campo indicizzato."); } 
        //-- 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioNz1Icode|K|Nz1Icode","sioNz1RecDate|N|Nz1Mdate,Nz1Cdate"
        ,"sioNzNomeNz1VersionNz1Deleted|U|NzNome,Nz1Version,Nz1Deleted"
        ,"sioNzCodiceNz1VersionNz1Deleted|U|NzCodice,Nz1Version,Nz1Deleted"
    };
}
}
}
