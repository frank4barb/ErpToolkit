using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Resource {
public class SelFarmaco {
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

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("FM_CODICE", SqlFieldNameExt="FM_CODICE", SqlFieldProperties="prop() xref() xdup(FARMACO.FM__ICODE[FM__ICODE] {FM_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? FmCodice  { get; set; }

[Display(Name = "Classe Risorsa", ShortName="", Description = "Classe di risorsa D: farmaci", Prompt="")]
[ErpDogField("FM_CLASSE_RISORSA", SqlFieldNameExt="FM_CLASSE_RISORSA", SqlFieldProperties="prop() xref() xdup(TIPO_RISORSA.TS_CLASSE_RISORSA[FARMACO.FM_ID_TIPO_RISORSA]) multbxref()")]
[DataType(DataType.Text)]
public string? FmClasseRisorsa  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("FM_DESCRIZIONE", SqlFieldNameExt="FM_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? FmDescrizione  { get; set; }

[Display(Name = "Id Tipo Risorsa", ShortName="", Description = "Codice del tipo di farmaco", Prompt="")]
[ErpDogField("FM_ID_TIPO_RISORSA", SqlFieldNameExt="FM_ID_TIPO_RISORSA", SqlFieldProperties="prop() xref(TIPO_RISORSA.TS__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoRisorsa", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> FmIdTipoRisorsa  { get; set; } = new List<string>();

[Display(Name = "Costo Unitario Uso", ShortName="", Description = "Costo unitario per l'utilizzo", Prompt="")]
[ErpDogField("FM_COSTO_UNITARIO_USO", SqlFieldNameExt="FM_COSTO_UNITARIO_USO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? FmCostoUnitarioUso  { get; set; }

[Display(Name = "Misura Unitaria Uso", ShortName="", Description = "Unità di misura per l'utilizzo", Prompt="")]
[ErpDogField("FM_MISURA_UNITARIA_USO", SqlFieldNameExt="FM_MISURA_UNITARIA_USO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? FmMisuraUnitariaUso  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("FM_NOTE", SqlFieldNameExt="FM_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? FmNote  { get; set; }

[Display(Name = "Disponibilita", ShortName="", Description = "Descrizione testuale dello stato attuale di disponibilità", Prompt="")]
[ErpDogField("FM_DISPONIBILITA", SqlFieldNameExt="FM_DISPONIBILITA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? FmDisponibilita  { get; set; }

[Display(Name = "Quantita Disponibile", ShortName="", Description = "Quantità attualmente disponibile", Prompt="")]
[ErpDogField("FM_QUANTITA_DISPONIBILE", SqlFieldNameExt="FM_QUANTITA_DISPONIBILE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? FmQuantitaDisponibile  { get; set; }

[Display(Name = "Quantita Minima Magazzino", ShortName="", Description = "Quantità minima che deve essere disponibile", Prompt="")]
[ErpDogField("FM_QUANTITA_MINIMA_MAGAZZINO", SqlFieldNameExt="FM_QUANTITA_MINIMA_MAGAZZINO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? FmQuantitaMinimaMagazzino  { get; set; }

[Display(Name = "Uso Medio Giornaliero", ShortName="", Description = "Utilizzo medio al giorno (numero di unità)", Prompt="")]
[ErpDogField("FM_USO_MEDIO_GIORNALIERO", SqlFieldNameExt="FM_USO_MEDIO_GIORNALIERO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? FmUsoMedioGiornaliero  { get; set; }

[Display(Name = "Data Ultimo Ordine", ShortName="", Description = "Data dell'ultimo ordine", Prompt="")]
[ErpDogField("FM_DATA_ULTIMO_ORDINE", SqlFieldNameExt="FM_DATA_ULTIMO_ORDINE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange FmDataUltimoOrdine  { get; set; } = new DateRange();

[Display(Name = "Quantita Ultimo Ordine", ShortName="", Description = "Quantità dell'ultimo ordine", Prompt="")]
[ErpDogField("FM_QUANTITA_ULTIMO_ORDINE", SqlFieldNameExt="FM_QUANTITA_ULTIMO_ORDINE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? FmQuantitaUltimoOrdine  { get; set; }

[Display(Name = "Data Prossimo Ordine", ShortName="", Description = "Data prevista per il prossimo ordine", Prompt="")]
[ErpDogField("FM_DATA_PROSSIMO_ORDINE", SqlFieldNameExt="FM_DATA_PROSSIMO_ORDINE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange FmDataProssimoOrdine  { get; set; } = new DateRange();

[Display(Name = "Quantita Media Ordine", ShortName="", Description = "Quantità media per ordine", Prompt="")]
[ErpDogField("FM_QUANTITA_MEDIA_ORDINE", SqlFieldNameExt="FM_QUANTITA_MEDIA_ORDINE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? FmQuantitaMediaOrdine  { get; set; }

[Display(Name = "Riferimenti Fornitore", ShortName="", Description = "Riferimento per il fornitore", Prompt="")]
[ErpDogField("FM_RIFERIMENTI_FORNITORE", SqlFieldNameExt="FM_RIFERIMENTI_FORNITORE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? FmRiferimentiFornitore  { get; set; }

[Display(Name = "Codice Nazionale", ShortName="", Description = "Codice nazionale", Prompt="")]
[ErpDogField("FM_CODICE_NAZIONALE", SqlFieldNameExt="FM_CODICE_NAZIONALE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? FmCodiceNazionale  { get; set; }

[Display(Name = "Codice Aic", ShortName="", Description = "Codice ministeriale in base 10", Prompt="")]
[ErpDogField("FM_CODICE_AIC", SqlFieldNameExt="FM_CODICE_AIC", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? FmCodiceAic  { get; set; }

[Display(Name = "Data Inizio Autorizzazione", ShortName="", Description = "Data di inizio dell'autorizzazione governativa", Prompt="")]
[ErpDogField("FM_DATA_INIZIO_AUTORIZZAZIONE", SqlFieldNameExt="FM_DATA_INIZIO_AUTORIZZAZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange FmDataInizioAutorizzazione  { get; set; } = new DateRange();

[Display(Name = "Data Fine Autorizzazione", ShortName="", Description = "Data di fine dell'autorizzazione governativa", Prompt="")]
[ErpDogField("FM_DATA_FINE_AUTORIZZAZIONE", SqlFieldNameExt="FM_DATA_FINE_AUTORIZZAZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange FmDataFineAutorizzazione  { get; set; } = new DateRange();
}
}
