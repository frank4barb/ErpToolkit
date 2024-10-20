using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class SelEpisodio : ModelErp {
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
public const string CATEG = "SEL"; //Data Model Name of the Class
public const int INTCODE = 53; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Ep"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Cod Episodio", ShortName="", Description = "Identificativo del contatto nell'organizzazione sanitaria", Prompt="")]
[ErpDogField("EP_COD_EPISODIO", SqlFieldNameExt="EP_COD_EPISODIO", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(EPISODIO.EP__ICODE[EP__ICODE] {EP_COD_EPISODIO=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? SelEpCodEpisodio  { get; set; }

[Display(Name = "Id Paziente", ShortName="", Description = "Codice del paziente a cui si riferisce il contatto", Prompt="")]
[ErpDogField("EP_ID_PAZIENTE", SqlFieldNameExt="EP_ID_PAZIENTE", SqlFieldOptions="", Xref="Pa1Icode", SqlFieldProperties="prop() xref(PAZIENTE.PA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Paziente", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> SelEpIdPaziente  { get; set; } = new List<string>();

[Display(Name = "Sesso", ShortName="", Description = "Sesso del paziente al momento dell'ammissione", Prompt="")]
[ErpDogField("EP_SESSO", SqlFieldNameExt="EP_SESSO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup(PAZIENTE.PA_SESSO[EPISODIO.EP_ID_PAZIENTE] {EP_SESSO=' '}) multbxref()")]
[DefaultValue(" ")]
[MultipleChoices(new[] { "M", "F", "N" }, MaxSelections=-1, LabelClassName="")]
[DataType(DataType.Text)]
public List<string> SelEpSesso  { get; set; } = new List<string>();

[Display(Name = "Classe Episodio", ShortName="", Description = "Classe di contatto 1=Permanenza 2=Day-hospital 3=Ambulatoriale 4-=definito dall'utente", Prompt="")]
[ErpDogField("EP_CLASSE_EPISODIO", SqlFieldNameExt="EP_CLASSE_EPISODIO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_EPISODIO.TE_CLASSE[EPISODIO.EP_ID_TIPO_EPISODIO]) multbxref()")]
[DefaultValue("1")]
[MultipleChoices(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" }, MaxSelections=-1, LabelClassName="")]
[DataType(DataType.Text)]
public List<string> SelEpClasseEpisodio  { get; set; } = new List<string>();

[Display(Name = "Id Tipo Episodio", ShortName="", Description = "Codice del tipo di contatto", Prompt="")]
[ErpDogField("EP_ID_TIPO_EPISODIO", SqlFieldNameExt="EP_ID_TIPO_EPISODIO", SqlFieldOptions="", Xref="Te1Icode", SqlFieldProperties="prop() xref(TIPO_EPISODIO.TE__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoEpisodio", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelEpIdTipoEpisodio  { get; set; } = new List<string>();

[Display(Name = "Stato Episodio", ShortName="", Description = "Stato del contatto F[oreseen] - A[ctual, in progress] - C[ompleted] - D[eleted] - S[uspended]", Prompt="")]
[ErpDogField("EP_STATO_EPISODIO", SqlFieldNameExt="EP_STATO_EPISODIO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[MultipleChoices(new[] { "F", "A", "C", "D", "S" }, MaxSelections=-1, LabelClassName="")]
[DataType(DataType.Text)]
public List<string> SelEpStatoEpisodio  { get; set; } = new List<string>();

[Display(Name = "Id Unita Ingresso", ShortName="", Description = "Identificativo dell'unità di assistenza che ha avviato il contatto", Prompt="")]
[ErpDogField("EP_ID_UNITA_INGRESSO", SqlFieldNameExt="EP_ID_UNITA_INGRESSO", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelEpIdUnitaIngresso  { get; set; } = new List<string>();

[Display(Name = "Data Inizio", ShortName="", Description = "Data di inizio del periodo di permanenza del contatto", Prompt="")]
[ErpDogField("EP_DATA_INIZIO", SqlFieldNameExt="EP_DATA_INIZIO", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SelEpDataInizio  { get; set; } = new DateRange();

[Display(Name = "Ora Inizio", ShortName="", Description = "Ora di inizio del periodo di permanenza del contatto", Prompt="")]
[ErpDogField("EP_ORA_INIZIO", SqlFieldNameExt="EP_ORA_INIZIO", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? SelEpOraInizio  { get; set; }

[Display(Name = "Data Fine", ShortName="", Description = "Data di fine del periodo di permanenza del contatto", Prompt="")]
[ErpDogField("EP_DATA_FINE", SqlFieldNameExt="EP_DATA_FINE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SelEpDataFine  { get; set; } = new DateRange();

[Display(Name = "Ora Fine", ShortName="", Description = "Ora di fine del periodo di permanenza del contatto", Prompt="")]
[ErpDogField("EP_ORA_FINE", SqlFieldNameExt="EP_ORA_FINE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? SelEpOraFine  { get; set; }

[Display(Name = "Cartella Ps", ShortName="", Description = "Identificativo del documento correlato dell'organizzazione di origine", Prompt="")]
[ErpDogField("EP_CARTELLA_PS", SqlFieldNameExt="EP_CARTELLA_PS", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelEpCartellaPs  { get; set; }

[Display(Name = "Id Corsia", ShortName="", Description = "Codice dell'ultima (o attuale) unità in cui è ubicato il paziente", Prompt="")]
[ErpDogField("EP_ID_CORSIA", SqlFieldNameExt="EP_ID_CORSIA", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelEpIdCorsia  { get; set; } = new List<string>();

[Display(Name = "Id Reparto", ShortName="", Description = "Codice dell'unità responsabile del paziente", Prompt="")]
[ErpDogField("EP_ID_REPARTO", SqlFieldNameExt="EP_ID_REPARTO", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup(EPISODIO.EP_ID_CORSIA[EP__ICODE] {EP_ID_REPARTO=' '}) multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelEpIdReparto  { get; set; } = new List<string>();

[Display(Name = "Letto", ShortName="", Description = "Letto assegnato al paziente", Prompt="")]
[ErpDogField("EP_LETTO", SqlFieldNameExt="EP_LETTO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelEpLetto  { get; set; }

[Display(Name = "Stanza", ShortName="", Description = "Stanza e altre strutture logistiche correlate al contatto", Prompt="")]
[ErpDogField("EP_STANZA", SqlFieldNameExt="EP_STANZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelEpStanza  { get; set; }

[Display(Name = "Id Diagnosi Ammissione", ShortName="", Description = "Codice della diagnosi di ammissione", Prompt="")]
[ErpDogField("EP_ID_DIAGNOSI_AMMISSIONE", SqlFieldNameExt="EP_ID_DIAGNOSI_AMMISSIONE", SqlFieldOptions="", Xref="Dg1Icode", SqlFieldProperties="prop() xref(DIAGNOSI.DG__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Diagnosi", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelEpIdDiagnosiAmmissione  { get; set; } = new List<string>();

[Display(Name = "Id Diagnosi Dimissione", ShortName="", Description = "Codice della diagnosi di dimissione", Prompt="")]
[ErpDogField("EP_ID_DIAGNOSI_DIMISSIONE", SqlFieldNameExt="EP_ID_DIAGNOSI_DIMISSIONE", SqlFieldOptions="", Xref="Dg1Icode", SqlFieldProperties="prop() xref(DIAGNOSI.DG__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Diagnosi", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelEpIdDiagnosiDimissione  { get; set; } = new List<string>();

[Display(Name = "Note", ShortName="", Description = "Note aggiuntive generiche", Prompt="")]
[ErpDogField("EP_NOTE", SqlFieldNameExt="EP_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelEpNote  { get; set; }

[Display(Name = "Id Atto Amministrativo", ShortName="", Description = "Identificativo dell'atto che descrive gli aspetti organizzativi attuali del contatto", Prompt="")]
[ErpDogField("EP_ID_ATTO_AMMINISTRATIVO", SqlFieldNameExt="EP_ID_ATTO_AMMINISTRATIVO", SqlFieldOptions="", Xref="Pr1Icode", SqlFieldProperties="prop() xref(PRESTAZIONE.PR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Prestazione", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> SelEpIdAttoAmministrativo  { get; set; } = new List<string>();

[Display(Name = "Data Inizio La", ShortName="", Description = "Data di inizio del periodo di lista d'attesa del contatto", Prompt="")]
[ErpDogField("EP_DATA_INIZIO_LA", SqlFieldNameExt="EP_DATA_INIZIO_LA", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SelEpDataInizioLa  { get; set; } = new DateRange();

[Display(Name = "Ora Inizio La", ShortName="", Description = "Ora di inizio del periodo di lista d'attesa del contatto", Prompt="")]
[ErpDogField("EP_ORA_INIZIO_LA", SqlFieldNameExt="EP_ORA_INIZIO_LA", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? SelEpOraInizioLa  { get; set; }

[Display(Name = "Id Reparto La", ShortName="", Description = "Identificativo dell'unità di assistenza responsabile del periodo di lista d'attesa", Prompt="")]
[ErpDogField("EP_ID_REPARTO_LA", SqlFieldNameExt="EP_ID_REPARTO_LA", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelEpIdRepartoLa  { get; set; } = new List<string>();

[Display(Name = "Data Inizio Preh", ShortName="", Description = "Data di inizio del periodo di preospedalizzazione del contatto", Prompt="")]
[ErpDogField("EP_DATA_INIZIO_PREH", SqlFieldNameExt="EP_DATA_INIZIO_PREH", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SelEpDataInizioPreh  { get; set; } = new DateRange();

[Display(Name = "Ora Inizio Preh", ShortName="", Description = "Ora di inizio del periodo di preospedalizzazione del contatto", Prompt="")]
[ErpDogField("EP_ORA_INIZIO_PREH", SqlFieldNameExt="EP_ORA_INIZIO_PREH", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? SelEpOraInizioPreh  { get; set; }

[Display(Name = "Id Reparto Preh", ShortName="", Description = "Identificativo dell'unità di assistenza responsabile del periodo di preospedalizzazione", Prompt="")]
[ErpDogField("EP_ID_REPARTO_PREH", SqlFieldNameExt="EP_ID_REPARTO_PREH", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelEpIdRepartoPreh  { get; set; } = new List<string>();

[Display(Name = "Fase Episodio", ShortName="", Description = "Codice del tipo attuale (ultimo) fase del contatto (ad es. Lista d'attesa, Preospedalizzazione, In-staying, Home-care, Sospeso)", Prompt="")]
[ErpDogField("EP_FASE_EPISODIO", SqlFieldNameExt="EP_FASE_EPISODIO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelEpFaseEpisodio  { get; set; }

public override bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        // verifica se almeno un campo indicizzato è valorizzato (test per validazioni complesse del modello) 
        bool found = false; 
        foreach (var idx in ListIndexes()) { 
            string fldLst = idx.Split("|")[2]; 
            foreach (var fld in fldLst.Split(",")) { 
                if (DogManager.getPropertyValue(this, "Sel" + UtilHelper.field2Property(fld.Trim())) != null) found = true; 
                if (DogManager.getPropertyValue(this, "Sel" + UtilHelper.field2Property(fld.Trim()) + "[0]") != null) found = true; 
                if (DogManager.getPropertyValue(this, "Sel" + UtilHelper.field2Property(fld.Trim()) + ".StartDate") != null) found = true; 
                if (DogManager.getPropertyValue(this, "Sel" + UtilHelper.field2Property(fld.Trim()) + ".EndDate") != null) found = true; 
            } 
        } 
        if (!found) { isValidate = false;  modelState.AddModelError(string.Empty, "Deve essere compilato almeno un campo indicizzato."); } 
        //-- 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioEp1Icode|K|EP__ICODE","sioEp1RecDate|N|EP__MDATE,EP__CDATE"
        ,"sioEpLetto|N|EP_LETTO"
        ,"sioEpDataFine|N|EP_DATA_FINE"
        ,"sioEpIdCorsiaepStatoEpisodio|N|EP_ID_CORSIA,EP_STATO_EPISODIO"
        ,"sioEpIdTipoEpisodioepDataInizio|N|EP_ID_TIPO_EPISODIO,EP_DATA_INIZIO"
        ,"sioEpIdAttoAmministrativo|N|EP_ID_ATTO_AMMINISTRATIVO"
        ,"sioEpIdDiagnosiDimissione|N|EP_ID_DIAGNOSI_DIMISSIONE"
        ,"sioEpCartellaPs|N|EP_CARTELLA_PS"
        ,"sioEpIdTipoEpisodio|N|EP_ID_TIPO_EPISODIO"
        ,"sioEpIdPaziente|N|EP_ID_PAZIENTE"
        ,"sioEpIdRepartoepStatoEpisodio|N|EP_ID_REPARTO,EP_STATO_EPISODIO"
        ,"sioEpDataInizio|N|EP_DATA_INIZIO"
        ,"sioEpStatoEpisodioepDataInizioepDataFine|N|EP_STATO_EPISODIO,EP_DATA_INIZIO,EP_DATA_FINE"
        ,"sioEpCodEpisodioep1Versionep1Deleted|U|EP_COD_EPISODIO,EP__VERSION,EP__DELETED"
    };
}
}
}
