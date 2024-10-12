using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Common {
public class Richiesta : ModelErp {
public const string Description = "Comunicazione e/o richiesta di prestazioni";
public const string SqlTableName = "RICHIESTA";
public const string SqlTableNameExt = "RICHIESTA";
public const string SqlRowIdName = "RI__ID";
public const string SqlRowIdNameExt = "RI__ICODE";
public const string SqlPrefix = "RI_";
public const string SqlPrefixExt = "RI_";
public const string SqlXdataTableName = "RI_XDATA";
public const string SqlXdataTableNameExt = "RI_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
public const int INTCODE = 46; //Internal Table Code
public const string TBAREA = "Organizzazione ospedaliera"; //Table Area
public const string PREFIX = "Ri"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
[Display(Name = "Ri1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("RI__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Ri1Ienv { get; set; }
[Key]
[Display(Name = "Ri1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("RI__ICODE", SqlFieldNameExt="RI__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ri1Icode { get; set; }
[Display(Name = "Ri1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("RI__DELETED", SqlFieldNameExt="RI__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ri1Deleted { get; set; }
[Display(Name = "Ri1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("RI__TIMESTAMP", SqlFieldNameExt="RI__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Ri1Timestamp { get; set; }
[Display(Name = "Ri1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("RI__HOME", SqlFieldNameExt="RI__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ri1Home { get; set; }
[Display(Name = "Ri1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("RI__VERSION", SqlFieldNameExt="RI__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ri1Version { get; set; }
[Display(Name = "Ri1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("RI__INACTIVE", SqlFieldNameExt="RI__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ri1Inactive { get; set; }
[Display(Name = "Ri1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("RI__EXTATT", SqlFieldNameExt="RI__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Ri1Extatt { get; set; }


[Display(Name = "Id Unita Richiedente", ShortName="", Description = "Codice dell'unità che ha originato la comunicazione", Prompt="")]
[ErpDogField("RI_ID_UNITA_RICHIEDENTE", SqlFieldNameExt="RI_ID_UNITA_RICHIEDENTE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? RiIdUnitaRichiedente  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? RiIdUnitaRichiedenteObj  { get; set; }

[Display(Name = "Id Postazione Richiedente", ShortName="", Description = "Codice del punto di servizio che ha originato la comunicazione", Prompt="")]
[ErpDogField("RI_ID_POSTAZIONE_RICHIEDENTE", SqlFieldNameExt="RI_ID_POSTAZIONE_RICHIEDENTE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? RiIdPostazioneRichiedente  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? RiIdPostazioneRichiedenteObj  { get; set; }

[Display(Name = "Id Istituto Richiedente", ShortName="", Description = "Codice dell'organizzazione che ha originato la comunicazione", Prompt="")]
[ErpDogField("RI_ID_ISTITUTO_RICHIEDENTE", SqlFieldNameExt="RI_ID_ISTITUTO_RICHIEDENTE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? RiIdIstitutoRichiedente  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? RiIdIstitutoRichiedenteObj  { get; set; }

[Display(Name = "Id Operatore Richiedente", ShortName="", Description = "Codice (se disponibile) dell'agente che ha effettivamente inserito la comunicazione", Prompt="")]
[ErpDogField("RI_ID_OPERATORE_RICHIEDENTE", SqlFieldNameExt="RI_ID_OPERATORE_RICHIEDENTE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? RiIdOperatoreRichiedente  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? RiIdOperatoreRichiedenteObj  { get; set; }

[Display(Name = "Data Richiesta", ShortName="", Description = "Data non prima della quale la comunicazione deve essere trasmessa / Data di completamento quando eseguita", Prompt="")]
[ErpDogField("RI_DATA_RICHIESTA", SqlFieldNameExt="RI_DATA_RICHIESTA", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateOnly? RiDataRichiesta  { get; set; }

[Display(Name = "Ora Richiesta", ShortName="", Description = "Ora non prima della quale la comunicazione deve essere trasmessa / Ora di completamento quando eseguita", Prompt="")]
[ErpDogField("RI_ORA_RICHIESTA", SqlFieldNameExt="RI_ORA_RICHIESTA", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? RiOraRichiesta  { get; set; }

[Display(Name = "Urgenza", ShortName="", Description = "Livello di urgenza da 1 a 5 [1: il più alto]", Prompt="")]
[ErpDogField("RI_URGENZA", SqlFieldNameExt="RI_URGENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? RiUrgenza  { get; set; }

[Display(Name = "Oggetto", ShortName="", Description = "Oggetto della comunicazione", Prompt="")]
[ErpDogField("RI_OGGETTO", SqlFieldNameExt="RI_OGGETTO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? RiOggetto  { get; set; }

[Display(Name = "Stato Richiesta", ShortName="", Description = "Stato della comunicazione: In attesa / Sospesa / Completata (o annullata) / X: trasmessa solo a alcuni indirizzi", Prompt="")]
[ErpDogField("RI_STATO_RICHIESTA", SqlFieldNameExt="RI_STATO_RICHIESTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("P")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "P", "C", "X", "H", "A" }, MaxSelections=1, LabelClassName="")]
public string? RiStatoRichiesta  { get; set; }

[Display(Name = "Classe Richiesta", ShortName="", Description = "Classe della comunicazione: Da 0 a 9 riservata al sistema A a Z riservata agli utenti", Prompt="")]
[ErpDogField("RI_CLASSE_RICHIESTA", SqlFieldNameExt="RI_CLASSE_RICHIESTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_RICHIESTA.TI_GRUPPO[RICHIESTA.RI_ID_TIPO_RICHIESTA]) multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "Z" }, MaxSelections=1, LabelClassName="")]
public string? RiClasseRichiesta  { get; set; }

[Display(Name = "Id Tipo Richiesta", ShortName="", Description = "Codice del tipo specifico di comunicazione", Prompt="")]
[ErpDogField("RI_ID_TIPO_RICHIESTA", SqlFieldNameExt="RI_ID_TIPO_RICHIESTA", SqlFieldOptions="", Xref="Ti1Icode", SqlFieldProperties="prop() xref(TIPO_RICHIESTA.TI__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteClient("TipoRichiesta", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? RiIdTipoRichiesta  { get; set; }
public ErpToolkit.Models.SIO.Common.TipoRichiesta? RiIdTipoRichiestaObj  { get; set; }

[Display(Name = "Id Paziente", ShortName="", Description = "Codice del paziente principale a cui si riferisce la comunicazione (se presente)", Prompt="")]
[ErpDogField("RI_ID_PAZIENTE", SqlFieldNameExt="RI_ID_PAZIENTE", SqlFieldOptions="", Xref="Pa1Icode", SqlFieldProperties="prop() xref(PAZIENTE.PA__ICODE) xdup() multbxref()")]
[AutocompleteServer("Paziente", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? RiIdPaziente  { get; set; }
public ErpToolkit.Models.SIO.Patient.Paziente? RiIdPazienteObj  { get; set; }

[Display(Name = "Id Episodio", ShortName="", Description = "Codice del contatto del paziente principale a cui si riferisce la comunicazione (se presente)", Prompt="")]
[ErpDogField("RI_ID_EPISODIO", SqlFieldNameExt="RI_ID_EPISODIO", SqlFieldOptions="", Xref="Ep1Icode", SqlFieldProperties="prop() xref(EPISODIO.EP__ICODE) xdup() multbxref()")]
[AutocompleteServer("Episodio", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? RiIdEpisodio  { get; set; }
public ErpToolkit.Models.SIO.Patient.Episodio? RiIdEpisodioObj  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioRi1Icode|K|Ri1Icode","sioRi1RecDate|N|Ri1Mdate,Ri1Cdate"
        ,"sioRiDataRichiesta|N|RiDataRichiesta"
        ,"sioRiIdOperatoreRichiedente|N|RiIdOperatoreRichiedente"
        ,"sioRiIdTipoRichiestaRiStatoRichiesta|N|RiIdTipoRichiesta,RiStatoRichiesta"
        ,"sioRiIdEpisodio|N|RiIdEpisodio"
        ,"sioRiIdPaziente|N|RiIdPaziente"
        ,"sioRiIdIstitutoRichiedente|N|RiIdIstitutoRichiedente"
        ,"sioRiIdPostazioneRichiedente|N|RiIdPostazioneRichiedente"
        ,"sioRiIdUnitaRichiedente|N|RiIdUnitaRichiedente"
    };
}
}
}
