using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class TipoOrganizzazione : ModelErp {
public const string Description = "Classificazione delle strutture";
public const string SqlTableName = "TIPO_ORGANIZZAZIONE";
public const string SqlTableNameExt = "TIPO_ORGANIZZAZIONE";
public const string SqlRowIdName = "TZ__ID";
public const string SqlRowIdNameExt = "TZ__ICODE";
public const string SqlPrefix = "TZ_";
public const string SqlPrefixExt = "TZ_";
public const string SqlXdataTableName = "TZ_XDATA";
public const string SqlXdataTableNameExt = "TZ_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
public const int INTCODE = 91; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Tz"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
[Display(Name = "Tz1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("TZ__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Tz1Ienv { get; set; }
[Key]
[Display(Name = "Tz1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("TZ__ICODE", SqlFieldNameExt="TZ__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Tz1Icode { get; set; }
[Display(Name = "Tz1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("TZ__DELETED", SqlFieldNameExt="TZ__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Tz1Deleted { get; set; }
[Display(Name = "Tz1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("TZ__TIMESTAMP", SqlFieldNameExt="TZ__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Tz1Timestamp { get; set; }
[Display(Name = "Tz1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("TZ__HOME", SqlFieldNameExt="TZ__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Tz1Home { get; set; }
[Display(Name = "Tz1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("TZ__VERSION", SqlFieldNameExt="TZ__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Tz1Version { get; set; }
[Display(Name = "Tz1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("TZ__INACTIVE", SqlFieldNameExt="TZ__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Tz1Inactive { get; set; }
[Display(Name = "Tz1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("TZ__EXTATT", SqlFieldNameExt="TZ__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Tz1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TZ_CODICE", SqlFieldNameExt="TZ_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_ORGANIZZAZIONE.TZ__ICODE[TZ__ICODE] {TZ_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TzCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TZ_DESCRIZIONE", SqlFieldNameExt="TZ_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TzDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TZ_NOTE", SqlFieldNameExt="TZ_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TzNote  { get; set; }

[Display(Name = "Gruppo", ShortName="", Description = "Classe di aggregazione (se presente)", Prompt="")]
[ErpDogField("TZ_GRUPPO", SqlFieldNameExt="TZ_GRUPPO", SqlFieldOptions="", Xref="Tz1Icode", SqlFieldProperties="prop() xref(TIPO_ORGANIZZAZIONE.TZ__ICODE) xdup() multbxref()")]
[AutocompleteClient("TipoOrganizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? TzGruppo  { get; set; }
public ErpToolkit.Models.SIO.Act.TipoOrganizzazione? TzGruppoObj  { get; set; }

[Display(Name = "Sequenza", ShortName="", Description = "Numero di sequenza nell'aggregazione (se presente)", Prompt="")]
[ErpDogField("TZ_SEQUENZA", SqlFieldNameExt="TZ_SEQUENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
public short? TzSequenza  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
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
