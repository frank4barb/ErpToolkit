using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class SelEpisodio {
public const string Description = "Episodi";
public const string SqlTableName = "EPISODIO";
public const string SqlTableNameExt = "EPISODIO";
public const string SqlRowIdName = "EP__ID";
public const string SqlRowIdNameExt = "EP__ICODE";
public const string SqlPrefix = "EP_";
public const string SqlPrefixExt = "EP_";
public const string SqlXdataTableName = "EP_XDATA";
public const string SqlXdataTableNameExt = "EP_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 53; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Ep"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Cod Episodio", ShortName="", Description = "Identificativo del contatto nell'organizzazione sanitaria", Prompt="")]
[ErpDogField("EP_COD_EPISODIO", SqlFieldNameExt="EP_COD_EPISODIO", SqlFieldProperties="prop() xref() xdup(EPISODIO.EP__ICODE[EP__ICODE] {EP_COD_EPISODIO=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? EpCodEpisodio  { get; set; }

[Display(Name = "Id Paziente", ShortName="", Description = "Codice del paziente a cui si riferisce il contatto", Prompt="")]
[ErpDogField("EP_ID_PAZIENTE", SqlFieldNameExt="EP_ID_PAZIENTE", SqlFieldProperties="prop() xref(PAZIENTE.PA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Paziente", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> EpIdPaziente  { get; set; } = new List<string>();

[Display(Name = "Sesso", ShortName="", Description = "Sesso del paziente al momento dell'ammissione", Prompt="")]
[ErpDogField("EP_SESSO", SqlFieldNameExt="EP_SESSO", SqlFieldProperties="prop() xref() xdup(PAZIENTE.PA_SESSO[EPISODIO.EP_ID_PAZIENTE] {EP_SESSO=' '}) multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("M|F|N", ErrorMessage = "Inserisci una delle seguenti opzioni: M|F|N")]
public string? EpSesso  { get; set; }

[Display(Name = "Classe Episodio", ShortName="", Description = "Classe di contatto 1=Permanenza 2=Day-hospital 3=Ambulatoriale 4-=definito dall'utente", Prompt="")]
[ErpDogField("EP_CLASSE_EPISODIO", SqlFieldNameExt="EP_CLASSE_EPISODIO", SqlFieldProperties="prop() xref() xdup(TIPO_EPISODIO.TE_CLASSE[EPISODIO.EP_ID_TIPO_EPISODIO]) multbxref()")]
[DefaultValue("1")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("1|2|3|4|5|6|7|8|9|0", ErrorMessage = "Inserisci una delle seguenti opzioni: 1|2|3|4|5|6|7|8|9|0")]
public string? EpClasseEpisodio  { get; set; }

[Display(Name = "Id Tipo Episodio", ShortName="", Description = "Codice del tipo di contatto", Prompt="")]
[ErpDogField("EP_ID_TIPO_EPISODIO", SqlFieldNameExt="EP_ID_TIPO_EPISODIO", SqlFieldProperties="prop() xref(TIPO_EPISODIO.TE__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoEpisodio", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> EpIdTipoEpisodio  { get; set; } = new List<string>();

[Display(Name = "Stato Episodio", ShortName="", Description = "Stato del contatto F[oreseen] - A[ctual, in progress] - C[ompleted] - D[eleted] - S[uspended]", Prompt="")]
[ErpDogField("EP_STATO_EPISODIO", SqlFieldNameExt="EP_STATO_EPISODIO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("F|A|C|D|S", ErrorMessage = "Inserisci una delle seguenti opzioni: F|A|C|D|S")]
public string? EpStatoEpisodio  { get; set; }

[Display(Name = "Id Unita Ingresso", ShortName="", Description = "Identificativo dell'unità di assistenza che ha avviato il contatto", Prompt="")]
[ErpDogField("EP_ID_UNITA_INGRESSO", SqlFieldNameExt="EP_ID_UNITA_INGRESSO", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> EpIdUnitaIngresso  { get; set; } = new List<string>();

[Display(Name = "Data Inizio", ShortName="", Description = "Data di inizio del periodo di permanenza del contatto", Prompt="")]
[ErpDogField("EP_DATA_INIZIO", SqlFieldNameExt="EP_DATA_INIZIO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange EpDataInizio  { get; set; } = new DateRange();

[Display(Name = "Ora Inizio", ShortName="", Description = "Ora di inizio del periodo di permanenza del contatto", Prompt="")]
[ErpDogField("EP_ORA_INIZIO", SqlFieldNameExt="EP_ORA_INIZIO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? EpOraInizio  { get; set; }

[Display(Name = "Data Fine", ShortName="", Description = "Data di fine del periodo di permanenza del contatto", Prompt="")]
[ErpDogField("EP_DATA_FINE", SqlFieldNameExt="EP_DATA_FINE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange EpDataFine  { get; set; } = new DateRange();

[Display(Name = "Ora Fine", ShortName="", Description = "Ora di fine del periodo di permanenza del contatto", Prompt="")]
[ErpDogField("EP_ORA_FINE", SqlFieldNameExt="EP_ORA_FINE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? EpOraFine  { get; set; }

[Display(Name = "Cartella Ps", ShortName="", Description = "Identificativo del documento correlato dell'organizzazione di origine", Prompt="")]
[ErpDogField("EP_CARTELLA_PS", SqlFieldNameExt="EP_CARTELLA_PS", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? EpCartellaPs  { get; set; }

[Display(Name = "Id Corsia", ShortName="", Description = "Codice dell'ultima (o attuale) unità in cui è ubicato il paziente", Prompt="")]
[ErpDogField("EP_ID_CORSIA", SqlFieldNameExt="EP_ID_CORSIA", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> EpIdCorsia  { get; set; } = new List<string>();

[Display(Name = "Id Reparto", ShortName="", Description = "Codice dell'unità responsabile del paziente", Prompt="")]
[ErpDogField("EP_ID_REPARTO", SqlFieldNameExt="EP_ID_REPARTO", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup(EPISODIO.EP_ID_CORSIA[EP__ICODE] {EP_ID_REPARTO=' '}) multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> EpIdReparto  { get; set; } = new List<string>();

[Display(Name = "Letto", ShortName="", Description = "Letto assegnato al paziente", Prompt="")]
[ErpDogField("EP_LETTO", SqlFieldNameExt="EP_LETTO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? EpLetto  { get; set; }

[Display(Name = "Stanza", ShortName="", Description = "Stanza e altre strutture logistiche correlate al contatto", Prompt="")]
[ErpDogField("EP_STANZA", SqlFieldNameExt="EP_STANZA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? EpStanza  { get; set; }

[Display(Name = "Id Diagnosi Ammissione", ShortName="", Description = "Codice della diagnosi di ammissione", Prompt="")]
[ErpDogField("EP_ID_DIAGNOSI_AMMISSIONE", SqlFieldNameExt="EP_ID_DIAGNOSI_AMMISSIONE", SqlFieldProperties="prop() xref(DIAGNOSI.DG__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Diagnosi", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> EpIdDiagnosiAmmissione  { get; set; } = new List<string>();

[Display(Name = "Id Diagnosi Dimissione", ShortName="", Description = "Codice della diagnosi di dimissione", Prompt="")]
[ErpDogField("EP_ID_DIAGNOSI_DIMISSIONE", SqlFieldNameExt="EP_ID_DIAGNOSI_DIMISSIONE", SqlFieldProperties="prop() xref(DIAGNOSI.DG__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Diagnosi", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> EpIdDiagnosiDimissione  { get; set; } = new List<string>();

[Display(Name = "Note", ShortName="", Description = "Note aggiuntive generiche", Prompt="")]
[ErpDogField("EP_NOTE", SqlFieldNameExt="EP_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? EpNote  { get; set; }

[Display(Name = "Id Atto Amministrativo", ShortName="", Description = "Identificativo dell'atto che descrive gli aspetti organizzativi attuali del contatto", Prompt="")]
[ErpDogField("EP_ID_ATTO_AMMINISTRATIVO", SqlFieldNameExt="EP_ID_ATTO_AMMINISTRATIVO", SqlFieldProperties="prop() xref(PRESTAZIONE.PR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Prestazione", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> EpIdAttoAmministrativo  { get; set; } = new List<string>();

[Display(Name = "Data Inizio La", ShortName="", Description = "Data di inizio del periodo di lista d'attesa del contatto", Prompt="")]
[ErpDogField("EP_DATA_INIZIO_LA", SqlFieldNameExt="EP_DATA_INIZIO_LA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange EpDataInizioLa  { get; set; } = new DateRange();

[Display(Name = "Ora Inizio La", ShortName="", Description = "Ora di inizio del periodo di lista d'attesa del contatto", Prompt="")]
[ErpDogField("EP_ORA_INIZIO_LA", SqlFieldNameExt="EP_ORA_INIZIO_LA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? EpOraInizioLa  { get; set; }

[Display(Name = "Id Reparto La", ShortName="", Description = "Identificativo dell'unità di assistenza responsabile del periodo di lista d'attesa", Prompt="")]
[ErpDogField("EP_ID_REPARTO_LA", SqlFieldNameExt="EP_ID_REPARTO_LA", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> EpIdRepartoLa  { get; set; } = new List<string>();

[Display(Name = "Data Inizio Preh", ShortName="", Description = "Data di inizio del periodo di preospedalizzazione del contatto", Prompt="")]
[ErpDogField("EP_DATA_INIZIO_PREH", SqlFieldNameExt="EP_DATA_INIZIO_PREH", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange EpDataInizioPreh  { get; set; } = new DateRange();

[Display(Name = "Ora Inizio Preh", ShortName="", Description = "Ora di inizio del periodo di preospedalizzazione del contatto", Prompt="")]
[ErpDogField("EP_ORA_INIZIO_PREH", SqlFieldNameExt="EP_ORA_INIZIO_PREH", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? EpOraInizioPreh  { get; set; }

[Display(Name = "Id Reparto Preh", ShortName="", Description = "Identificativo dell'unità di assistenza responsabile del periodo di preospedalizzazione", Prompt="")]
[ErpDogField("EP_ID_REPARTO_PREH", SqlFieldNameExt="EP_ID_REPARTO_PREH", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> EpIdRepartoPreh  { get; set; } = new List<string>();

[Display(Name = "Fase Episodio", ShortName="", Description = "Codice del tipo attuale (ultimo) fase del contatto (ad es. Lista d'attesa, Preospedalizzazione, In-staying, Home-care, Sospeso)", Prompt="")]
[ErpDogField("EP_FASE_EPISODIO", SqlFieldNameExt="EP_FASE_EPISODIO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? EpFaseEpisodio  { get; set; }
}
}
