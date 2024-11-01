using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class RelOrganizzazioneContiene : ModelErp {
public const string Description = "Relazioni generiche esistenti tra diverse strutture";
public const string SqlTableName = "REL_ORGANIZZAZIONE_CONTIENE";
public const string SqlTableNameExt = "REL_ORGANIZZAZIONE_CONTIENE";
public const string SqlRowIdName = "OO__ID";
public const string SqlRowIdNameExt = "OO__ICODE";
public const string SqlPrefix = "OO_";
public const string SqlPrefixExt = "OO_";
public const string SqlXdataTableName = "OO_XDATA";
public const string SqlXdataTableNameExt = "OO_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
public const int INTCODE = 115; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Oo"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "Y"; //Is Relation Table: Yes or No

[Display(Name = "Oo1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("OO__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Oo1Ienv { get; set; }
[Key]
[Display(Name = "Oo1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("OO__ICODE", SqlFieldNameExt="OO__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Oo1Icode { get; set; }
[Display(Name = "Oo1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("OO__DELETED", SqlFieldNameExt="OO__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Oo1Deleted { get; set; }
[Display(Name = "Oo1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("OO__TIMESTAMP", SqlFieldNameExt="OO__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
//[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Oo1Timestamp { get; set; }
[Display(Name = "Oo1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("OO__HOME", SqlFieldNameExt="OO__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Oo1Home { get; set; }
[Display(Name = "Oo1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("OO__VERSION", SqlFieldNameExt="OO__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Oo1Version { get; set; }
[Display(Name = "Oo1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("OO__INACTIVE", SqlFieldNameExt="OO__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Oo1Inactive { get; set; }
[Display(Name = "Oo1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("OO__EXTATT", SqlFieldNameExt="OO__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Oo1Extatt { get; set; }


[Display(Name = "Id Organizzazione Padre", ShortName="", Description = "Codice del primo agente correlato all'altro", Prompt="")]
[ErpDogField("OO_ID_ORGANIZZAZIONE_PADRE", SqlFieldNameExt="OO_ID_ORGANIZZAZIONE_PADRE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? OoIdOrganizzazionePadre  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? OoIdOrganizzazionePadreObj  { get; set; }

[Display(Name = "Id Organizzazione Figlio", ShortName="", Description = "Codice del secondo agente correlato al primo", Prompt="")]
[ErpDogField("OO_ID_ORGANIZZAZIONE_FIGLIO", SqlFieldNameExt="OO_ID_ORGANIZZAZIONE_FIGLIO", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? OoIdOrganizzazioneFiglio  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? OoIdOrganizzazioneFiglioObj  { get; set; }

[Display(Name = "Regola Di Inclusione", ShortName="", Description = "Ruolo della relazione tra i due agenti", Prompt="")]
[ErpDogField("OO_REGOLA_DI_INCLUSIONE", SqlFieldNameExt="OO_REGOLA_DI_INCLUSIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? OoRegolaDiInclusione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note opzionali riguardo alla relazione tra gli agenti", Prompt="")]
[ErpDogField("OO_NOTE", SqlFieldNameExt="OO_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? OoNote  { get; set; }

public override bool TryValidateInt(ModelStateDictionary modelState, string? prefix = null) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioOo1Icode|K|OO__ICODE","sioOo1RecDate|N|OO__MDATE,OO__CDATE"
        ,"sioOoIdOrganizzazionePadreooIdOrganizzazioneFiglioooRegolaDiInclusione|N|OO_ID_ORGANIZZAZIONE_PADRE,OO_ID_ORGANIZZAZIONE_FIGLIO,OO_REGOLA_DI_INCLUSIONE"
        ,"sioOoIdOrganizzazioneFiglioooIdOrganizzazionePadre|N|OO_ID_ORGANIZZAZIONE_FIGLIO,OO_ID_ORGANIZZAZIONE_PADRE"
    };
}
}
}
