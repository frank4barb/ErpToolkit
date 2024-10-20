using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class RelAttivitaErogataDa : ModelErp {
public const string Description = "Strutture che possono eseguire un certo tipo di attività";
public const string SqlTableName = "REL_ATTIVITA_EROGATA_DA";
public const string SqlTableNameExt = "REL_ATTIVITA_EROGATA_DA";
public const string SqlRowIdName = "AE__ID";
public const string SqlRowIdNameExt = "AE__ICODE";
public const string SqlPrefix = "AE_";
public const string SqlPrefixExt = "AE_";
public const string SqlXdataTableName = "AE_XDATA";
public const string SqlXdataTableNameExt = "AE_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
public const int INTCODE = 81; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Ae"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "Y"; //Is Relation Table: Yes or No

[Display(Name = "Ae1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("AE__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Ae1Ienv { get; set; }
[Key]
[Display(Name = "Ae1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("AE__ICODE", SqlFieldNameExt="AE__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ae1Icode { get; set; }
[Display(Name = "Ae1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("AE__DELETED", SqlFieldNameExt="AE__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ae1Deleted { get; set; }
[Display(Name = "Ae1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("AE__TIMESTAMP", SqlFieldNameExt="AE__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
//[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Ae1Timestamp { get; set; }
[Display(Name = "Ae1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("AE__HOME", SqlFieldNameExt="AE__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ae1Home { get; set; }
[Display(Name = "Ae1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("AE__VERSION", SqlFieldNameExt="AE__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ae1Version { get; set; }
[Display(Name = "Ae1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("AE__INACTIVE", SqlFieldNameExt="AE__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ae1Inactive { get; set; }
[Display(Name = "Ae1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("AE__EXTATT", SqlFieldNameExt="AE__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Ae1Extatt { get; set; }


[Display(Name = "Id Attivita", ShortName="", Description = "Codice dell'attività", Prompt="")]
[ErpDogField("AE_ID_ATTIVITA", SqlFieldNameExt="AE_ID_ATTIVITA", SqlFieldOptions="", Xref="Av1Icode", SqlFieldProperties="prop() xref(ATTIVITA.AV__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteClient("Attivita", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? AeIdAttivita  { get; set; }
public ErpToolkit.Models.SIO.Act.Attivita? AeIdAttivitaObj  { get; set; }

[Display(Name = "Id Unita", ShortName="", Description = "Codice dell'agente autorizzato a eseguire l'attività", Prompt="")]
[ErpDogField("AE_ID_UNITA", SqlFieldNameExt="AE_ID_UNITA", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? AeIdUnita  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? AeIdUnitaObj  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note testuali", Prompt="")]
[ErpDogField("AE_NOTE", SqlFieldNameExt="AE_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(40, ErrorMessage = "Inserire massimo 40 caratteri")]
[DataType(DataType.Text)]
public string? AeNote  { get; set; }

[Display(Name = "Modalita Di Pianificazione", ShortName="", Description = "Modalità di pianificazione predefinita [P]ianificazione - [R]andom", Prompt="")]
[ErpDogField("AE_MODALITA_DI_PIANIFICAZIONE", SqlFieldNameExt="AE_MODALITA_DI_PIANIFICAZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("P")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "P", "R" }, MaxSelections=1, LabelClassName="")]
public string? AeModalitaDiPianificazione  { get; set; }

[Display(Name = "Erogazione Frequente", ShortName="", Description = "Attività frequentemente richiesta (Sì - No)", Prompt="")]
[ErpDogField("AE_EROGAZIONE_FREQUENTE", SqlFieldNameExt="AE_EROGAZIONE_FREQUENTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("N")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "Y", "N" }, MaxSelections=1, LabelClassName="")]
public string? AeErogazioneFrequente  { get; set; }

[Display(Name = "Attributi", ShortName="", Description = "Flag operativi autonomamente gestiti dall'applicazione", Prompt="")]
[ErpDogField("AE_ATTRIBUTI", SqlFieldNameExt="AE_ATTRIBUTI", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(240, ErrorMessage = "Inserire massimo 240 caratteri")]
[DataType(DataType.Text)]
public string? AeAttributi  { get; set; }

[Display(Name = "Filtro Regime Erogazione", ShortName="", Description = "Classi di contatti per cui viene svolta l'attività", Prompt="")]
[ErpDogField("AE_FILTRO_REGIME_EROGAZIONE", SqlFieldNameExt="AE_FILTRO_REGIME_EROGAZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(4, ErrorMessage = "Inserire massimo 4 caratteri")]
[DataType(DataType.Text)]
public string? AeFiltroRegimeErogazione  { get; set; }

public override bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioAe1Icode|K|AE__ICODE","sioAe1RecDate|N|AE__MDATE,AE__CDATE"
        ,"sioAeIdAttivitaaeIdUnita|N|AE_ID_ATTIVITA,AE_ID_UNITA"
        ,"sioAeIdUnitaaeIdAttivitaae1Versionae1Deleted|U|AE_ID_UNITA,AE_ID_ATTIVITA,AE__VERSION,AE__DELETED"
    };
}
}
}
