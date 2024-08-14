using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class Distretto : ModelErp {
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
[Display(Name = "Di1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("DI__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Di1Ienv { get; set; }
[Key]
[Display(Name = "Di1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("DI__ICODE", SqlFieldNameExt="DI__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Di1Icode { get; set; }
[Display(Name = "Di1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("DI__DELETED", SqlFieldNameExt="DI__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Di1Deleted { get; set; }
[Display(Name = "Di1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("DI__TIMESTAMP", SqlFieldNameExt="DI__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Di1Timestamp { get; set; }
[Display(Name = "Di1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("DI__HOME", SqlFieldNameExt="DI__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Di1Home { get; set; }
[Display(Name = "Di1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("DI__VERSION", SqlFieldNameExt="DI__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Di1Version { get; set; }
[Display(Name = "Di1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("DI__INACTIVE", SqlFieldNameExt="DI__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Di1Inactive { get; set; }
[Display(Name = "Di1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("DI__EXTATT", SqlFieldNameExt="DI__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Di1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice utente del distretto (CAP)", Prompt="")]
[ErpDogField("DI_CODICE", SqlFieldNameExt="DI_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(DISTRETTO.DI__ICODE[DI__ICODE] {DI_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? DiCodice  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Descrizione estesa del distretto", Prompt="")]
[ErpDogField("DI_NOME", SqlFieldNameExt="DI_NOME", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? DiNome  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note sul distretto", Prompt="")]
[ErpDogField("DI_NOTE", SqlFieldNameExt="DI_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? DiNote  { get; set; }

[Display(Name = "Id Comune", ShortName="", Description = "Città in cui si trova il distretto", Prompt="")]
[ErpDogField("DI_ID_COMUNE", SqlFieldNameExt="DI_ID_COMUNE", SqlFieldOptions="", Xref="Cm1Icode", SqlFieldProperties="prop() xref(COMUNE.CM__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Comune", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? DiIdComune  { get; set; }
public ErpToolkit.Models.SIO.Patient.Comune? DiIdComuneObj  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
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
