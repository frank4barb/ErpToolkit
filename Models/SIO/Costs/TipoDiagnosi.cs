using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Costs {
public class TipoDiagnosi {
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
[Display(Name = "Td1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("TD__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Td1Ienv { get; set; }
[Key]
[Display(Name = "Td1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("TD__ICODE", SqlFieldNameExt="TD__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Td1Icode { get; set; }
[Display(Name = "Td1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("TD__DELETED", SqlFieldNameExt="TD__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Td1Deleted { get; set; }
[Display(Name = "Td1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("TD__TIMESTAMP", SqlFieldNameExt="TD__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Td1Timestamp { get; set; }
[Display(Name = "Td1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("TD__HOME", SqlFieldNameExt="TD__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Td1Home { get; set; }
[Display(Name = "Td1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("TD__VERSION", SqlFieldNameExt="TD__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Td1Version { get; set; }
[Display(Name = "Td1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("TD__INACTIVE", SqlFieldNameExt="TD__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Td1Inactive { get; set; }
[Display(Name = "Td1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("TD__EXTATT", SqlFieldNameExt="TD__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Td1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TD_CODICE", SqlFieldNameExt="TD_CODICE", SqlFieldProperties="prop() xref() xdup(TIPO_DIAGNOSI.TD__ICODE[TD__ICODE] {TD_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TdCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TD_DESCRIZIONE", SqlFieldNameExt="TD_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TdDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TD_NOTE", SqlFieldNameExt="TD_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TdNote  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Superclasse che raggruppa la classificazione corrente", Prompt="")]
[ErpDogField("TD_ID_GRUPPO", SqlFieldNameExt="TD_ID_GRUPPO", SqlFieldProperties="prop() xref(TIPO_DIAGNOSI.TD__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoDiagnosi", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? TdIdGruppo  { get; set; }
public ErpToolkit.Models.SIO.Costs.TipoDiagnosi? TdIdGruppoObj  { get; set; }
}
}
