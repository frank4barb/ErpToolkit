using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class SelTipoAttivita {
public const string Description = "Tassonomie e classe di tipi di attività";
public const string SqlTableName = "TIPO_ATTIVITA";
public const string SqlTableNameExt = "TIPO_ATTIVITA";
public const string SqlRowIdName = "TA__ID";
public const string SqlRowIdNameExt = "TA__ICODE";
public const string SqlPrefix = "TA_";
public const string SqlPrefixExt = "TA_";
public const string SqlXdataTableName = "TA_XDATA";
public const string SqlXdataTableNameExt = "TA_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 3; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Ta"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TA_CODICE", SqlFieldNameExt="TA_CODICE", SqlFieldOptions="[UID]", SqlFieldProperties="prop() xref() xdup(TIPO_ATTIVITA.TA__ICODE[TA__ICODE] {TA_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? TaCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TA_DESCRIZIONE", SqlFieldNameExt="TA_DESCRIZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TaDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TA_NOTE", SqlFieldNameExt="TA_NOTE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TaNote  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Superclasse che raggruppa la classificazione corrente", Prompt="")]
[ErpDogField("TA_ID_GRUPPO", SqlFieldNameExt="TA_ID_GRUPPO", SqlFieldOptions="", SqlFieldProperties="prop() xref(TIPO_ATTIVITA.TA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoAttivita", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> TaIdGruppo  { get; set; } = new List<string>();

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
    return new List<string>() { "sioTa1Icode|K|Ta1Icode","sioTa1RecDate|N|Ta1Mdate,Ta1Cdate"
        ,"sioTaIdGruppo|N|TaIdGruppo"
        ,"sioTa1VersionTa1Deleted|U|Ta1Version,Ta1Deleted"
        ,"sioTaCodiceTa1VersionTa1Deleted|U|TaCodice,Ta1Version,Ta1Deleted"
        ,"sioTa1Version|U|Ta1Version"
    };
}
}
}
