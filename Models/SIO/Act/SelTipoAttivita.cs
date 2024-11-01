using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class SelTipoAttivita : ModelErp {
public const string Description = "Tassonomie e classe di tipi di attività";
public const string SqlTableName = "TIPO_ATTIVITA";
public const string SqlTableNameExt = "TIPO_ATTIVITA";
public const string SqlRowIdName = "TA__ID";
public const string SqlRowIdNameExt = "TA__ICODE";
public const string SqlPrefix = "TA_";
public const string SqlPrefixExt = "TA_";
public const string SqlXdataTableName = "TA_XDATA";
public const string SqlXdataTableNameExt = "TA_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "SEL"; //Data Model Name of the Class
public const int INTCODE = 3; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Ta"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TA_CODICE", SqlFieldNameExt="TA_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_ATTIVITA.TA__ICODE[TA__ICODE] {TA_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? SelTaCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TA_DESCRIZIONE", SqlFieldNameExt="TA_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelTaDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TA_NOTE", SqlFieldNameExt="TA_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelTaNote  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Superclasse che raggruppa la classificazione corrente", Prompt="")]
[ErpDogField("TA_ID_GRUPPO", SqlFieldNameExt="TA_ID_GRUPPO", SqlFieldOptions="", Xref="Ta1Icode", SqlFieldProperties="prop() xref(TIPO_ATTIVITA.TA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoAttivita", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelTaIdGruppo  { get; set; } = new List<string>();

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
    return new List<string>() { "sioTa1Icode|K|TA__ICODE","sioTa1RecDate|N|TA__MDATE,TA__CDATE"
        ,"sioTaIdGruppo|N|TA_ID_GRUPPO"
        ,"sioTa1Versionta1Deleted|U|TA__VERSION,TA__DELETED"
        ,"sioTaCodiceta1Versionta1Deleted|U|TA_CODICE,TA__VERSION,TA__DELETED"
        ,"sioTa1Version|U|TA__VERSION"
    };
}
}
}
