using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Resource {
public class TipoRisorsa : ModelErp {
public const string Description = "Tipi di risorse disponibili/utilizzate nell'organizzazione sanitaria";
public const string SqlTableName = "TIPO_RISORSA";
public const string SqlTableNameExt = "TIPO_RISORSA";
public const string SqlRowIdName = "TS__ID";
public const string SqlRowIdNameExt = "TS__ICODE";
public const string SqlPrefix = "TS_";
public const string SqlPrefixExt = "TS_";
public const string SqlXdataTableName = "TS_XDATA";
public const string SqlXdataTableNameExt = "TS_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 20; //Internal Table Code
public const string TBAREA = "Risorse"; //Table Area
public const string PREFIX = "Ts"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//126-124//REL_PRESTAZIONE_USA.PU_ID_TIPO_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa> RelPrestazioneUsa4PuIdTipoRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa>();
//1181-1179//REL_ATTIVITA_USA.AU_ID_TIPO_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa> RelAttivitaUsa4AuIdTipoRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa>();
[Display(Name = "Ts1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("TS__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Ts1Ienv { get; set; }
[Key]
[Display(Name = "Ts1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("TS__ICODE", SqlFieldNameExt="TS__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ts1Icode { get; set; }
[Display(Name = "Ts1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("TS__DELETED", SqlFieldNameExt="TS__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ts1Deleted { get; set; }
[Display(Name = "Ts1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("TS__TIMESTAMP", SqlFieldNameExt="TS__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Ts1Timestamp { get; set; }
[Display(Name = "Ts1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("TS__HOME", SqlFieldNameExt="TS__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ts1Home { get; set; }
[Display(Name = "Ts1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("TS__VERSION", SqlFieldNameExt="TS__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ts1Version { get; set; }
[Display(Name = "Ts1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("TS__INACTIVE", SqlFieldNameExt="TS__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ts1Inactive { get; set; }
[Display(Name = "Ts1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("TS__EXTATT", SqlFieldNameExt="TS__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Ts1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TS_CODICE", SqlFieldNameExt="TS_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_RISORSA.TS__ICODE[TS__ICODE] {TS_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TsCodice  { get; set; }

[Display(Name = "Classe Risorsa", ShortName="", Description = "Classe: E[quipments] - L[ocations] - S[taff] - M[aterial] - [G]eneric", Prompt="")]
[ErpDogField("TS_CLASSE_RISORSA", SqlFieldNameExt="TS_CLASSE_RISORSA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("M")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "E", "L", "M", "D", "S", "G" }, MaxSelections=1, LabelClassName="")]
public string? TsClasseRisorsa  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Codice del super-tipo di risorsa (cioè l'aggregazione nella gerarchia), se presente", Prompt="")]
[ErpDogField("TS_ID_GRUPPO", SqlFieldNameExt="TS_ID_GRUPPO", SqlFieldOptions="", Xref="Ts1Icode", SqlFieldProperties="prop() xref(TIPO_RISORSA.TS__ICODE) xdup() multbxref()")]
[AutocompleteClient("TipoRisorsa", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? TsIdGruppo  { get; set; }
public ErpToolkit.Models.SIO.Resource.TipoRisorsa? TsIdGruppoObj  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TS_DESCRIZIONE", SqlFieldNameExt="TS_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TsDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TS_NOTE", SqlFieldNameExt="TS_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TsNote  { get; set; }

[Display(Name = "Unita Di Misura", ShortName="", Description = "Unità di misura", Prompt="")]
[ErpDogField("TS_UNITA_DI_MISURA", SqlFieldNameExt="TS_UNITA_DI_MISURA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TsUnitaDiMisura  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioTs1Icode|K|Ts1Icode","sioTs1RecDate|N|Ts1Mdate,Ts1Cdate"
        ,"sioTsClasseRisorsaTs1VersionTs1Deleted|U|TsClasseRisorsa,Ts1Version,Ts1Deleted"
        ,"sioTsIdGruppo|N|TsIdGruppo"
        ,"sioTsCodiceTs1VersionTs1Deleted|U|TsCodice,Ts1Version,Ts1Deleted"
    };
}
}
}
