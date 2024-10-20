using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class RelAttivitaRichiestaDa : ModelErp {
public const string Description = "Tipi di attività che possono essere richiesti da un certo operatore/struttura sanitaria";
public const string SqlTableName = "REL_ATTIVITA_RICHIESTA_DA";
public const string SqlTableNameExt = "REL_ATTIVITA_RICHIESTA_DA";
public const string SqlRowIdName = "AR__ID";
public const string SqlRowIdNameExt = "AR__ICODE";
public const string SqlPrefix = "AR_";
public const string SqlPrefixExt = "AR_";
public const string SqlXdataTableName = "AR_XDATA";
public const string SqlXdataTableNameExt = "AR_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
public const int INTCODE = 15; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Ar"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "Y"; //Is Relation Table: Yes or No

[Display(Name = "Ar1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("AR__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Ar1Ienv { get; set; }
[Key]
[Display(Name = "Ar1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("AR__ICODE", SqlFieldNameExt="AR__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ar1Icode { get; set; }
[Display(Name = "Ar1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("AR__DELETED", SqlFieldNameExt="AR__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ar1Deleted { get; set; }
[Display(Name = "Ar1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("AR__TIMESTAMP", SqlFieldNameExt="AR__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
//[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Ar1Timestamp { get; set; }
[Display(Name = "Ar1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("AR__HOME", SqlFieldNameExt="AR__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ar1Home { get; set; }
[Display(Name = "Ar1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("AR__VERSION", SqlFieldNameExt="AR__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ar1Version { get; set; }
[Display(Name = "Ar1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("AR__INACTIVE", SqlFieldNameExt="AR__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ar1Inactive { get; set; }
[Display(Name = "Ar1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("AR__EXTATT", SqlFieldNameExt="AR__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Ar1Extatt { get; set; }


[Display(Name = "Id Attivita", ShortName="", Description = "Codice dell'attività che può essere eseguita", Prompt="")]
[ErpDogField("AR_ID_ATTIVITA", SqlFieldNameExt="AR_ID_ATTIVITA", SqlFieldOptions="", Xref="Av1Icode", SqlFieldProperties="prop() xref(ATTIVITA.AV__ICODE) xdup() multbxref()")]
[AutocompleteClient("Attivita", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? ArIdAttivita  { get; set; }
public ErpToolkit.Models.SIO.Act.Attivita? ArIdAttivitaObj  { get; set; }

[Display(Name = "Id Istituto", ShortName="", Description = "Codice dell'organizzazione che può eseguire l'atto", Prompt="")]
[ErpDogField("AR_ID_ISTITUTO", SqlFieldNameExt="AR_ID_ISTITUTO", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? ArIdIstituto  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? ArIdIstitutoObj  { get; set; }

[Display(Name = "Id Unita", ShortName="", Description = "Codice dell'unità che può eseguire l'atto", Prompt="")]
[ErpDogField("AR_ID_UNITA", SqlFieldNameExt="AR_ID_UNITA", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? ArIdUnita  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? ArIdUnitaObj  { get; set; }

[Display(Name = "Id Postazione", ShortName="", Description = "Codice del punto di servizio (SP) che può eseguire l'atto", Prompt="")]
[ErpDogField("AR_ID_POSTAZIONE", SqlFieldNameExt="AR_ID_POSTAZIONE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? ArIdPostazione  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? ArIdPostazioneObj  { get; set; }

[Display(Name = "Id Operatore", ShortName="", Description = "Codice dell'agente che può eseguire l'atto", Prompt="")]
[ErpDogField("AR_ID_OPERATORE", SqlFieldNameExt="AR_ID_OPERATORE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? ArIdOperatore  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? ArIdOperatoreObj  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note testuali", Prompt="")]
[ErpDogField("AR_NOTE", SqlFieldNameExt="AR_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? ArNote  { get; set; }

[Display(Name = "Richiesta Frequente", ShortName="", Description = "Attività frequentemente richiesta (Sì/No)", Prompt="")]
[ErpDogField("AR_RICHIESTA_FREQUENTE", SqlFieldNameExt="AR_RICHIESTA_FREQUENTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("N")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "Y", "N" }, MaxSelections=1, LabelClassName="")]
public string? ArRichiestaFrequente  { get; set; }

public override bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioAr1Icode|K|AR__ICODE","sioAr1RecDate|N|AR__MDATE,AR__CDATE"
        ,"sioArIdOperatore|N|AR_ID_OPERATORE"
        ,"sioArIdAttivita|N|AR_ID_ATTIVITA"
        ,"sioArIdIstituto|N|AR_ID_ISTITUTO"
        ,"sioArIdPostazione|N|AR_ID_POSTAZIONE"
        ,"sioArIdUnita|N|AR_ID_UNITA"
    };
}
}
}
