using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Resource {
public class Attrezzatura {
public const string Description = "Risorse: attrezzature ... intcode:[96] prefix:[AT_] has_xdt:[AT_XDATA] is_xdt:[0] ";
public const string SqlTableName = "ATTREZZATURA";
public const string SqlTableNameExt = "ATTREZZATURA";
public const string SqlRowIdName = "AT__ID";
public const string SqlRowIdNameExt = "AT__ICODE";
public const string SqlPrefix = "AT_";
public const string SqlPrefixExt = "AT_";
public const string SqlXdataTableName = "AT_XDATA";
public const string SqlXdataTableNameExt = "AT_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 96; //Internal Table Code
public const string TBAREA = "Risorse"; //Table Area
public const string PREFIX = "At"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//127-124//REL_PRESTAZIONE_USA.PU_ID_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa> RelPrestazioneUsa4PuIdRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa>();
//1182-1179//REL_ATTIVITA_USA.AU_ID_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa> RelAttivitaUsa4AuIdRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa>();
[Display(Name = "At1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("AT__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? At1Ienv { get; set; }
[Key]
[Display(Name = "At1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("AT__ICODE", SqlFieldNameExt="AT__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? At1Icode { get; set; }
[Display(Name = "At1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("AT__DELETED", SqlFieldNameExt="AT__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? At1Deleted { get; set; }
[Display(Name = "At1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("AT__TIMESTAMP", SqlFieldNameExt="AT__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? At1Timestamp { get; set; }
[Display(Name = "At1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("AT__HOME", SqlFieldNameExt="AT__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? At1Home { get; set; }
[Display(Name = "At1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("AT__VERSION", SqlFieldNameExt="AT__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? At1Version { get; set; }
[Display(Name = "At1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("AT__INACTIVE", SqlFieldNameExt="AT__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? At1Inactive { get; set; }
[Display(Name = "At1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("AT__EXTATT", SqlFieldNameExt="AT__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? At1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("AT_CODICE", SqlFieldNameExt="AT_CODICE", SqlFieldProperties="prop() xref() xdup(ATTREZZATURA.AT__ICODE[AT__ICODE] {AT_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? AtCodice  { get; set; }

[Display(Name = "Classe Risorsa", ShortName="", Description = "Classe di risorsa E[quipaggiamenti]", Prompt="")]
[ErpDogField("AT_CLASSE_RISORSA", SqlFieldNameExt="AT_CLASSE_RISORSA", SqlFieldProperties="prop() xref() xdup(TIPO_RISORSA.TS_CLASSE_RISORSA[ATTREZZATURA.AT_ID_TIPO_RISORSA]) multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("E")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? AtClasseRisorsa  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("AT_DESCRIZIONE", SqlFieldNameExt="AT_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? AtDescrizione  { get; set; }

[Display(Name = "Id Tipo Risorsa", ShortName="", Description = "Codice del tipo di attrezzatura", Prompt="")]
[ErpDogField("AT_ID_TIPO_RISORSA", SqlFieldNameExt="AT_ID_TIPO_RISORSA", SqlFieldProperties="prop() xref(TIPO_RISORSA.TS__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[AutocompleteClient("TipoRisorsa", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? AtIdTipoRisorsa  { get; set; }
public ErpToolkit.Models.SIO.Resource.TipoRisorsa? AtIdTipoRisorsaObj  { get; set; }

[Display(Name = "Costo Unitario Uso", ShortName="", Description = "Costo unitario per l'utilizzo", Prompt="")]
[ErpDogField("AT_COSTO_UNITARIO_USO", SqlFieldNameExt="AT_COSTO_UNITARIO_USO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? AtCostoUnitarioUso  { get; set; }

[Display(Name = "Misura Unitaria Uso", ShortName="", Description = "Unità di misura per l'utilizzo", Prompt="")]
[ErpDogField("AT_MISURA_UNITARIA_USO", SqlFieldNameExt="AT_MISURA_UNITARIA_USO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? AtMisuraUnitariaUso  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("AT_NOTE", SqlFieldNameExt="AT_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? AtNote  { get; set; }

[Display(Name = "Disponibilita", ShortName="", Description = "Descrizione testuale dello stato attuale di disponibilità", Prompt="")]
[ErpDogField("AT_DISPONIBILITA", SqlFieldNameExt="AT_DISPONIBILITA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(20, ErrorMessage = "Inserire massimo 20 caratteri")]
[DataType(DataType.Text)]
public string? AtDisponibilita  { get; set; }

[Display(Name = "Telefono Fornitore", ShortName="", Description = "Numero di telefono collegato all'attrezzatura", Prompt="")]
[ErpDogField("AT_TELEFONO_FORNITORE", SqlFieldNameExt="AT_TELEFONO_FORNITORE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(15, ErrorMessage = "Inserire massimo 15 caratteri")]
[DataType(DataType.Text)]
public string? AtTelefonoFornitore  { get; set; }

[Display(Name = "Numero Seriale", ShortName="", Description = "Numero di serie", Prompt="")]
[ErpDogField("AT_NUMERO_SERIALE", SqlFieldNameExt="AT_NUMERO_SERIALE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(20, ErrorMessage = "Inserire massimo 20 caratteri")]
[DataType(DataType.Text)]
public string? AtNumeroSeriale  { get; set; }

[Display(Name = "Riferimenti Assistenza", ShortName="", Description = "Riferimento al fornitore per l'assistenza", Prompt="")]
[ErpDogField("AT_RIFERIMENTI_ASSISTENZA", SqlFieldNameExt="AT_RIFERIMENTI_ASSISTENZA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(80, ErrorMessage = "Inserire massimo 80 caratteri")]
[DataType(DataType.Text)]
public string? AtRiferimentiAssistenza  { get; set; }

[Display(Name = "Telefono Assistenza", ShortName="", Description = "Numero di telefono per l'assistenza", Prompt="")]
[ErpDogField("AT_TELEFONO_ASSISTENZA", SqlFieldNameExt="AT_TELEFONO_ASSISTENZA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(15, ErrorMessage = "Inserire massimo 15 caratteri")]
[DataType(DataType.Text)]
public string? AtTelefonoAssistenza  { get; set; }

[Display(Name = "Data Ultima Manutenzione", ShortName="", Description = "Data dell'ultimo intervento di manutenzione", Prompt="")]
[ErpDogField("AT_DATA_ULTIMA_MANUTENZIONE", SqlFieldNameExt="AT_DATA_ULTIMA_MANUTENZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public string? AtDataUltimaManutenzione  { get; set; }

[Display(Name = "Frequenza Manutenzione", ShortName="", Description = "Frequenza della manutenzione periodica [numero di ore di funzionamento]", Prompt="")]
[ErpDogField("AT_FREQUENZA_MANUTENZIONE", SqlFieldNameExt="AT_FREQUENZA_MANUTENZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? AtFrequenzaManutenzione  { get; set; }

[Display(Name = "Uso Medio Giornaliero", ShortName="", Description = "Numero medio di ore effettive di lavoro al giorno", Prompt="")]
[ErpDogField("AT_USO_MEDIO_GIORNALIERO", SqlFieldNameExt="AT_USO_MEDIO_GIORNALIERO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? AtUsoMedioGiornaliero  { get; set; }

[Display(Name = "Data Prossima Manutenzione", ShortName="", Description = "Data della prossima manutenzione prevedibile", Prompt="")]
[ErpDogField("AT_DATA_PROSSIMA_MANUTENZIONE", SqlFieldNameExt="AT_DATA_PROSSIMA_MANUTENZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public string? AtDataProssimaManutenzione  { get; set; }
}
}
