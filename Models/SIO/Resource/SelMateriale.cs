using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Resource {
public class SelMateriale {
public const string Description = "Risorse: materiali";
public const string SqlTableName = "MATERIALE";
public const string SqlTableNameExt = "MATERIALE";
public const string SqlRowIdName = "MT__ID";
public const string SqlRowIdNameExt = "MT__ICODE";
public const string SqlPrefix = "MT_";
public const string SqlPrefixExt = "MT_";
public const string SqlXdataTableName = "MT_XDATA";
public const string SqlXdataTableNameExt = "MT_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 97; //Internal Table Code
public const string TBAREA = "Risorse"; //Table Area
public const string PREFIX = "Mt"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//127-124//REL_PRESTAZIONE_USA.PU_ID_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa> RelPrestazioneUsa4PuIdRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa>();
//1182-1179//REL_ATTIVITA_USA.AU_ID_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa> RelAttivitaUsa4AuIdRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa>();

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("MT_CODICE", SqlFieldNameExt="MT_CODICE", SqlFieldProperties="prop() xref() xdup(MATERIALE.MT__ICODE[MT__ICODE] {MT_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? MtCodice  { get; set; }

[Display(Name = "Classe Risorsa", ShortName="", Description = "Classe di risorsa: M[aterial] (Materiale)", Prompt="")]
[ErpDogField("MT_CLASSE_RISORSA", SqlFieldNameExt="MT_CLASSE_RISORSA", SqlFieldProperties="prop() xref() xdup(TIPO_RISORSA.TS_CLASSE_RISORSA[MATERIALE.MT_ID_TIPO_RISORSA]) multbxref()")]
[DefaultValue("M")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? MtClasseRisorsa  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("MT_DESCRIZIONE", SqlFieldNameExt="MT_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? MtDescrizione  { get; set; }

[Display(Name = "Id Tipo Risorsa", ShortName="", Description = "Codice del tipo di materiale, secondo la classificazione operativa definita in A_TYRESOUR con radice #SIO#MATER", Prompt="")]
[ErpDogField("MT_ID_TIPO_RISORSA", SqlFieldNameExt="MT_ID_TIPO_RISORSA", SqlFieldProperties="prop() xref(TIPO_RISORSA.TS__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoRisorsa", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> MtIdTipoRisorsa  { get; set; } = new List<string>();

[Display(Name = "Costo Unitario Uso", ShortName="", Description = "Costo unitario per l'utilizzo", Prompt="")]
[ErpDogField("MT_COSTO_UNITARIO_USO", SqlFieldNameExt="MT_COSTO_UNITARIO_USO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? MtCostoUnitarioUso  { get; set; }

[Display(Name = "Misura Unitaria Uso", ShortName="", Description = "Unità di misura per l'utilizzo", Prompt="")]
[ErpDogField("MT_MISURA_UNITARIA_USO", SqlFieldNameExt="MT_MISURA_UNITARIA_USO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? MtMisuraUnitariaUso  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("MT_NOTE", SqlFieldNameExt="MT_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? MtNote  { get; set; }

[Display(Name = "Disponibilita", ShortName="", Description = "Descrizione testuale dei criteri di disponibilità/consegna", Prompt="")]
[ErpDogField("MT_DISPONIBILITA", SqlFieldNameExt="MT_DISPONIBILITA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(20, ErrorMessage = "Inserire massimo 20 caratteri")]
[DataType(DataType.Text)]
public string? MtDisponibilita  { get; set; }

[Display(Name = "Telefono Fornitore", ShortName="", Description = "Numero di telefono del fornitore", Prompt="")]
[ErpDogField("MT_TELEFONO_FORNITORE", SqlFieldNameExt="MT_TELEFONO_FORNITORE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
[StringLength(15, ErrorMessage = "Inserire massimo 15 caratteri")]
[DataType(DataType.Text)]
public string? MtTelefonoFornitore  { get; set; }

[Display(Name = "Quantita Disponibile", ShortName="", Description = "Quantità attualmente disponibile", Prompt="")]
[ErpDogField("MT_QUANTITA_DISPONIBILE", SqlFieldNameExt="MT_QUANTITA_DISPONIBILE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? MtQuantitaDisponibile  { get; set; }

[Display(Name = "Quantita Minima Magazzino", ShortName="", Description = "Quantità minima richiesta", Prompt="")]
[ErpDogField("MT_QUANTITA_MINIMA_MAGAZZINO", SqlFieldNameExt="MT_QUANTITA_MINIMA_MAGAZZINO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? MtQuantitaMinimaMagazzino  { get; set; }

[Display(Name = "Uso Medio Giornaliero", ShortName="", Description = "Utilizzo medio al giorno (numero di unità)", Prompt="")]
[ErpDogField("MT_USO_MEDIO_GIORNALIERO", SqlFieldNameExt="MT_USO_MEDIO_GIORNALIERO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? MtUsoMedioGiornaliero  { get; set; }

[Display(Name = "Data Ultimo Ordine", ShortName="", Description = "Data dell'ultimo ordine", Prompt="")]
[ErpDogField("MT_DATA_ULTIMO_ORDINE", SqlFieldNameExt="MT_DATA_ULTIMO_ORDINE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange MtDataUltimoOrdine  { get; set; } = new DateRange();

[Display(Name = "Quantita Ultimo Ordine", ShortName="", Description = "Quantità dell'ultimo ordine", Prompt="")]
[ErpDogField("MT_QUANTITA_ULTIMO_ORDINE", SqlFieldNameExt="MT_QUANTITA_ULTIMO_ORDINE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? MtQuantitaUltimoOrdine  { get; set; }

[Display(Name = "Data Prossimo Ordine", ShortName="", Description = "Data prevista per il prossimo ordine", Prompt="")]
[ErpDogField("MT_DATA_PROSSIMO_ORDINE", SqlFieldNameExt="MT_DATA_PROSSIMO_ORDINE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange MtDataProssimoOrdine  { get; set; } = new DateRange();

[Display(Name = "Quantita Media Ordine", ShortName="", Description = "Quantità media per ordine", Prompt="")]
[ErpDogField("MT_QUANTITA_MEDIA_ORDINE", SqlFieldNameExt="MT_QUANTITA_MEDIA_ORDINE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? MtQuantitaMediaOrdine  { get; set; }

[Display(Name = "Codice Nazionale", ShortName="", Description = "Codice nazionale", Prompt="")]
[ErpDogField("MT_CODICE_NAZIONALE", SqlFieldNameExt="MT_CODICE_NAZIONALE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(20, ErrorMessage = "Inserire massimo 20 caratteri")]
[DataType(DataType.Text)]
public string? MtCodiceNazionale  { get; set; }
}
}
