using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Costs {
public class SelTipoDiagnosi {
public const string Description = "Tipi generali di classificazioni diagnostiche.";
public const string SqlTableName = "TIPO_DIAGNOSI";
public const string SqlTableNameExt = "TIPO_DIAGNOSI";
public const string SqlRowIdName = "TD__ID";
public const string SqlRowIdNameExt = "TD__ICODE";
public const string SqlPrefix = "TD_";
public const string SqlPrefixExt = "TD_";
public const string SqlXdataTableName = "TD_XDATA";
public const string SqlXdataTableNameExt = "TD_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 114; //Internal Table Code
public const string TBAREA = "Controllo di gestione"; //Table Area
public const string PREFIX = "Td"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TD_CODICE", SqlFieldNameExt="TD_CODICE", SqlFieldOptions="[UID]", SqlFieldProperties="prop() xref() xdup(TIPO_DIAGNOSI.TD__ICODE[TD__ICODE] {TD_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? TdCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TD_DESCRIZIONE", SqlFieldNameExt="TD_DESCRIZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TdDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TD_NOTE", SqlFieldNameExt="TD_NOTE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TdNote  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Superclasse che raggruppa la classificazione corrente", Prompt="")]
[ErpDogField("TD_ID_GRUPPO", SqlFieldNameExt="TD_ID_GRUPPO", SqlFieldOptions="", SqlFieldProperties="prop() xref(TIPO_DIAGNOSI.TD__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoDiagnosi", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> TdIdGruppo  { get; set; } = new List<string>();

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
    return new List<string>() { "sioTd1Icode|K|Td1Icode","sioTd1RecDate|N|Td1Mdate,Td1Cdate"
        ,"sioTdIdGruppo|N|TdIdGruppo"
        ,"sioTd1VersionTd1Deleted|U|Td1Version,Td1Deleted"
        ,"sioTdCodiceTd1VersionTd1Deleted|U|TdCodice,Td1Version,Td1Deleted"
    };
}
}
}
