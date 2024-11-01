using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
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
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "SEL"; //Data Model Name of the Class
public const int INTCODE = 128; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Di"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice utente del distretto (CAP)", Prompt="")]
[ErpDogField("DI_CODICE", SqlFieldNameExt="DI_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(DISTRETTO.DI__ICODE[DI__ICODE] {DI_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? SelDiCodice  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Descrizione estesa del distretto", Prompt="")]
[ErpDogField("DI_NOME", SqlFieldNameExt="DI_NOME", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelDiNome  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note sul distretto", Prompt="")]
[ErpDogField("DI_NOTE", SqlFieldNameExt="DI_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelDiNote  { get; set; }

[Display(Name = "Id Comune", ShortName="", Description = "Città in cui si trova il distretto", Prompt="")]
[ErpDogField("DI_ID_COMUNE", SqlFieldNameExt="DI_ID_COMUNE", SqlFieldOptions="", Xref="Cm1Icode", SqlFieldProperties="prop() xref(COMUNE.CM__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Comune", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelDiIdComune  { get; set; } = new List<string>();

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
    return new List<string>() { "sioDi1Icode|K|DI__ICODE","sioDi1RecDate|N|DI__MDATE,DI__CDATE"
        ,"sioDiIdComune|N|DI_ID_COMUNE"
        ,"sioDi1Versiondi1Deleted|U|DI__VERSION,DI__DELETED"
        ,"sioDiCodicedi1Versiondi1Deleted|U|DI_CODICE,DI__VERSION,DI__DELETED"
    };
}
}
}
