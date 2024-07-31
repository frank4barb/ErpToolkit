using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class TipoAttivita {
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
[Display(Name = "Ta1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("TA__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Ta1Ienv { get; set; }
[Key]
[Display(Name = "Ta1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("TA__ICODE", SqlFieldNameExt="TA__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ta1Icode { get; set; }
[Display(Name = "Ta1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("TA__DELETED", SqlFieldNameExt="TA__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ta1Deleted { get; set; }
[Display(Name = "Ta1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("TA__TIMESTAMP", SqlFieldNameExt="TA__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Ta1Timestamp { get; set; }
[Display(Name = "Ta1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("TA__HOME", SqlFieldNameExt="TA__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ta1Home { get; set; }
[Display(Name = "Ta1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("TA__VERSION", SqlFieldNameExt="TA__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ta1Version { get; set; }
[Display(Name = "Ta1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("TA__INACTIVE", SqlFieldNameExt="TA__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ta1Inactive { get; set; }
[Display(Name = "Ta1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("TA__EXTATT", SqlFieldNameExt="TA__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Ta1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TA_CODICE", SqlFieldNameExt="TA_CODICE", SqlFieldProperties="prop() xref() xdup(TIPO_ATTIVITA.TA__ICODE[TA__ICODE] {TA_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TaCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TA_DESCRIZIONE", SqlFieldNameExt="TA_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TaDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TA_NOTE", SqlFieldNameExt="TA_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TaNote  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Superclasse che raggruppa la classificazione corrente", Prompt="")]
[ErpDogField("TA_ID_GRUPPO", SqlFieldNameExt="TA_ID_GRUPPO", SqlFieldProperties="prop() xref(TIPO_ATTIVITA.TA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoAttivita", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? TaIdGruppo  { get; set; }
public ErpToolkit.Models.SIO.Act.TipoAttivita? TaIdGruppoObj  { get; set; }
}
}
