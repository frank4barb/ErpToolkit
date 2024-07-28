using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class Campione {
public const string Description = "Campione effettivo raccolto durante le attività quotidiane ... intcode:[101] prefix:[CP_] has_xdt:[CP_XDATA] is_xdt:[0] ";
public const string SqlTableName = "CAMPIONE";
public const string SqlTableNameExt = "CAMPIONE";
public const string SqlRowIdName = "CP__ID";
public const string SqlRowIdNameExt = "CP__ICODE";
public const string SqlPrefix = "CP_";
public const string SqlPrefixExt = "CP_";
public const string SqlXdataTableName = "CP_XDATA";
public const string SqlXdataTableNameExt = "CP_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 101; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Cp"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//119-119//REL_PRESTAZIONE_CAMPIONE.PC_ID_CAMPIONE
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneCampione> RelPrestazioneCampione4PcIdCampione  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneCampione>();
[Display(Name = "Cp1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("CP__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Cp1Ienv { get; set; }
[Key]
[Display(Name = "Cp1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("CP__ICODE", SqlFieldNameExt="CP__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Cp1Icode { get; set; }
[Display(Name = "Cp1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("CP__DELETED", SqlFieldNameExt="CP__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Cp1Deleted { get; set; }
[Display(Name = "Cp1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("CP__TIMESTAMP", SqlFieldNameExt="CP__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Cp1Timestamp { get; set; }
[Display(Name = "Cp1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("CP__HOME", SqlFieldNameExt="CP__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Cp1Home { get; set; }
[Display(Name = "Cp1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("CP__VERSION", SqlFieldNameExt="CP__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Cp1Version { get; set; }
[Display(Name = "Cp1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("CP__INACTIVE", SqlFieldNameExt="CP__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Cp1Inactive { get; set; }
[Display(Name = "Cp1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("CP__EXTATT", SqlFieldNameExt="CP__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Cp1Extatt { get; set; }


[Display(Name = "Id Tipo Campione", ShortName="", Description = "Codice del tipo di campione", Prompt="")]
[ErpDogField("CP_ID_TIPO_CAMPIONE", SqlFieldNameExt="CP_ID_TIPO_CAMPIONE", SqlFieldProperties="prop() xref(TIPO_CAMPIONE.TP__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[AutocompleteClient("TipoCampione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? CpIdTipoCampione  { get; set; }
public ErpToolkit.Models.SIO.Act.TipoCampione? CpIdTipoCampioneObj  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione del campione", Prompt="")]
[ErpDogField("CP_DESCRIZIONE", SqlFieldNameExt="CP_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? CpDescrizione  { get; set; }

[Display(Name = "Codice Univoco", ShortName="", Description = "Codice del campione definito dall'utente", Prompt="")]
[ErpDogField("CP_CODICE_UNIVOCO", SqlFieldNameExt="CP_CODICE_UNIVOCO", SqlFieldProperties="prop() xref() xdup(CAMPIONE.CP__ICODE[CP__ICODE] {CP_CODICE_UNIVOCO=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(15, ErrorMessage = "Inserire massimo 15 caratteri")]
[DataType(DataType.Text)]
public string? CpCodiceUnivoco  { get; set; }

[Display(Name = "Data Prelievo", ShortName="", Description = "Data di raccolta", Prompt="")]
[ErpDogField("CP_DATA_PRELIEVO", SqlFieldNameExt="CP_DATA_PRELIEVO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public string? CpDataPrelievo  { get; set; }

[Display(Name = "Ora Prelievo", ShortName="", Description = "Ora di raccolta", Prompt="")]
[ErpDogField("CP_ORA_PRELIEVO", SqlFieldNameExt="CP_ORA_PRELIEVO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? CpOraPrelievo  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("CP_NOTE", SqlFieldNameExt="CP_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? CpNote  { get; set; }

[Display(Name = "Id Episodio", ShortName="", Description = "Codice del contatto", Prompt="")]
[ErpDogField("CP_ID_EPISODIO", SqlFieldNameExt="CP_ID_EPISODIO", SqlFieldProperties="prop() xref(EPISODIO.EP__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[AutocompleteServer("Episodio", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? CpIdEpisodio  { get; set; }
public ErpToolkit.Models.SIO.Patient.Episodio? CpIdEpisodioObj  { get; set; }

[Display(Name = "Id Paziente", ShortName="", Description = "Codice del paziente", Prompt="")]
[ErpDogField("CP_ID_PAZIENTE", SqlFieldNameExt="CP_ID_PAZIENTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? CpIdPaziente  { get; set; }

[Display(Name = "Codice Assoluto", ShortName="", Description = "Codice esterno del campione (ad esempio, generato da strumenti)", Prompt="")]
[ErpDogField("CP_CODICE_ASSOLUTO", SqlFieldNameExt="CP_CODICE_ASSOLUTO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
[StringLength(36, ErrorMessage = "Inserire massimo 36 caratteri")]
[DataType(DataType.Text)]
public string? CpCodiceAssoluto  { get; set; }

[Display(Name = "Stato Campione", ShortName="", Description = "Stato del campione durante il suo ciclo di vita", Prompt="")]
[ErpDogField("CP_STATO_CAMPIONE", SqlFieldNameExt="CP_STATO_CAMPIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("0")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("0|1|2|3|4|5|6|7|8|9", ErrorMessage = "Inserisci una delle seguenti opzioni: 0|1|2|3|4|5|6|7|8|9")]
public string? CpStatoCampione  { get; set; }

[Display(Name = "Id Posizione Attuale", ShortName="", Description = "Posizione del campione nell'organizzazione", Prompt="")]
[ErpDogField("CP_ID_POSIZIONE_ATTUALE", SqlFieldNameExt="CP_ID_POSIZIONE_ATTUALE", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? CpIdPosizioneAttuale  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? CpIdPosizioneAttualeObj  { get; set; }

[Display(Name = "Desc Posizione Attuale", ShortName="", Description = "Posizione attuale testuale del campione", Prompt="")]
[ErpDogField("CP_DESC_POSIZIONE_ATTUALE", SqlFieldNameExt="CP_DESC_POSIZIONE_ATTUALE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? CpDescPosizioneAttuale  { get; set; }

[Display(Name = "Data Cambiamento Stato", ShortName="", Description = "Data dell'ultima modifica dello stato del campione", Prompt="")]
[ErpDogField("CP_DATA_CAMBIAMENTO_STATO", SqlFieldNameExt="CP_DATA_CAMBIAMENTO_STATO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DataType(DataType.Text)]
public string? CpDataCambiamentoStato  { get; set; }

[Display(Name = "Ora Cambiamento Stato", ShortName="", Description = "Ora dell'ultima modifica dello stato del campione", Prompt="")]
[ErpDogField("CP_ORA_CAMBIAMENTO_STATO", SqlFieldNameExt="CP_ORA_CAMBIAMENTO_STATO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Text)]
public string? CpOraCambiamentoStato  { get; set; }

[Display(Name = "Note Cambiamento Stato", ShortName="", Description = "Note sull'ultima modifica dello stato del campione", Prompt="")]
[ErpDogField("CP_NOTE_CAMBIAMENTO_STATO", SqlFieldNameExt="CP_NOTE_CAMBIAMENTO_STATO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? CpNoteCambiamentoStato  { get; set; }
}
}
