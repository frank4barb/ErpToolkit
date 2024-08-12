using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.HealthData {
public class SelCategoriaDatoClinico {
public const string Description = "Classificazione dei tipi di dati sanitari";
public const string SqlTableName = "CATEGORIA_DATO_CLINICO";
public const string SqlTableNameExt = "CATEGORIA_DATO_CLINICO";
public const string SqlRowIdName = "CC__ID";
public const string SqlRowIdNameExt = "CC__ICODE";
public const string SqlPrefix = "CC_";
public const string SqlPrefixExt = "CC_";
public const string SqlXdataTableName = "CC_XDATA";
public const string SqlXdataTableNameExt = "CC_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 16; //Internal Table Code
public const string TBAREA = "Dati clinici"; //Table Area
public const string PREFIX = "Cc"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("CC_CODICE", SqlFieldNameExt="CC_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(CATEGORIA_DATO_CLINICO.CC__ICODE[CC__ICODE] {CC_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? CcCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("CC_DESCRIZIONE", SqlFieldNameExt="CC_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? CcDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("CC_NOTE", SqlFieldNameExt="CC_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? CcNote  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Codice della superclasse che raggruppa la classe attuale", Prompt="")]
[ErpDogField("CC_ID_GRUPPO", SqlFieldNameExt="CC_ID_GRUPPO", SqlFieldOptions="", Xref="Cc1Icode", SqlFieldProperties="prop() xref(CATEGORIA_DATO_CLINICO.CC__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("CategoriaDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> CcIdGruppo  { get; set; } = new List<string>();

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
    return new List<string>() { "sioCc1Icode|K|Cc1Icode","sioCc1RecDate|N|Cc1Mdate,Cc1Cdate"
        ,"sioCcIdGruppo|N|CcIdGruppo"
        ,"sioCc1VersionCc1Deleted|U|Cc1Version,Cc1Deleted"
        ,"sioCcCodiceCc1VersionCc1Deleted|U|CcCodice,Cc1Version,Cc1Deleted"
    };
}
}
}
