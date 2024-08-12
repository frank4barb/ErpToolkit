using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class SelTipoOrganizzazione {
public const string Description = "Classificazione delle strutture";
public const string SqlTableName = "TIPO_ORGANIZZAZIONE";
public const string SqlTableNameExt = "TIPO_ORGANIZZAZIONE";
public const string SqlRowIdName = "TZ__ID";
public const string SqlRowIdNameExt = "TZ__ICODE";
public const string SqlPrefix = "TZ_";
public const string SqlPrefixExt = "TZ_";
public const string SqlXdataTableName = "TZ_XDATA";
public const string SqlXdataTableNameExt = "TZ_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 91; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Tz"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TZ_CODICE", SqlFieldNameExt="TZ_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_ORGANIZZAZIONE.TZ__ICODE[TZ__ICODE] {TZ_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? TzCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TZ_DESCRIZIONE", SqlFieldNameExt="TZ_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TzDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TZ_NOTE", SqlFieldNameExt="TZ_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TzNote  { get; set; }

[Display(Name = "Gruppo", ShortName="", Description = "Classe di aggregazione (se presente)", Prompt="")]
[ErpDogField("TZ_GRUPPO", SqlFieldNameExt="TZ_GRUPPO", SqlFieldOptions="", Xref="Tz1Icode", SqlFieldProperties="prop() xref(TIPO_ORGANIZZAZIONE.TZ__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoOrganizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> TzGruppo  { get; set; } = new List<string>();

[Display(Name = "Sequenza", ShortName="", Description = "Numero di sequenza nell'aggregazione (se presente)", Prompt="")]
[ErpDogField("TZ_SEQUENZA", SqlFieldNameExt="TZ_SEQUENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? TzSequenza  { get; set; }

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
    return new List<string>() { "sioTz1Icode|K|Tz1Icode","sioTz1RecDate|N|Tz1Mdate,Tz1Cdate"
        ,"sioTz1VersionTz1Deleted|U|Tz1Version,Tz1Deleted"
        ,"sioTzCodiceTz1VersionTz1Deleted|U|TzCodice,Tz1Version,Tz1Deleted"
    };
}
}
}
