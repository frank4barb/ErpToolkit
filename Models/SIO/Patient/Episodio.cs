using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class Episodio : ModelErp {
public const string Description = "Episodi";
public const string SqlTableName = "EPISODIO";
public const string SqlTableNameExt = "EPISODIO";
public const string SqlRowIdName = "EP__ID";
public const string SqlRowIdNameExt = "EP__ICODE";
public const string SqlPrefix = "EP_";
public const string SqlPrefixExt = "EP_";
public const string SqlXdataTableName = "EP_XDATA";
public const string SqlXdataTableNameExt = "EP_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
public const int INTCODE = 53; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Ep"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
[Display(Name = "Ep1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("EP__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Ep1Ienv { get; set; }
[Key]
[Display(Name = "Ep1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("EP__ICODE", SqlFieldNameExt="EP__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ep1Icode { get; set; }
[Display(Name = "Ep1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("EP__DELETED", SqlFieldNameExt="EP__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ep1Deleted { get; set; }
[Display(Name = "Ep1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("EP__TIMESTAMP", SqlFieldNameExt="EP__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Ep1Timestamp { get; set; }
[Display(Name = "Ep1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("EP__HOME", SqlFieldNameExt="EP__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ep1Home { get; set; }
[Display(Name = "Ep1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("EP__VERSION", SqlFieldNameExt="EP__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ep1Version { get; set; }
[Display(Name = "Ep1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("EP__INACTIVE", SqlFieldNameExt="EP__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ep1Inactive { get; set; }
[Display(Name = "Ep1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("EP__EXTATT", SqlFieldNameExt="EP__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Ep1Extatt { get; set; }


[Display(Name = "Cod Episodio", ShortName="", Description = "Identificativo del contatto nell'organizzazione sanitaria", Prompt="")]
[ErpDogField("EP_COD_EPISODIO", SqlFieldNameExt="EP_COD_EPISODIO", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(EPISODIO.EP__ICODE[EP__ICODE] {EP_COD_EPISODIO=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(16, ErrorMessage = "Inserire massimo 16 caratteri")]
[DataType(DataType.Text)]
public string? EpCodEpisodio  { get; set; }

[Display(Name = "Id Paziente", ShortName="", Description = "Codice del paziente a cui si riferisce il contatto", Prompt="")]
[ErpDogField("EP_ID_PAZIENTE", SqlFieldNameExt="EP_ID_PAZIENTE", SqlFieldOptions="", Xref="Pa1Icode", SqlFieldProperties="prop() xref(PAZIENTE.PA__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteServer("Paziente", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? EpIdPaziente  { get; set; }
public ErpToolkit.Models.SIO.Patient.Paziente? EpIdPazienteObj  { get; set; }

[Display(Name = "Sesso", ShortName="", Description = "Sesso del paziente al momento dell'ammissione", Prompt="")]
[ErpDogField("EP_SESSO", SqlFieldNameExt="EP_SESSO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup(PAZIENTE.PA_SESSO[EPISODIO.EP_ID_PAZIENTE] {EP_SESSO=' '}) multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "M", "F", "N" }, MaxSelections=1, LabelClassName="")]
public string? EpSesso  { get; set; }

[Display(Name = "Classe Episodio", ShortName="", Description = "Classe di contatto 1=Permanenza 2=Day-hospital 3=Ambulatoriale 4-=definito dall'utente", Prompt="")]
[ErpDogField("EP_CLASSE_EPISODIO", SqlFieldNameExt="EP_CLASSE_EPISODIO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_EPISODIO.TE_CLASSE[EPISODIO.EP_ID_TIPO_EPISODIO]) multbxref()")]
[DefaultValue("1")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" }, MaxSelections=1, LabelClassName="")]
public string? EpClasseEpisodio  { get; set; }

[Display(Name = "Id Tipo Episodio", ShortName="", Description = "Codice del tipo di contatto", Prompt="")]
[ErpDogField("EP_ID_TIPO_EPISODIO", SqlFieldNameExt="EP_ID_TIPO_EPISODIO", SqlFieldOptions="", Xref="Te1Icode", SqlFieldProperties="prop() xref(TIPO_EPISODIO.TE__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteClient("TipoEpisodio", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? EpIdTipoEpisodio  { get; set; }
public ErpToolkit.Models.SIO.Act.TipoEpisodio? EpIdTipoEpisodioObj  { get; set; }

[Display(Name = "Stato Episodio", ShortName="", Description = "Stato del contatto F[oreseen] - A[ctual, in progress] - C[ompleted] - D[eleted] - S[uspended]", Prompt="")]
[ErpDogField("EP_STATO_EPISODIO", SqlFieldNameExt="EP_STATO_EPISODIO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "F", "A", "C", "D", "S" }, MaxSelections=1, LabelClassName="")]
public string? EpStatoEpisodio  { get; set; }

[Display(Name = "Id Unita Ingresso", ShortName="", Description = "Identificativo dell'unità di assistenza che ha avviato il contatto", Prompt="")]
[ErpDogField("EP_ID_UNITA_INGRESSO", SqlFieldNameExt="EP_ID_UNITA_INGRESSO", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? EpIdUnitaIngresso  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? EpIdUnitaIngressoObj  { get; set; }

[Display(Name = "Data Inizio", ShortName="", Description = "Data di inizio del periodo di permanenza del contatto", Prompt="")]
[ErpDogField("EP_DATA_INIZIO", SqlFieldNameExt="EP_DATA_INIZIO", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateOnly? EpDataInizio  { get; set; }

[Display(Name = "Ora Inizio", ShortName="", Description = "Ora di inizio del periodo di permanenza del contatto", Prompt="")]
[ErpDogField("EP_ORA_INIZIO", SqlFieldNameExt="EP_ORA_INIZIO", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? EpOraInizio  { get; set; }

[Display(Name = "Data Fine", ShortName="", Description = "Data di fine del periodo di permanenza del contatto", Prompt="")]
[ErpDogField("EP_DATA_FINE", SqlFieldNameExt="EP_DATA_FINE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateOnly? EpDataFine  { get; set; }

[Display(Name = "Ora Fine", ShortName="", Description = "Ora di fine del periodo di permanenza del contatto", Prompt="")]
[ErpDogField("EP_ORA_FINE", SqlFieldNameExt="EP_ORA_FINE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? EpOraFine  { get; set; }

[Display(Name = "Cartella Ps", ShortName="", Description = "Identificativo del documento correlato dell'organizzazione di origine", Prompt="")]
[ErpDogField("EP_CARTELLA_PS", SqlFieldNameExt="EP_CARTELLA_PS", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(20, ErrorMessage = "Inserire massimo 20 caratteri")]
[DataType(DataType.Text)]
public string? EpCartellaPs  { get; set; }

[Display(Name = "Id Corsia", ShortName="", Description = "Codice dell'ultima (o attuale) unità in cui è ubicato il paziente", Prompt="")]
[ErpDogField("EP_ID_CORSIA", SqlFieldNameExt="EP_ID_CORSIA", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? EpIdCorsia  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? EpIdCorsiaObj  { get; set; }

[Display(Name = "Id Reparto", ShortName="", Description = "Codice dell'unità responsabile del paziente", Prompt="")]
[ErpDogField("EP_ID_REPARTO", SqlFieldNameExt="EP_ID_REPARTO", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup(EPISODIO.EP_ID_CORSIA[EP__ICODE] {EP_ID_REPARTO=' '}) multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? EpIdReparto  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? EpIdRepartoObj  { get; set; }

[Display(Name = "Letto", ShortName="", Description = "Letto assegnato al paziente", Prompt="")]
[ErpDogField("EP_LETTO", SqlFieldNameExt="EP_LETTO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
[StringLength(15, ErrorMessage = "Inserire massimo 15 caratteri")]
[DataType(DataType.Text)]
public string? EpLetto  { get; set; }

[Display(Name = "Stanza", ShortName="", Description = "Stanza e altre strutture logistiche correlate al contatto", Prompt="")]
[ErpDogField("EP_STANZA", SqlFieldNameExt="EP_STANZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? EpStanza  { get; set; }

[Display(Name = "Id Diagnosi Ammissione", ShortName="", Description = "Codice della diagnosi di ammissione", Prompt="")]
[ErpDogField("EP_ID_DIAGNOSI_AMMISSIONE", SqlFieldNameExt="EP_ID_DIAGNOSI_AMMISSIONE", SqlFieldOptions="", Xref="Dg1Icode", SqlFieldProperties="prop() xref(DIAGNOSI.DG__ICODE) xdup() multbxref()")]
[AutocompleteClient("Diagnosi", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? EpIdDiagnosiAmmissione  { get; set; }
public ErpToolkit.Models.SIO.Costs.Diagnosi? EpIdDiagnosiAmmissioneObj  { get; set; }

[Display(Name = "Id Diagnosi Dimissione", ShortName="", Description = "Codice della diagnosi di dimissione", Prompt="")]
[ErpDogField("EP_ID_DIAGNOSI_DIMISSIONE", SqlFieldNameExt="EP_ID_DIAGNOSI_DIMISSIONE", SqlFieldOptions="", Xref="Dg1Icode", SqlFieldProperties="prop() xref(DIAGNOSI.DG__ICODE) xdup() multbxref()")]
[AutocompleteClient("Diagnosi", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? EpIdDiagnosiDimissione  { get; set; }
public ErpToolkit.Models.SIO.Costs.Diagnosi? EpIdDiagnosiDimissioneObj  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note aggiuntive generiche", Prompt="")]
[ErpDogField("EP_NOTE", SqlFieldNameExt="EP_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? EpNote  { get; set; }

[Display(Name = "Id Atto Amministrativo", ShortName="", Description = "Identificativo dell'atto che descrive gli aspetti organizzativi attuali del contatto", Prompt="")]
[ErpDogField("EP_ID_ATTO_AMMINISTRATIVO", SqlFieldNameExt="EP_ID_ATTO_AMMINISTRATIVO", SqlFieldOptions="", Xref="Pr1Icode", SqlFieldProperties="prop() xref(PRESTAZIONE.PR__ICODE) xdup() multbxref()")]
[AutocompleteServer("Prestazione", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? EpIdAttoAmministrativo  { get; set; }
public ErpToolkit.Models.SIO.Act.Prestazione? EpIdAttoAmministrativoObj  { get; set; }

[Display(Name = "Data Inizio La", ShortName="", Description = "Data di inizio del periodo di lista d'attesa del contatto", Prompt="")]
[ErpDogField("EP_DATA_INIZIO_LA", SqlFieldNameExt="EP_DATA_INIZIO_LA", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateOnly? EpDataInizioLa  { get; set; }

[Display(Name = "Ora Inizio La", ShortName="", Description = "Ora di inizio del periodo di lista d'attesa del contatto", Prompt="")]
[ErpDogField("EP_ORA_INIZIO_LA", SqlFieldNameExt="EP_ORA_INIZIO_LA", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? EpOraInizioLa  { get; set; }

[Display(Name = "Id Reparto La", ShortName="", Description = "Identificativo dell'unità di assistenza responsabile del periodo di lista d'attesa", Prompt="")]
[ErpDogField("EP_ID_REPARTO_LA", SqlFieldNameExt="EP_ID_REPARTO_LA", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? EpIdRepartoLa  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? EpIdRepartoLaObj  { get; set; }

[Display(Name = "Data Inizio Preh", ShortName="", Description = "Data di inizio del periodo di preospedalizzazione del contatto", Prompt="")]
[ErpDogField("EP_DATA_INIZIO_PREH", SqlFieldNameExt="EP_DATA_INIZIO_PREH", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateOnly? EpDataInizioPreh  { get; set; }

[Display(Name = "Ora Inizio Preh", ShortName="", Description = "Ora di inizio del periodo di preospedalizzazione del contatto", Prompt="")]
[ErpDogField("EP_ORA_INIZIO_PREH", SqlFieldNameExt="EP_ORA_INIZIO_PREH", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? EpOraInizioPreh  { get; set; }

[Display(Name = "Id Reparto Preh", ShortName="", Description = "Identificativo dell'unità di assistenza responsabile del periodo di preospedalizzazione", Prompt="")]
[ErpDogField("EP_ID_REPARTO_PREH", SqlFieldNameExt="EP_ID_REPARTO_PREH", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? EpIdRepartoPreh  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? EpIdRepartoPrehObj  { get; set; }

[Display(Name = "Fase Episodio", ShortName="", Description = "Codice del tipo attuale (ultimo) fase del contatto (ad es. Lista d'attesa, Preospedalizzazione, In-staying, Home-care, Sospeso)", Prompt="")]
[ErpDogField("EP_FASE_EPISODIO", SqlFieldNameExt="EP_FASE_EPISODIO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("I")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? EpFaseEpisodio  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioEp1Icode|K|Ep1Icode","sioEp1RecDate|N|Ep1Mdate,Ep1Cdate"
        ,"sioEpLetto|N|EpLetto"
        ,"sioEpDataFine|N|EpDataFine"
        ,"sioEpIdCorsiaEpStatoEpisodio|N|EpIdCorsia,EpStatoEpisodio"
        ,"sioEpIdTipoEpisodioEpDataInizio|N|EpIdTipoEpisodio,EpDataInizio"
        ,"sioEpIdAttoAmministrativo|N|EpIdAttoAmministrativo"
        ,"sioEpIdDiagnosiDimissione|N|EpIdDiagnosiDimissione"
        ,"sioEpCartellaPs|N|EpCartellaPs"
        ,"sioEpIdTipoEpisodio|N|EpIdTipoEpisodio"
        ,"sioEpIdPaziente|N|EpIdPaziente"
        ,"sioEpIdRepartoEpStatoEpisodio|N|EpIdReparto,EpStatoEpisodio"
        ,"sioEpDataInizio|N|EpDataInizio"
        ,"sioEpStatoEpisodioEpDataInizioEpDataFine|N|EpStatoEpisodio,EpDataInizio,EpDataFine"
        ,"sioEpCodEpisodioEp1VersionEp1Deleted|U|EpCodEpisodio,Ep1Version,Ep1Deleted"
    };
}
}
}
