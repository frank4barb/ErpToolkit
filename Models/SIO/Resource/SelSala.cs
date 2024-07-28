using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Resource {
public class SelSala {
public const string Description = "Risorse: località ... intcode:[94] prefix:[SA_] has_xdt:[SA_XDATA] is_xdt:[0] ";
public const string SqlTableName = "SALA";
public const string SqlTableNameExt = "SALA";
public const string SqlRowIdName = "SA__ID";
public const string SqlRowIdNameExt = "SA__ICODE";
public const string SqlPrefix = "SA_";
public const string SqlPrefixExt = "SA_";
public const string SqlXdataTableName = "SA_XDATA";
public const string SqlXdataTableNameExt = "SA_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 94; //Internal Table Code
public const string TBAREA = "Risorse"; //Table Area
public const string PREFIX = "Sa"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//127-124//REL_PRESTAZIONE_USA.PU_ID_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa> RelPrestazioneUsa4PuIdRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa>();
//1182-1179//REL_ATTIVITA_USA.AU_ID_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa> RelAttivitaUsa4AuIdRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa>();

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("SA_CODICE", SqlFieldNameExt="SA_CODICE", SqlFieldProperties="prop() xref() xdup(SALA.SA__ICODE[SA__ICODE] {SA_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? SaCodice  { get; set; }

[Display(Name = "Classe Risorsa", ShortName="", Description = "Classe di risorse: L[ocations] (Località)", Prompt="")]
[ErpDogField("SA_CLASSE_RISORSA", SqlFieldNameExt="SA_CLASSE_RISORSA", SqlFieldProperties="prop() xref() xdup(TIPO_RISORSA.TS_CLASSE_RISORSA[SALA.SA_ID_TIPO_RISORSA]) multbxref()")]
[DefaultValue("L")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? SaClasseRisorsa  { get; set; }

[Display(Name = "Id Tipo Risorsa", ShortName="", Description = "Codice del tipo di località", Prompt="")]
[ErpDogField("SA_ID_TIPO_RISORSA", SqlFieldNameExt="SA_ID_TIPO_RISORSA", SqlFieldProperties="prop() xref(TIPO_RISORSA.TS__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoRisorsa", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SaIdTipoRisorsa  { get; set; } = new List<string>();

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("SA_DESCRIZIONE", SqlFieldNameExt="SA_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? SaDescrizione  { get; set; }

[Display(Name = "Costo Unitario Uso", ShortName="", Description = "Costo unitario per l'utilizzo", Prompt="")]
[ErpDogField("SA_COSTO_UNITARIO_USO", SqlFieldNameExt="SA_COSTO_UNITARIO_USO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? SaCostoUnitarioUso  { get; set; }

[Display(Name = "Misura Unitaria Uso", ShortName="", Description = "Unità di misura per l'utilizzo", Prompt="")]
[ErpDogField("SA_MISURA_UNITARIA_USO", SqlFieldNameExt="SA_MISURA_UNITARIA_USO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? SaMisuraUnitariaUso  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("SA_NOTE", SqlFieldNameExt="SA_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? SaNote  { get; set; }

[Display(Name = "Disponibilita", ShortName="", Description = "Descrizione testuale dello stato attuale di disponibilità", Prompt="")]
[ErpDogField("SA_DISPONIBILITA", SqlFieldNameExt="SA_DISPONIBILITA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(20, ErrorMessage = "Inserire massimo 20 caratteri")]
[DataType(DataType.Text)]
public string? SaDisponibilita  { get; set; }

[Display(Name = "Telefono Fornitore", ShortName="", Description = "Numero di telefono", Prompt="")]
[ErpDogField("SA_TELEFONO_FORNITORE", SqlFieldNameExt="SA_TELEFONO_FORNITORE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(15, ErrorMessage = "Inserire massimo 15 caratteri")]
[DataType(DataType.Text)]
public string? SaTelefonoFornitore  { get; set; }

[Display(Name = "Data Ultima Manutenzione", ShortName="", Description = "Data dell'ultima manutenzione", Prompt="")]
[ErpDogField("SA_DATA_ULTIMA_MANUTENZIONE", SqlFieldNameExt="SA_DATA_ULTIMA_MANUTENZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SaDataUltimaManutenzione  { get; set; } = new DateRange();

[Display(Name = "Frequenza Manutenzione", ShortName="", Description = "Frequenza della manutenzione periodica [numero di ore di funzionamento]", Prompt="")]
[ErpDogField("SA_FREQUENZA_MANUTENZIONE", SqlFieldNameExt="SA_FREQUENZA_MANUTENZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? SaFrequenzaManutenzione  { get; set; }

[Display(Name = "Uso Medio Giornaliero", ShortName="", Description = "Numero medio di ore effettive di lavoro al giorno", Prompt="")]
[ErpDogField("SA_USO_MEDIO_GIORNALIERO", SqlFieldNameExt="SA_USO_MEDIO_GIORNALIERO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? SaUsoMedioGiornaliero  { get; set; }

[Display(Name = "Data Prossima Manutenzione", ShortName="", Description = "Data della prossima manutenzione prevista", Prompt="")]
[ErpDogField("SA_DATA_PROSSIMA_MANUTENZIONE", SqlFieldNameExt="SA_DATA_PROSSIMA_MANUTENZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SaDataProssimaManutenzione  { get; set; } = new DateRange();
}
}
