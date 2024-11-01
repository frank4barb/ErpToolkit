using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class Prestazione : ModelErp {
public const string Description = "Prestazione effettuata: dettaglio delle attività effettivamente eseguite durante il lavoro quotidiano nell'organizzazione";
public const string SqlTableName = "PRESTAZIONE";
public const string SqlTableNameExt = "PRESTAZIONE";
public const string SqlRowIdName = "PR__ID";
public const string SqlRowIdNameExt = "PR__ICODE";
public const string SqlPrefix = "PR_";
public const string SqlPrefixExt = "PR_";
public const string SqlXdataTableName = "PR_XDATA";
public const string SqlXdataTableNameExt = "PR_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
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
[Display(Name = "Pr1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("PR__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Pr1Ienv { get; set; }
[Key]
[Display(Name = "Pr1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("PR__ICODE", SqlFieldNameExt="PR__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pr1Icode { get; set; }
[Display(Name = "Pr1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("PR__DELETED", SqlFieldNameExt="PR__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Pr1Deleted { get; set; }
[Display(Name = "Pr1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("PR__TIMESTAMP", SqlFieldNameExt="PR__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
//[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Pr1Timestamp { get; set; }
[Display(Name = "Pr1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("PR__HOME", SqlFieldNameExt="PR__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pr1Home { get; set; }
[Display(Name = "Pr1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("PR__VERSION", SqlFieldNameExt="PR__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pr1Version { get; set; }
[Display(Name = "Pr1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("PR__INACTIVE", SqlFieldNameExt="PR__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Pr1Inactive { get; set; }
[Display(Name = "Pr1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("PR__EXTATT", SqlFieldNameExt="PR__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Pr1Extatt { get; set; }


[Display(Name = "Id Tipo Attivita", ShortName="", Description = "Codice della classe generale di attività", Prompt="")]
[ErpDogField("PR_ID_TIPO_ATTIVITA", SqlFieldNameExt="PR_ID_TIPO_ATTIVITA", SqlFieldOptions="", Xref="Ta1Icode", SqlFieldProperties="prop() xref(TIPO_ATTIVITA.TA__ICODE) xdup(ATTIVITA.AV_ID_TIPO_ATTIVITA[PRESTAZIONE.PR_ID_ATTIVITA_ESEGUITA]) multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteClient("TipoAttivita", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PrIdTipoAttivita  { get; set; }
public ErpToolkit.Models.SIO.Act.TipoAttivita? PrIdTipoAttivitaObj  { get; set; }

[Display(Name = "Id Attivita Richiesta", ShortName="", Description = "Codice del tipo di attività richiesto dal richiedente", Prompt="")]
[ErpDogField("PR_ID_ATTIVITA_RICHIESTA", SqlFieldNameExt="PR_ID_ATTIVITA_RICHIESTA", SqlFieldOptions="", Xref="Av1Icode", SqlFieldProperties="prop() xref(ATTIVITA.AV__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteClient("Attivita", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PrIdAttivitaRichiesta  { get; set; }
public ErpToolkit.Models.SIO.Act.Attivita? PrIdAttivitaRichiestaObj  { get; set; }

[Display(Name = "Id Attivita Eseguita", ShortName="", Description = "Codice del tipo di attività effettivamente eseguito", Prompt="")]
[ErpDogField("PR_ID_ATTIVITA_ESEGUITA", SqlFieldNameExt="PR_ID_ATTIVITA_ESEGUITA", SqlFieldOptions="", Xref="Av1Icode", SqlFieldProperties="prop() xref(ATTIVITA.AV__ICODE) xdup() multbxref()")]
[AutocompleteClient("Attivita", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PrIdAttivitaEseguita  { get; set; }
public ErpToolkit.Models.SIO.Act.Attivita? PrIdAttivitaEseguitaObj  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Codice dell'atto di cui questo è una sotto-attività", Prompt="")]
[ErpDogField("PR_ID_GRUPPO", SqlFieldNameExt="PR_ID_GRUPPO", SqlFieldOptions="", Xref="Pr1Icode", SqlFieldProperties="prop() xref(PRESTAZIONE.PR__ICODE) xdup() multbxref()")]
[AutocompleteServer("Prestazione", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? PrIdGruppo  { get; set; }
public ErpToolkit.Models.SIO.Act.Prestazione? PrIdGruppoObj  { get; set; }

[Display(Name = "In Evidenza", ShortName="", Description = "Evidenzia questo atto per scopi di ricerca o speciali Sì [Y] - No [N]", Prompt="")]
[ErpDogField("PR_IN_EVIDENZA", SqlFieldNameExt="PR_IN_EVIDENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("N")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? PrInEvidenza  { get; set; }

[Display(Name = "Id Paziente", ShortName="", Description = "Codice del paziente", Prompt="")]
[ErpDogField("PR_ID_PAZIENTE", SqlFieldNameExt="PR_ID_PAZIENTE", SqlFieldOptions="", Xref="Pa1Icode", SqlFieldProperties="prop() xref(PAZIENTE.PA__ICODE) xdup() multbxref()")]
[AutocompleteServer("Paziente", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? PrIdPaziente  { get; set; }
public ErpToolkit.Models.SIO.Patient.Paziente? PrIdPazienteObj  { get; set; }

[Display(Name = "Id Episodio", ShortName="", Description = "Codice del contatto (codice in-paziente o ambulatoriale)", Prompt="")]
[ErpDogField("PR_ID_EPISODIO", SqlFieldNameExt="PR_ID_EPISODIO", SqlFieldOptions="", Xref="Ep1Icode", SqlFieldProperties="prop() xref(EPISODIO.EP__ICODE) xdup() multbxref()")]
[AutocompleteServer("Episodio", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? PrIdEpisodio  { get; set; }
public ErpToolkit.Models.SIO.Patient.Episodio? PrIdEpisodioObj  { get; set; }

[Display(Name = "Classe Episodio", ShortName="", Description = "Classe di contatto 1=Ricovero - 2=Day-Hospital - 3=Ambulatorio", Prompt="")]
[ErpDogField("PR_CLASSE_EPISODIO", SqlFieldNameExt="PR_CLASSE_EPISODIO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup(EPISODIO.EP_CLASSE_EPISODIO[PRESTAZIONE.PR_ID_EPISODIO]) multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "1", "2", "3" }, MaxSelections=1, LabelClassName="")]
public string? PrClasseEpisodio  { get; set; }

[Display(Name = "Tipo Episodio", ShortName="", Description = "Codice del tipo di contatto", Prompt="")]
[ErpDogField("PR_TIPO_EPISODIO", SqlFieldNameExt="PR_TIPO_EPISODIO", SqlFieldOptions="", Xref="Te1Icode", SqlFieldProperties="prop() xref(TIPO_EPISODIO.TE__ICODE) xdup(EPISODIO.EP_ID_TIPO_EPISODIO[PRESTAZIONE.PR_ID_EPISODIO]) multbxref()")]
[AutocompleteClient("TipoEpisodio", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PrTipoEpisodio  { get; set; }
public ErpToolkit.Models.SIO.Act.TipoEpisodio? PrTipoEpisodioObj  { get; set; }

[Display(Name = "Stato Prestazione", ShortName="", Description = "Stato dell'atto", Prompt="")]
[ErpDogField("PR_STATO_PRESTAZIONE", SqlFieldNameExt="PR_STATO_PRESTAZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" }, MaxSelections=1, LabelClassName="")]
public string? PrStatoPrestazione  { get; set; }

[Display(Name = "Id Operatore Richiedente", ShortName="", Description = "Codice dell'agente che ha richiesto l'atto", Prompt="")]
[ErpDogField("PR_ID_OPERATORE_RICHIEDENTE", SqlFieldNameExt="PR_ID_OPERATORE_RICHIEDENTE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PrIdOperatoreRichiedente  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? PrIdOperatoreRichiedenteObj  { get; set; }

[Display(Name = "Id Unita Richiedente", ShortName="", Description = "Codice dell'unità che ha richiesto l'atto", Prompt="")]
[ErpDogField("PR_ID_UNITA_RICHIEDENTE", SqlFieldNameExt="PR_ID_UNITA_RICHIEDENTE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PrIdUnitaRichiedente  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? PrIdUnitaRichiedenteObj  { get; set; }

[Display(Name = "Id Postazione Richiedente", ShortName="", Description = "Codice del punto di servizio che ha richiesto l'atto", Prompt="")]
[ErpDogField("PR_ID_POSTAZIONE_RICHIEDENTE", SqlFieldNameExt="PR_ID_POSTAZIONE_RICHIEDENTE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PrIdPostazioneRichiedente  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? PrIdPostazioneRichiedenteObj  { get; set; }

[Display(Name = "Data Richiesta", ShortName="", Description = "Data della richiesta", Prompt="")]
[ErpDogField("PR_DATA_RICHIESTA", SqlFieldNameExt="PR_DATA_RICHIESTA", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateOnly? PrDataRichiesta  { get; set; }

[Display(Name = "Ora Richiesta", ShortName="", Description = "Ora della richiesta", Prompt="")]
[ErpDogField("PR_ORA_RICHIESTA", SqlFieldNameExt="PR_ORA_RICHIESTA", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
public TimeOnly? PrOraRichiesta  { get; set; }

[Display(Name = "Id Richiesta", ShortName="", Description = "Codice della comunicazione di richiesta (se presente)", Prompt="")]
[ErpDogField("PR_ID_RICHIESTA", SqlFieldNameExt="PR_ID_RICHIESTA", SqlFieldOptions="", Xref="Ri1Icode", SqlFieldProperties="prop() xref(RICHIESTA.RI__ICODE) xdup() multbxref()")]
[AutocompleteServer("Richiesta", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? PrIdRichiesta  { get; set; }
public ErpToolkit.Models.SIO.Common.Richiesta? PrIdRichiestaObj  { get; set; }

[Display(Name = "Data Proposta Esecuzione", ShortName="", Description = "Data inizialmente richiesta per l'esecuzione", Prompt="")]
[ErpDogField("PR_DATA_PROPOSTA_ESECUZIONE", SqlFieldNameExt="PR_DATA_PROPOSTA_ESECUZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateOnly? PrDataPropostaEsecuzione  { get; set; }

[Display(Name = "Ora Proposta Esecuzione", ShortName="", Description = "Ora inizialmente richiesta per l'esecuzione", Prompt="")]
[ErpDogField("PR_ORA_PROPOSTA_ESECUZIONE", SqlFieldNameExt="PR_ORA_PROPOSTA_ESECUZIONE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
public TimeOnly? PrOraPropostaEsecuzione  { get; set; }

[Display(Name = "Durata Prevista", ShortName="", Description = "Durata prevista dell'atto (peso)", Prompt="")]
[ErpDogField("PR_DURATA_PREVISTA", SqlFieldNameExt="PR_DURATA_PREVISTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
public short? PrDurataPrevista  { get; set; }

[Display(Name = "Urgenza", ShortName="", Description = "Modalità adottata per l'inserimento/esecuzione dell'atto", Prompt="")]
[ErpDogField("PR_URGENZA", SqlFieldNameExt="PR_URGENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? PrUrgenza  { get; set; }

[Display(Name = "Note Richiesta", ShortName="", Description = "Commenti allegati alla richiesta", Prompt="")]
[ErpDogField("PR_NOTE_RICHIESTA", SqlFieldNameExt="PR_NOTE_RICHIESTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(80, ErrorMessage = "Inserire massimo 80 caratteri")]
[DataType(DataType.Text)]
public string? PrNoteRichiesta  { get; set; }

[Display(Name = "Routine", ShortName="", Description = "Pianificazione routinaria dell'atto Sì [Y] / No [N]", Prompt="")]
[ErpDogField("PR_ROUTINE", SqlFieldNameExt="PR_ROUTINE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("N")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? PrRoutine  { get; set; }

[Display(Name = "Data Appuntamento", ShortName="", Description = "Data attualmente programmata per l'esecuzione", Prompt="")]
[ErpDogField("PR_DATA_APPUNTAMENTO", SqlFieldNameExt="PR_DATA_APPUNTAMENTO", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateOnly? PrDataAppuntamento  { get; set; }

[Display(Name = "Ora Appuntamento", ShortName="", Description = "Ora attualmente programmata per l'esecuzione", Prompt="")]
[ErpDogField("PR_ORA_APPUNTAMENTO", SqlFieldNameExt="PR_ORA_APPUNTAMENTO", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
public TimeOnly? PrOraAppuntamento  { get; set; }

[Display(Name = "Note Pianificazione", ShortName="", Description = "Commento allegato alla pianificazione", Prompt="")]
[ErpDogField("PR_NOTE_PIANIFICAZIONE", SqlFieldNameExt="PR_NOTE_PIANIFICAZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(40, ErrorMessage = "Inserire massimo 40 caratteri")]
[DataType(DataType.Text)]
public string? PrNotePianificazione  { get; set; }

[Display(Name = "Data Inizio Esecuzione", ShortName="", Description = "Data di inizio dell'esecuzione", Prompt="")]
[ErpDogField("PR_DATA_INIZIO_ESECUZIONE", SqlFieldNameExt="PR_DATA_INIZIO_ESECUZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateOnly? PrDataInizioEsecuzione  { get; set; }

[Display(Name = "Ora Inizio Esecuzione", ShortName="", Description = "Ora di inizio dell'esecuzione", Prompt="")]
[ErpDogField("PR_ORA_INIZIO_ESECUZIONE", SqlFieldNameExt="PR_ORA_INIZIO_ESECUZIONE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
public TimeOnly? PrOraInizioEsecuzione  { get; set; }

[Display(Name = "Data Fine Esecuzione", ShortName="", Description = "Data di completamento dell'esecuzione o annullamento (se appropriato)", Prompt="")]
[ErpDogField("PR_DATA_FINE_ESECUZIONE", SqlFieldNameExt="PR_DATA_FINE_ESECUZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateOnly? PrDataFineEsecuzione  { get; set; }

[Display(Name = "Ora Fine Esecuzione", ShortName="", Description = "Ora di completamento dell'esecuzione o annullamento (se appropriato)", Prompt="")]
[ErpDogField("PR_ORA_FINE_ESECUZIONE", SqlFieldNameExt="PR_ORA_FINE_ESECUZIONE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
public TimeOnly? PrOraFineEsecuzione  { get; set; }

[Display(Name = "Data Refertazione", ShortName="", Description = "Data di refertazione finale", Prompt="")]
[ErpDogField("PR_DATA_REFERTAZIONE", SqlFieldNameExt="PR_DATA_REFERTAZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateOnly? PrDataRefertazione  { get; set; }

[Display(Name = "Ora Refertazione", ShortName="", Description = "Ora di refertazione finale", Prompt="")]
[ErpDogField("PR_ORA_REFERTAZIONE", SqlFieldNameExt="PR_ORA_REFERTAZIONE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
public TimeOnly? PrOraRefertazione  { get; set; }

[Display(Name = "Id Operatore Esecutore", ShortName="", Description = "Codice dell'agente che ha annullato o eseguito l'atto (se appropriato)", Prompt="")]
[ErpDogField("PR_ID_OPERATORE_ESECUTORE", SqlFieldNameExt="PR_ID_OPERATORE_ESECUTORE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PrIdOperatoreEsecutore  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? PrIdOperatoreEsecutoreObj  { get; set; }

[Display(Name = "Id Unita Esecutrice", ShortName="", Description = "Codice dell'unità che ha eseguito/annullato l'atto", Prompt="")]
[ErpDogField("PR_ID_UNITA_ESECUTRICE", SqlFieldNameExt="PR_ID_UNITA_ESECUTRICE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PrIdUnitaEsecutrice  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? PrIdUnitaEsecutriceObj  { get; set; }

[Display(Name = "Id Postazione Esecutrice", ShortName="", Description = "Codice del punto di servizio che ha eseguito/annullato l'atto", Prompt="")]
[ErpDogField("PR_ID_POSTAZIONE_ESECUTRICE", SqlFieldNameExt="PR_ID_POSTAZIONE_ESECUTRICE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PrIdPostazioneEsecutrice  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? PrIdPostazioneEsecutriceObj  { get; set; }

[Display(Name = "Durata Effettiva", ShortName="", Description = "Durata effettiva dell'atto (peso)", Prompt="")]
[ErpDogField("PR_DURATA_EFFETTIVA", SqlFieldNameExt="PR_DURATA_EFFETTIVA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
public short? PrDurataEffettiva  { get; set; }

[Display(Name = "Costo Prestazione", ShortName="", Description = "Costo effettivo", Prompt="")]
[ErpDogField("PR_COSTO_PRESTAZIONE", SqlFieldNameExt="PR_COSTO_PRESTAZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
public double? PrCostoPrestazione  { get; set; }

[Display(Name = "Note Esecuzione", ShortName="", Description = "Breve nota relativa all'esecuzione, all'annullamento, alla sospensione, ecc. dell'atto", Prompt="")]
[ErpDogField("PR_NOTE_ESECUZIONE", SqlFieldNameExt="PR_NOTE_ESECUZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(80, ErrorMessage = "Inserire massimo 80 caratteri")]
[DataType(DataType.Text)]
public string? PrNoteEsecuzione  { get; set; }

[Display(Name = "Id Operatore Pianificatore", ShortName="", Description = "Codice dell'individuo che ha pianificato l'atto", Prompt="")]
[ErpDogField("PR_ID_OPERATORE_PIANIFICATORE", SqlFieldNameExt="PR_ID_OPERATORE_PIANIFICATORE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PrIdOperatorePianificatore  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? PrIdOperatorePianificatoreObj  { get; set; }

[Display(Name = "Data Pianificazione", ShortName="", Description = "Data in cui l'atto è stato pianificato", Prompt="")]
[ErpDogField("PR_DATA_PIANIFICAZIONE", SqlFieldNameExt="PR_DATA_PIANIFICAZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateOnly? PrDataPianificazione  { get; set; }

[Display(Name = "Ora Pianificazione", ShortName="", Description = "Ora in cui l'atto è stato pianificato", Prompt="")]
[ErpDogField("PR_ORA_PIANIFICAZIONE", SqlFieldNameExt="PR_ORA_PIANIFICAZIONE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
public TimeOnly? PrOraPianificazione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note testuali generiche relative all'atto", Prompt="")]
[ErpDogField("PR_NOTE", SqlFieldNameExt="PR_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(240, ErrorMessage = "Inserire massimo 240 caratteri")]
[DataType(DataType.Text)]
public string? PrNote  { get; set; }

[Display(Name = "Mobilita", ShortName="", Description = "Mobilità del paziente (quando applicabile) vuoto: non applicabile - 1: autonomo - 2: sedia a rotelle - 3: a letto - 4: non mobile", Prompt="")]
[ErpDogField("PR_MOBILITA", SqlFieldNameExt="PR_MOBILITA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? PrMobilita  { get; set; }

public override bool TryValidateInt(ModelStateDictionary modelState, string? prefix = null) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioPr1Icode|K|PR__ICODE","sioPr1RecDate|N|PR__MDATE,PR__CDATE"
        ,"sioPrDataRichiesta|N|PR_DATA_RICHIESTA"
        ,"sioPrIdAttivitaEseguitaprDataFineEsecuzione|N|PR_ID_ATTIVITA_ESEGUITA,PR_DATA_FINE_ESECUZIONE"
        ,"sioPrIdRichiesta|N|PR_ID_RICHIESTA"
        ,"sioPrIdEpisodio|N|PR_ID_EPISODIO"
        ,"sioPrIdPazienteprIdEpisodioprIdAttivitaEseguita|N|PR_ID_PAZIENTE,PR_ID_EPISODIO,PR_ID_ATTIVITA_ESEGUITA"
        ,"sioPrIdAttivitaRichiesta|N|PR_ID_ATTIVITA_RICHIESTA"
        ,"sioPrIdUnitaRichiedenteprIdPostazioneRichiedente|N|PR_ID_UNITA_RICHIEDENTE,PR_ID_POSTAZIONE_RICHIEDENTE"
        ,"sioPrIdPostazioneEsecutrice|N|PR_ID_POSTAZIONE_ESECUTRICE"
        ,"sioPrIdUnitaEsecutriceprDataFineEsecuzione|N|PR_ID_UNITA_ESECUTRICE,PR_DATA_FINE_ESECUZIONE"
        ,"sioPrIdOperatorePianificatore|N|PR_ID_OPERATORE_PIANIFICATORE"
        ,"sioPrStatoPrestazione|N|PR_STATO_PRESTAZIONE"
        ,"sioPrDataFineEsecuzioneprOraFineEsecuzione|N|PR_DATA_FINE_ESECUZIONE,PR_ORA_FINE_ESECUZIONE"
        ,"sioPrDataRefertazione|N|PR_DATA_REFERTAZIONE"
        ,"sioPrDataRichiestaprIdPaziente|N|PR_DATA_RICHIESTA,PR_ID_PAZIENTE"
        ,"sioPrDataPropostaEsecuzione|N|PR_DATA_PROPOSTA_ESECUZIONE"
        ,"sioPrDataAppuntamento|N|PR_DATA_APPUNTAMENTO"
    };
}
}
}
