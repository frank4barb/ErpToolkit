using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.HealthData {
public class CategoriaDatoClinico {
public const string Description = "Classificazione dei tipi di dati sanitari ... intcode:[16] prefix:[CC_] has_xdt:[CC_XDATA] is_xdt:[0] ";
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
[Display(Name = "Cc1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("CC__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Cc1Ienv { get; set; }
[Key]
[Display(Name = "Cc1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("CC__ICODE", SqlFieldNameExt="CC__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Cc1Icode { get; set; }
[Display(Name = "Cc1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("CC__DELETED", SqlFieldNameExt="CC__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Cc1Deleted { get; set; }
[Display(Name = "Cc1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("CC__TIMESTAMP", SqlFieldNameExt="CC__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Cc1Timestamp { get; set; }
[Display(Name = "Cc1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("CC__HOME", SqlFieldNameExt="CC__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Cc1Home { get; set; }
[Display(Name = "Cc1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("CC__VERSION", SqlFieldNameExt="CC__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Cc1Version { get; set; }
[Display(Name = "Cc1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("CC__INACTIVE", SqlFieldNameExt="CC__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Cc1Inactive { get; set; }
[Display(Name = "Cc1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("CC__EXTATT", SqlFieldNameExt="CC__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Cc1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("CC_CODICE", SqlFieldNameExt="CC_CODICE", SqlFieldProperties="prop() xref() xdup(CATEGORIA_DATO_CLINICO.CC__ICODE[CC__ICODE] {CC_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? CcCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("CC_DESCRIZIONE", SqlFieldNameExt="CC_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? CcDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("CC_NOTE", SqlFieldNameExt="CC_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? CcNote  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Codice della superclasse che raggruppa la classe attuale", Prompt="")]
[ErpDogField("CC_ID_GRUPPO", SqlFieldNameExt="CC_ID_GRUPPO", SqlFieldProperties="prop() xref(CATEGORIA_DATO_CLINICO.CC__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("CategoriaDatoClinico", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? CcIdGruppo  { get; set; }
public ErpToolkit.Models.SIO.HealthData.CategoriaDatoClinico? CcIdGruppoObj  { get; set; }
}
}
