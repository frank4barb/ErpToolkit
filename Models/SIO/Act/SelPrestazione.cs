using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class SelPrestazione {
public const string Description = "Prestazione effettuata: dettaglio delle attività effettivamente eseguite durante il lavoro quotidiano nell'organizzazione";
public const string SqlTableName = "PRESTAZIONE";
public const string SqlTableNameExt = "PRESTAZIONE";
public const string SqlRowIdName = "PR__ID";
public const string SqlRowIdNameExt = "PR__ICODE";
public const string SqlPrefix = "PR_";
public const string SqlPrefixExt = "PR_";
public const string SqlXdataTableName = "PR_XDATA";
public const string SqlXdataTableNameExt = "PR_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 31; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Pr"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//120-119//REL_PRESTAZIONE_CAMPIONE.PC_ID_PRESTAZIONE
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneCampione> RelPrestazioneCampione4PcIdPrestazione  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneCampione>();
//124-124//REL_PRESTAZIONE_USA.PU_ID_PRESTAZIONE
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa> RelPrestazioneUsa4PuIdPrestazione  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa>();
//1027-1025//REL_PRESTAZIONE_DATO_CLINICO.PD_ID_PRESTAZIONE
public List<ErpToolkit.Models.SIO.HealthData.RelPrestazioneDatoClinico> RelPrestazioneDatoClinico4PdIdPrestazione  { get; set; } = new List<ErpToolkit.Models.SIO.HealthData.RelPrestazioneDatoClinico>();

[Display(Name = "Id Tipo Attivita", ShortName="", Description = "Codice della classe generale di attività", Prompt="")]
[ErpDogField("PR_ID_TIPO_ATTIVITA", SqlFieldNameExt="PR_ID_TIPO_ATTIVITA", SqlFieldOptions="", SqlFieldProperties="prop() xref(TIPO_ATTIVITA.TA__ICODE) xdup(ATTIVITA.AV_ID_TIPO_ATTIVITA[PRESTAZIONE.PR_ID_ATTIVITA_ESEGUITA]) multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoAttivita", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PrIdTipoAttivita  { get; set; } = new List<string>();

[Display(Name = "Id Attivita Richiesta", ShortName="", Description = "Codice del tipo di attività richiesto dal richiedente", Prompt="")]
[ErpDogField("PR_ID_ATTIVITA_RICHIESTA", SqlFieldNameExt="PR_ID_ATTIVITA_RICHIESTA", SqlFieldOptions="", SqlFieldProperties="prop() xref(ATTIVITA.AV__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Attivita", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PrIdAttivitaRichiesta  { get; set; } = new List<string>();

[Display(Name = "Id Attivita Eseguita", ShortName="", Description = "Codice del tipo di attività effettivamente eseguito", Prompt="")]
[ErpDogField("PR_ID_ATTIVITA_ESEGUITA", SqlFieldNameExt="PR_ID_ATTIVITA_ESEGUITA", SqlFieldOptions="", SqlFieldProperties="prop() xref(ATTIVITA.AV__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Attivita", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PrIdAttivitaEseguita  { get; set; } = new List<string>();

[Display(Name = "Id Gruppo", ShortName="", Description = "Codice dell'atto di cui questo è una sotto-attività", Prompt="")]
[ErpDogField("PR_ID_GRUPPO", SqlFieldNameExt="PR_ID_GRUPPO", SqlFieldOptions="", SqlFieldProperties="prop() xref(PRESTAZIONE.PR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Prestazione", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> PrIdGruppo  { get; set; } = new List<string>();

[Display(Name = "In Evidenza", ShortName="", Description = "Evidenzia questo atto per scopi di ricerca o speciali Sì [Y] - No [N]", Prompt="")]
[ErpDogField("PR_IN_EVIDENZA", SqlFieldNameExt="PR_IN_EVIDENZA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PrInEvidenza  { get; set; }

[Display(Name = "Id Paziente", ShortName="", Description = "Codice del paziente", Prompt="")]
[ErpDogField("PR_ID_PAZIENTE", SqlFieldNameExt="PR_ID_PAZIENTE", SqlFieldOptions="", SqlFieldProperties="prop() xref(PAZIENTE.PA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Paziente", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> PrIdPaziente  { get; set; } = new List<string>();

[Display(Name = "Id Episodio", ShortName="", Description = "Codice del contatto (codice in-paziente o ambulatoriale)", Prompt="")]
[ErpDogField("PR_ID_EPISODIO", SqlFieldNameExt="PR_ID_EPISODIO", SqlFieldOptions="", SqlFieldProperties="prop() xref(EPISODIO.EP__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Episodio", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> PrIdEpisodio  { get; set; } = new List<string>();

[Display(Name = "Classe Episodio", ShortName="", Description = "Classe di contatto 1=Ricovero - 2=Day-Hospital - 3=Ambulatorio", Prompt="")]
[ErpDogField("PR_CLASSE_EPISODIO", SqlFieldNameExt="PR_CLASSE_EPISODIO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup(EPISODIO.EP_CLASSE_EPISODIO[PRESTAZIONE.PR_ID_EPISODIO]) multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("1|2|3", ErrorMessage = "Inserisci una delle seguenti opzioni: 1|2|3")]
public string? PrClasseEpisodio  { get; set; }

[Display(Name = "Tipo Episodio", ShortName="", Description = "Codice del tipo di contatto", Prompt="")]
[ErpDogField("PR_TIPO_EPISODIO", SqlFieldNameExt="PR_TIPO_EPISODIO", SqlFieldOptions="", SqlFieldProperties="prop() xref(TIPO_EPISODIO.TE__ICODE) xdup(EPISODIO.EP_ID_TIPO_EPISODIO[PRESTAZIONE.PR_ID_EPISODIO]) multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoEpisodio", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PrTipoEpisodio  { get; set; } = new List<string>();

[Display(Name = "Stato Prestazione", ShortName="", Description = "Stato dell'atto", Prompt="")]
[ErpDogField("PR_STATO_PRESTAZIONE", SqlFieldNameExt="PR_STATO_PRESTAZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("0|1|2|3|4|5|6|7|8|9|A|B|C|D|E|F", ErrorMessage = "Inserisci una delle seguenti opzioni: 0|1|2|3|4|5|6|7|8|9|A|B|C|D|E|F")]
public string? PrStatoPrestazione  { get; set; }

[Display(Name = "Id Operatore Richiedente", ShortName="", Description = "Codice dell'agente che ha richiesto l'atto", Prompt="")]
[ErpDogField("PR_ID_OPERATORE_RICHIEDENTE", SqlFieldNameExt="PR_ID_OPERATORE_RICHIEDENTE", SqlFieldOptions="", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PrIdOperatoreRichiedente  { get; set; } = new List<string>();

[Display(Name = "Id Unita Richiedente", ShortName="", Description = "Codice dell'unità che ha richiesto l'atto", Prompt="")]
[ErpDogField("PR_ID_UNITA_RICHIEDENTE", SqlFieldNameExt="PR_ID_UNITA_RICHIEDENTE", SqlFieldOptions="", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PrIdUnitaRichiedente  { get; set; } = new List<string>();

[Display(Name = "Id Postazione Richiedente", ShortName="", Description = "Codice del punto di servizio che ha richiesto l'atto", Prompt="")]
[ErpDogField("PR_ID_POSTAZIONE_RICHIEDENTE", SqlFieldNameExt="PR_ID_POSTAZIONE_RICHIEDENTE", SqlFieldOptions="", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PrIdPostazioneRichiedente  { get; set; } = new List<string>();

[Display(Name = "Data Richiesta", ShortName="", Description = "Data della richiesta", Prompt="")]
[ErpDogField("PR_DATA_RICHIESTA", SqlFieldNameExt="PR_DATA_RICHIESTA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange PrDataRichiesta  { get; set; } = new DateRange();

[Display(Name = "Ora Richiesta", ShortName="", Description = "Ora della richiesta", Prompt="")]
[ErpDogField("PR_ORA_RICHIESTA", SqlFieldNameExt="PR_ORA_RICHIESTA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? PrOraRichiesta  { get; set; }

[Display(Name = "Id Richiesta", ShortName="", Description = "Codice della comunicazione di richiesta (se presente)", Prompt="")]
[ErpDogField("PR_ID_RICHIESTA", SqlFieldNameExt="PR_ID_RICHIESTA", SqlFieldOptions="", SqlFieldProperties="prop() xref(RICHIESTA.RI__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Richiesta", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> PrIdRichiesta  { get; set; } = new List<string>();

[Display(Name = "Data Proposta Esecuzione", ShortName="", Description = "Data inizialmente richiesta per l'esecuzione", Prompt="")]
[ErpDogField("PR_DATA_PROPOSTA_ESECUZIONE", SqlFieldNameExt="PR_DATA_PROPOSTA_ESECUZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange PrDataPropostaEsecuzione  { get; set; } = new DateRange();

[Display(Name = "Ora Proposta Esecuzione", ShortName="", Description = "Ora inizialmente richiesta per l'esecuzione", Prompt="")]
[ErpDogField("PR_ORA_PROPOSTA_ESECUZIONE", SqlFieldNameExt="PR_ORA_PROPOSTA_ESECUZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? PrOraPropostaEsecuzione  { get; set; }

[Display(Name = "Durata Prevista", ShortName="", Description = "Durata prevista dell'atto (peso)", Prompt="")]
[ErpDogField("PR_DURATA_PREVISTA", SqlFieldNameExt="PR_DURATA_PREVISTA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? PrDurataPrevista  { get; set; }

[Display(Name = "Urgenza", ShortName="", Description = "Modalità adottata per l'inserimento/esecuzione dell'atto", Prompt="")]
[ErpDogField("PR_URGENZA", SqlFieldNameExt="PR_URGENZA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PrUrgenza  { get; set; }

[Display(Name = "Note Richiesta", ShortName="", Description = "Commenti allegati alla richiesta", Prompt="")]
[ErpDogField("PR_NOTE_RICHIESTA", SqlFieldNameExt="PR_NOTE_RICHIESTA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PrNoteRichiesta  { get; set; }

[Display(Name = "Routine", ShortName="", Description = "Pianificazione routinaria dell'atto Sì [Y] / No [N]", Prompt="")]
[ErpDogField("PR_ROUTINE", SqlFieldNameExt="PR_ROUTINE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PrRoutine  { get; set; }

[Display(Name = "Data Appuntamento", ShortName="", Description = "Data attualmente programmata per l'esecuzione", Prompt="")]
[ErpDogField("PR_DATA_APPUNTAMENTO", SqlFieldNameExt="PR_DATA_APPUNTAMENTO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange PrDataAppuntamento  { get; set; } = new DateRange();

[Display(Name = "Ora Appuntamento", ShortName="", Description = "Ora attualmente programmata per l'esecuzione", Prompt="")]
[ErpDogField("PR_ORA_APPUNTAMENTO", SqlFieldNameExt="PR_ORA_APPUNTAMENTO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? PrOraAppuntamento  { get; set; }

[Display(Name = "Note Pianificazione", ShortName="", Description = "Commento allegato alla pianificazione", Prompt="")]
[ErpDogField("PR_NOTE_PIANIFICAZIONE", SqlFieldNameExt="PR_NOTE_PIANIFICAZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PrNotePianificazione  { get; set; }

[Display(Name = "Data Inizio Esecuzione", ShortName="", Description = "Data di inizio dell'esecuzione", Prompt="")]
[ErpDogField("PR_DATA_INIZIO_ESECUZIONE", SqlFieldNameExt="PR_DATA_INIZIO_ESECUZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange PrDataInizioEsecuzione  { get; set; } = new DateRange();

[Display(Name = "Ora Inizio Esecuzione", ShortName="", Description = "Ora di inizio dell'esecuzione", Prompt="")]
[ErpDogField("PR_ORA_INIZIO_ESECUZIONE", SqlFieldNameExt="PR_ORA_INIZIO_ESECUZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? PrOraInizioEsecuzione  { get; set; }

[Display(Name = "Data Fine Esecuzione", ShortName="", Description = "Data di completamento dell'esecuzione o annullamento (se appropriato)", Prompt="")]
[ErpDogField("PR_DATA_FINE_ESECUZIONE", SqlFieldNameExt="PR_DATA_FINE_ESECUZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange PrDataFineEsecuzione  { get; set; } = new DateRange();

[Display(Name = "Ora Fine Esecuzione", ShortName="", Description = "Ora di completamento dell'esecuzione o annullamento (se appropriato)", Prompt="")]
[ErpDogField("PR_ORA_FINE_ESECUZIONE", SqlFieldNameExt="PR_ORA_FINE_ESECUZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? PrOraFineEsecuzione  { get; set; }

[Display(Name = "Data Refertazione", ShortName="", Description = "Data di refertazione finale", Prompt="")]
[ErpDogField("PR_DATA_REFERTAZIONE", SqlFieldNameExt="PR_DATA_REFERTAZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange PrDataRefertazione  { get; set; } = new DateRange();

[Display(Name = "Ora Refertazione", ShortName="", Description = "Ora di refertazione finale", Prompt="")]
[ErpDogField("PR_ORA_REFERTAZIONE", SqlFieldNameExt="PR_ORA_REFERTAZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? PrOraRefertazione  { get; set; }

[Display(Name = "Id Operatore Esecutore", ShortName="", Description = "Codice dell'agente che ha annullato o eseguito l'atto (se appropriato)", Prompt="")]
[ErpDogField("PR_ID_OPERATORE_ESECUTORE", SqlFieldNameExt="PR_ID_OPERATORE_ESECUTORE", SqlFieldOptions="", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PrIdOperatoreEsecutore  { get; set; } = new List<string>();

[Display(Name = "Id Unita Esecutrice", ShortName="", Description = "Codice dell'unità che ha eseguito/annullato l'atto", Prompt="")]
[ErpDogField("PR_ID_UNITA_ESECUTRICE", SqlFieldNameExt="PR_ID_UNITA_ESECUTRICE", SqlFieldOptions="", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PrIdUnitaEsecutrice  { get; set; } = new List<string>();

[Display(Name = "Id Postazione Esecutrice", ShortName="", Description = "Codice del punto di servizio che ha eseguito/annullato l'atto", Prompt="")]
[ErpDogField("PR_ID_POSTAZIONE_ESECUTRICE", SqlFieldNameExt="PR_ID_POSTAZIONE_ESECUTRICE", SqlFieldOptions="", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PrIdPostazioneEsecutrice  { get; set; } = new List<string>();

[Display(Name = "Durata Effettiva", ShortName="", Description = "Durata effettiva dell'atto (peso)", Prompt="")]
[ErpDogField("PR_DURATA_EFFETTIVA", SqlFieldNameExt="PR_DURATA_EFFETTIVA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? PrDurataEffettiva  { get; set; }

[Display(Name = "Costo Prestazione", ShortName="", Description = "Costo effettivo", Prompt="")]
[ErpDogField("PR_COSTO_PRESTAZIONE", SqlFieldNameExt="PR_COSTO_PRESTAZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? PrCostoPrestazione  { get; set; }

[Display(Name = "Note Esecuzione", ShortName="", Description = "Breve nota relativa all'esecuzione, all'annullamento, alla sospensione, ecc. dell'atto", Prompt="")]
[ErpDogField("PR_NOTE_ESECUZIONE", SqlFieldNameExt="PR_NOTE_ESECUZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PrNoteEsecuzione  { get; set; }

[Display(Name = "Id Operatore Pianificatore", ShortName="", Description = "Codice dell'individuo che ha pianificato l'atto", Prompt="")]
[ErpDogField("PR_ID_OPERATORE_PIANIFICATORE", SqlFieldNameExt="PR_ID_OPERATORE_PIANIFICATORE", SqlFieldOptions="", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PrIdOperatorePianificatore  { get; set; } = new List<string>();

[Display(Name = "Data Pianificazione", ShortName="", Description = "Data in cui l'atto è stato pianificato", Prompt="")]
[ErpDogField("PR_DATA_PIANIFICAZIONE", SqlFieldNameExt="PR_DATA_PIANIFICAZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange PrDataPianificazione  { get; set; } = new DateRange();

[Display(Name = "Ora Pianificazione", ShortName="", Description = "Ora in cui l'atto è stato pianificato", Prompt="")]
[ErpDogField("PR_ORA_PIANIFICAZIONE", SqlFieldNameExt="PR_ORA_PIANIFICAZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? PrOraPianificazione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note testuali generiche relative all'atto", Prompt="")]
[ErpDogField("PR_NOTE", SqlFieldNameExt="PR_NOTE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PrNote  { get; set; }

[Display(Name = "Mobilita", ShortName="", Description = "Mobilità del paziente (quando applicabile) vuoto: non applicabile - 1: autonomo - 2: sedia a rotelle - 3: a letto - 4: non mobile", Prompt="")]
[ErpDogField("PR_MOBILITA", SqlFieldNameExt="PR_MOBILITA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PrMobilita  { get; set; }
}
}
