using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Resource {
public class Farmaco {
public const string Description = "Risorse: farmaci";
public const string SqlTableName = "FARMACO";
public const string SqlTableNameExt = "FARMACO";
public const string SqlRowIdName = "FM__ID";
public const string SqlRowIdNameExt = "FM__ICODE";
public const string SqlPrefix = "FM_";
public const string SqlPrefixExt = "FM_";
public const string SqlXdataTableName = "FM_XDATA";
public const string SqlXdataTableNameExt = "FM_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 98; //Internal Table Code
public const string TBAREA = "Risorse"; //Table Area
public const string PREFIX = "Fm"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//127-124//REL_PRESTAZIONE_USA.PU_ID_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa> RelPrestazioneUsa4PuIdRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa>();
//1182-1179//REL_ATTIVITA_USA.AU_ID_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa> RelAttivitaUsa4AuIdRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa>();
[Display(Name = "Fm1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("FM__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Fm1Ienv { get; set; }
[Key]
[Display(Name = "Fm1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("FM__ICODE", SqlFieldNameExt="FM__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Fm1Icode { get; set; }
[Display(Name = "Fm1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("FM__DELETED", SqlFieldNameExt="FM__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Fm1Deleted { get; set; }
[Display(Name = "Fm1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("FM__TIMESTAMP", SqlFieldNameExt="FM__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Fm1Timestamp { get; set; }
[Display(Name = "Fm1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("FM__HOME", SqlFieldNameExt="FM__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Fm1Home { get; set; }
[Display(Name = "Fm1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("FM__VERSION", SqlFieldNameExt="FM__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Fm1Version { get; set; }
[Display(Name = "Fm1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("FM__INACTIVE", SqlFieldNameExt="FM__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Fm1Inactive { get; set; }
[Display(Name = "Fm1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("FM__EXTATT", SqlFieldNameExt="FM__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Fm1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("FM_CODICE", SqlFieldNameExt="FM_CODICE", SqlFieldOptions="[UID]", SqlFieldProperties="prop() xref() xdup(FARMACO.FM__ICODE[FM__ICODE] {FM_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? FmCodice  { get; set; }

[Display(Name = "Classe Risorsa", ShortName="", Description = "Classe di risorsa D: farmaci", Prompt="")]
[ErpDogField("FM_CLASSE_RISORSA", SqlFieldNameExt="FM_CLASSE_RISORSA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup(TIPO_RISORSA.TS_CLASSE_RISORSA[FARMACO.FM_ID_TIPO_RISORSA]) multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("D")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? FmClasseRisorsa  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("FM_DESCRIZIONE", SqlFieldNameExt="FM_DESCRIZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? FmDescrizione  { get; set; }

[Display(Name = "Id Tipo Risorsa", ShortName="", Description = "Codice del tipo di farmaco", Prompt="")]
[ErpDogField("FM_ID_TIPO_RISORSA", SqlFieldNameExt="FM_ID_TIPO_RISORSA", SqlFieldOptions="", SqlFieldProperties="prop() xref(TIPO_RISORSA.TS__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[AutocompleteClient("TipoRisorsa", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? FmIdTipoRisorsa  { get; set; }
public ErpToolkit.Models.SIO.Resource.TipoRisorsa? FmIdTipoRisorsaObj  { get; set; }

[Display(Name = "Costo Unitario Uso", ShortName="", Description = "Costo unitario per l'utilizzo", Prompt="")]
[ErpDogField("FM_COSTO_UNITARIO_USO", SqlFieldNameExt="FM_COSTO_UNITARIO_USO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? FmCostoUnitarioUso  { get; set; }

[Display(Name = "Misura Unitaria Uso", ShortName="", Description = "Unità di misura per l'utilizzo", Prompt="")]
[ErpDogField("FM_MISURA_UNITARIA_USO", SqlFieldNameExt="FM_MISURA_UNITARIA_USO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? FmMisuraUnitariaUso  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("FM_NOTE", SqlFieldNameExt="FM_NOTE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? FmNote  { get; set; }

[Display(Name = "Disponibilita", ShortName="", Description = "Descrizione testuale dello stato attuale di disponibilità", Prompt="")]
[ErpDogField("FM_DISPONIBILITA", SqlFieldNameExt="FM_DISPONIBILITA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(20, ErrorMessage = "Inserire massimo 20 caratteri")]
[DataType(DataType.Text)]
public string? FmDisponibilita  { get; set; }

[Display(Name = "Quantita Disponibile", ShortName="", Description = "Quantità attualmente disponibile", Prompt="")]
[ErpDogField("FM_QUANTITA_DISPONIBILE", SqlFieldNameExt="FM_QUANTITA_DISPONIBILE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? FmQuantitaDisponibile  { get; set; }

[Display(Name = "Quantita Minima Magazzino", ShortName="", Description = "Quantità minima che deve essere disponibile", Prompt="")]
[ErpDogField("FM_QUANTITA_MINIMA_MAGAZZINO", SqlFieldNameExt="FM_QUANTITA_MINIMA_MAGAZZINO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? FmQuantitaMinimaMagazzino  { get; set; }

[Display(Name = "Uso Medio Giornaliero", ShortName="", Description = "Utilizzo medio al giorno (numero di unità)", Prompt="")]
[ErpDogField("FM_USO_MEDIO_GIORNALIERO", SqlFieldNameExt="FM_USO_MEDIO_GIORNALIERO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? FmUsoMedioGiornaliero  { get; set; }

[Display(Name = "Data Ultimo Ordine", ShortName="", Description = "Data dell'ultimo ordine", Prompt="")]
[ErpDogField("FM_DATA_ULTIMO_ORDINE", SqlFieldNameExt="FM_DATA_ULTIMO_ORDINE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public string? FmDataUltimoOrdine  { get; set; }

[Display(Name = "Quantita Ultimo Ordine", ShortName="", Description = "Quantità dell'ultimo ordine", Prompt="")]
[ErpDogField("FM_QUANTITA_ULTIMO_ORDINE", SqlFieldNameExt="FM_QUANTITA_ULTIMO_ORDINE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? FmQuantitaUltimoOrdine  { get; set; }

[Display(Name = "Data Prossimo Ordine", ShortName="", Description = "Data prevista per il prossimo ordine", Prompt="")]
[ErpDogField("FM_DATA_PROSSIMO_ORDINE", SqlFieldNameExt="FM_DATA_PROSSIMO_ORDINE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public string? FmDataProssimoOrdine  { get; set; }

[Display(Name = "Quantita Media Ordine", ShortName="", Description = "Quantità media per ordine", Prompt="")]
[ErpDogField("FM_QUANTITA_MEDIA_ORDINE", SqlFieldNameExt="FM_QUANTITA_MEDIA_ORDINE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? FmQuantitaMediaOrdine  { get; set; }

[Display(Name = "Riferimenti Fornitore", ShortName="", Description = "Riferimento per il fornitore", Prompt="")]
[ErpDogField("FM_RIFERIMENTI_FORNITORE", SqlFieldNameExt="FM_RIFERIMENTI_FORNITORE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(20, ErrorMessage = "Inserire massimo 20 caratteri")]
[DataType(DataType.Text)]
public string? FmRiferimentiFornitore  { get; set; }

[Display(Name = "Codice Nazionale", ShortName="", Description = "Codice nazionale", Prompt="")]
[ErpDogField("FM_CODICE_NAZIONALE", SqlFieldNameExt="FM_CODICE_NAZIONALE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(20, ErrorMessage = "Inserire massimo 20 caratteri")]
[DataType(DataType.Text)]
public string? FmCodiceNazionale  { get; set; }

[Display(Name = "Codice Aic", ShortName="", Description = "Codice ministeriale in base 10", Prompt="")]
[ErpDogField("FM_CODICE_AIC", SqlFieldNameExt="FM_CODICE_AIC", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? FmCodiceAic  { get; set; }

[Display(Name = "Data Inizio Autorizzazione", ShortName="", Description = "Data di inizio dell'autorizzazione governativa", Prompt="")]
[ErpDogField("FM_DATA_INIZIO_AUTORIZZAZIONE", SqlFieldNameExt="FM_DATA_INIZIO_AUTORIZZAZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public string? FmDataInizioAutorizzazione  { get; set; }

[Display(Name = "Data Fine Autorizzazione", ShortName="", Description = "Data di fine dell'autorizzazione governativa", Prompt="")]
[ErpDogField("FM_DATA_FINE_AUTORIZZAZIONE", SqlFieldNameExt="FM_DATA_FINE_AUTORIZZAZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public string? FmDataFineAutorizzazione  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioFm1Icode|K|Fm1Icode","sioFm1RecDate|N|Fm1Mdate,Fm1Cdate"
        ,"sioFmIdTipoRisorsa|N|FmIdTipoRisorsa"
        ,"sioFm1VersionFm1Deleted|U|Fm1Version,Fm1Deleted"
        ,"sioFmCodiceFm1VersionFm1Deleted|U|FmCodice,Fm1Version,Fm1Deleted"
    };
}
}
}
