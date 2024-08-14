using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class SelDistretto : ModelErp {
public const string Description = "Distretto territoriale (circoscrizione)";
public const string SqlTableName = "DISTRETTO";
public const string SqlTableNameExt = "DISTRETTO";
public const string SqlRowIdName = "DI__ID";
public const string SqlRowIdNameExt = "DI__ICODE";
public const string SqlPrefix = "DI_";
public const string SqlPrefixExt = "DI_";
public const string SqlXdataTableName = "DI_XDATA";
public const string SqlXdataTableNameExt = "DI_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 128; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Di"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice utente del distretto (CAP)", Prompt="")]
[ErpDogField("DI_CODICE", SqlFieldNameExt="DI_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(DISTRETTO.DI__ICODE[DI__ICODE] {DI_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? DiCodice  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Descrizione estesa del distretto", Prompt="")]
[ErpDogField("DI_NOME", SqlFieldNameExt="DI_NOME", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? DiNome  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note sul distretto", Prompt="")]
[ErpDogField("DI_NOTE", SqlFieldNameExt="DI_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? DiNote  { get; set; }

[Display(Name = "Id Comune", ShortName="", Description = "Città in cui si trova il distretto", Prompt="")]
[ErpDogField("DI_ID_COMUNE", SqlFieldNameExt="DI_ID_COMUNE", SqlFieldOptions="", Xref="Cm1Icode", SqlFieldProperties="prop() xref(COMUNE.CM__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Comune", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> DiIdComune  { get; set; } = new List<string>();

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
    return new List<string>() { "sioDi1Icode|K|Di1Icode","sioDi1RecDate|N|Di1Mdate,Di1Cdate"
        ,"sioDiIdComune|N|DiIdComune"
        ,"sioDi1VersionDi1Deleted|U|Di1Version,Di1Deleted"
        ,"sioDiCodiceDi1VersionDi1Deleted|U|DiCodice,Di1Version,Di1Deleted"
    };
}
}
}
