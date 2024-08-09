using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class RelPrestazioneCampione {
public const string Description = "Campioni utilizzati e/o generati da una prestazione";
public const string SqlTableName = "REL_PRESTAZIONE_CAMPIONE";
public const string SqlTableNameExt = "REL_PRESTAZIONE_CAMPIONE";
public const string SqlRowIdName = "PC__ID";
public const string SqlRowIdNameExt = "PC__ICODE";
public const string SqlPrefix = "PC_";
public const string SqlPrefixExt = "PC_";
public const string SqlXdataTableName = "PC_XDATA";
public const string SqlXdataTableNameExt = "PC_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 102; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Pc"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "Y"; //Is Relation Table: Yes or No
[Display(Name = "Pc1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("PC__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Pc1Ienv { get; set; }
[Key]
[Display(Name = "Pc1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("PC__ICODE", SqlFieldNameExt="PC__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pc1Icode { get; set; }
[Display(Name = "Pc1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("PC__DELETED", SqlFieldNameExt="PC__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Pc1Deleted { get; set; }
[Display(Name = "Pc1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("PC__TIMESTAMP", SqlFieldNameExt="PC__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Pc1Timestamp { get; set; }
[Display(Name = "Pc1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("PC__HOME", SqlFieldNameExt="PC__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pc1Home { get; set; }
[Display(Name = "Pc1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("PC__VERSION", SqlFieldNameExt="PC__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pc1Version { get; set; }
[Display(Name = "Pc1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("PC__INACTIVE", SqlFieldNameExt="PC__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Pc1Inactive { get; set; }
[Display(Name = "Pc1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("PC__EXTATT", SqlFieldNameExt="PC__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Pc1Extatt { get; set; }


[Display(Name = "Id Campione", ShortName="", Description = "Codice del campione", Prompt="")]
[ErpDogField("PC_ID_CAMPIONE", SqlFieldNameExt="PC_ID_CAMPIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref(CAMPIONE.CP__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[AutocompleteServer("Campione", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? PcIdCampione  { get; set; }
public ErpToolkit.Models.SIO.Act.Campione? PcIdCampioneObj  { get; set; }

[Display(Name = "Id Prestazione", ShortName="", Description = "Codice dell'atto", Prompt="")]
[ErpDogField("PC_ID_PRESTAZIONE", SqlFieldNameExt="PC_ID_PRESTAZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref(PRESTAZIONE.PR__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[AutocompleteServer("Prestazione", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? PcIdPrestazione  { get; set; }
public ErpToolkit.Models.SIO.Act.Prestazione? PcIdPrestazioneObj  { get; set; }

[Display(Name = "Tipo", ShortName="", Description = "Generato da / Necessario per l'esecuzione [G/E]", Prompt="")]
[ErpDogField("PC_TIPO", SqlFieldNameExt="PC_TIPO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("E")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("G|E", ErrorMessage = "Inserisci una delle seguenti opzioni: G|E")]
public string? PcTipo  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("PC_NOTE", SqlFieldNameExt="PC_NOTE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? PcNote  { get; set; }

[Display(Name = "Id Tipo Campione", ShortName="", Description = "Tipo di campione", Prompt="")]
[ErpDogField("PC_ID_TIPO_CAMPIONE", SqlFieldNameExt="PC_ID_TIPO_CAMPIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref(TIPO_CAMPIONE.TP__ICODE) xdup(CAMPIONE.CP_ID_TIPO_CAMPIONE[REL_PRESTAZIONE_CAMPIONE.PC_ID_CAMPIONE] {PC_ID_TIPO_CAMPIONE=' '}) multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoCampione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PcIdTipoCampione  { get; set; }
public ErpToolkit.Models.SIO.Act.TipoCampione? PcIdTipoCampioneObj  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioPc1Icode|K|Pc1Icode","sioPc1RecDate|N|Pc1Mdate,Pc1Cdate"
        ,"sioPcIdPrestazionePcIdCampionePcTipoPc1VersionPc1Deleted|U|PcIdPrestazione,PcIdCampione,PcTipo,Pc1Version,Pc1Deleted"
        ,"sioPcIdPrestazionePcTipo|N|PcIdPrestazione,PcTipo"
        ,"sioPcIdTipoCampione|N|PcIdTipoCampione"
        ,"sioPcIdCampione|N|PcIdCampione"
        ,"sioPcIdTipoCampionePcIdPrestazionePcTipo|N|PcIdTipoCampione,PcIdPrestazione,PcTipo"
    };
}
}
}
