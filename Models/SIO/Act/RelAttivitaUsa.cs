using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class RelAttivitaUsa {
public const string Description = "Tipi e/o risorse individuali generalmente necessari per l'esecuzione di un'attività";
public const string SqlTableName = "REL_ATTIVITA_USA";
public const string SqlTableNameExt = "REL_ATTIVITA_USA";
public const string SqlRowIdName = "AU__ID";
public const string SqlRowIdNameExt = "AU__ICODE";
public const string SqlPrefix = "AU_";
public const string SqlPrefixExt = "AU_";
public const string SqlXdataTableName = "AU_XDATA";
public const string SqlXdataTableNameExt = "AU_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 21; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Au"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "Y"; //Is Relation Table: Yes or No
//1193-1179//REL_ATTIVITA_USA.AU_ID_GRUPPO
public List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa> RelAttivitaUsa4AuIdGruppo  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa>();
[Display(Name = "Au1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("AU__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Au1Ienv { get; set; }
[Key]
[Display(Name = "Au1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("AU__ICODE", SqlFieldNameExt="AU__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Au1Icode { get; set; }
[Display(Name = "Au1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("AU__DELETED", SqlFieldNameExt="AU__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Au1Deleted { get; set; }
[Display(Name = "Au1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("AU__TIMESTAMP", SqlFieldNameExt="AU__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Au1Timestamp { get; set; }
[Display(Name = "Au1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("AU__HOME", SqlFieldNameExt="AU__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Au1Home { get; set; }
[Display(Name = "Au1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("AU__VERSION", SqlFieldNameExt="AU__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Au1Version { get; set; }
[Display(Name = "Au1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("AU__INACTIVE", SqlFieldNameExt="AU__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Au1Inactive { get; set; }
[Display(Name = "Au1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("AU__EXTATT", SqlFieldNameExt="AU__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Au1Extatt { get; set; }


[Display(Name = "Id Attivita", ShortName="", Description = "Codice del tipo di attività", Prompt="")]
[ErpDogField("AU_ID_ATTIVITA", SqlFieldNameExt="AU_ID_ATTIVITA", SqlFieldProperties="prop() xref(ATTIVITA.AV__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[AutocompleteClient("Attivita", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? AuIdAttivita  { get; set; }
public ErpToolkit.Models.SIO.Act.Attivita? AuIdAttivitaObj  { get; set; }

[Display(Name = "Classe Risorsa", ShortName="", Description = "Classe di risorsa: E[quipments] (Attrezzature) - L[ocations] (Luoghi) - S[taff] (Personale) - M[aterial] (Materiali) - B[ed] (Letti)", Prompt="")]
[ErpDogField("AU_CLASSE_RISORSA", SqlFieldNameExt="AU_CLASSE_RISORSA", SqlFieldProperties="prop() xref() xdup(TIPO_RISORSA.TS_CLASSE_RISORSA[REL_ATTIVITA_USA.AU_ID_TIPO_RISORSA]) multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("E|L|M|D|S", ErrorMessage = "Inserisci una delle seguenti opzioni: E|L|M|D|S")]
public string? AuClasseRisorsa  { get; set; }

[Display(Name = "Id Tipo Risorsa", ShortName="", Description = "Codice del tipo di risorsa", Prompt="")]
[ErpDogField("AU_ID_TIPO_RISORSA", SqlFieldNameExt="AU_ID_TIPO_RISORSA", SqlFieldProperties="prop() xref(TIPO_RISORSA.TS__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[AutocompleteClient("TipoRisorsa", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? AuIdTipoRisorsa  { get; set; }
public ErpToolkit.Models.SIO.Resource.TipoRisorsa? AuIdTipoRisorsaObj  { get; set; }

[Display(Name = "Id Risorsa", ShortName="", Description = "Codice della risorsa individuale", Prompt="")]
[ErpDogField("AU_ID_RISORSA_S", SqlFieldNameExt="AU_ID_RISORSA_S", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE{AU_CLASSE_RISORSA='S'} | MATERIALE.MT__ICODE{AU_CLASSE_RISORSA='M'} | ATTREZZATURA.AT__ICODE{AU_CLASSE_RISORSA='E'} | SALA.SA__ICODE{AU_CLASSE_RISORSA='L'} | FARMACO.FM__ICODE{AU_CLASSE_RISORSA='D'}) xdup() multbxref(AU_CLASSE_RISORSA)")]
[DataType(DataType.Text)]
public string? AuIdRisorsaS  { get; set; }
public ErpToolkit.Models.SIO.Resource.Personale? AuIdRisorsaSObj  { get; set; }

[Display(Name = "Id Risorsa", ShortName="", Description = "Codice della risorsa individuale", Prompt="")]
[ErpDogField("AU_ID_RISORSA_M", SqlFieldNameExt="AU_ID_RISORSA_M", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE{AU_CLASSE_RISORSA='S'} | MATERIALE.MT__ICODE{AU_CLASSE_RISORSA='M'} | ATTREZZATURA.AT__ICODE{AU_CLASSE_RISORSA='E'} | SALA.SA__ICODE{AU_CLASSE_RISORSA='L'} | FARMACO.FM__ICODE{AU_CLASSE_RISORSA='D'}) xdup() multbxref(AU_CLASSE_RISORSA)")]
[DataType(DataType.Text)]
public string? AuIdRisorsaM  { get; set; }
public ErpToolkit.Models.SIO.Resource.Materiale? AuIdRisorsaMObj  { get; set; }

[Display(Name = "Id Risorsa", ShortName="", Description = "Codice della risorsa individuale", Prompt="")]
[ErpDogField("AU_ID_RISORSA_E", SqlFieldNameExt="AU_ID_RISORSA_E", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE{AU_CLASSE_RISORSA='S'} | MATERIALE.MT__ICODE{AU_CLASSE_RISORSA='M'} | ATTREZZATURA.AT__ICODE{AU_CLASSE_RISORSA='E'} | SALA.SA__ICODE{AU_CLASSE_RISORSA='L'} | FARMACO.FM__ICODE{AU_CLASSE_RISORSA='D'}) xdup() multbxref(AU_CLASSE_RISORSA)")]
[DataType(DataType.Text)]
public string? AuIdRisorsaE  { get; set; }
public ErpToolkit.Models.SIO.Resource.Attrezzatura? AuIdRisorsaEObj  { get; set; }

[Display(Name = "Id Risorsa", ShortName="", Description = "Codice della risorsa individuale", Prompt="")]
[ErpDogField("AU_ID_RISORSA_L", SqlFieldNameExt="AU_ID_RISORSA_L", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE{AU_CLASSE_RISORSA='S'} | MATERIALE.MT__ICODE{AU_CLASSE_RISORSA='M'} | ATTREZZATURA.AT__ICODE{AU_CLASSE_RISORSA='E'} | SALA.SA__ICODE{AU_CLASSE_RISORSA='L'} | FARMACO.FM__ICODE{AU_CLASSE_RISORSA='D'}) xdup() multbxref(AU_CLASSE_RISORSA)")]
[DataType(DataType.Text)]
public string? AuIdRisorsaL  { get; set; }
public ErpToolkit.Models.SIO.Resource.Sala? AuIdRisorsaLObj  { get; set; }

[Display(Name = "Id Risorsa", ShortName="", Description = "Codice della risorsa individuale", Prompt="")]
[ErpDogField("AU_ID_RISORSA_D", SqlFieldNameExt="AU_ID_RISORSA_D", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE{AU_CLASSE_RISORSA='S'} | MATERIALE.MT__ICODE{AU_CLASSE_RISORSA='M'} | ATTREZZATURA.AT__ICODE{AU_CLASSE_RISORSA='E'} | SALA.SA__ICODE{AU_CLASSE_RISORSA='L'} | FARMACO.FM__ICODE{AU_CLASSE_RISORSA='D'}) xdup() multbxref(AU_CLASSE_RISORSA)")]
[DataType(DataType.Text)]
public string? AuIdRisorsaD  { get; set; }
public ErpToolkit.Models.SIO.Resource.Farmaco? AuIdRisorsaDObj  { get; set; }

[Display(Name = "Id Risorsa", ShortName="", Description = "Codice della risorsa individuale", Prompt="")]
[ErpDogField("AU_ID_RISORSA", SqlFieldNameExt="AU_ID_RISORSA", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE{AU_CLASSE_RISORSA='S'} | MATERIALE.MT__ICODE{AU_CLASSE_RISORSA='M'} | ATTREZZATURA.AT__ICODE{AU_CLASSE_RISORSA='E'} | SALA.SA__ICODE{AU_CLASSE_RISORSA='L'} | FARMACO.FM__ICODE{AU_CLASSE_RISORSA='D'}) xdup() multbxref(AU_CLASSE_RISORSA)")]
public string? AuIdRisorsa  { get; set; }

[Display(Name = "Sequenza", ShortName="", Description = "Numero di sequenza della relazione", Prompt="")]
[ErpDogField("AU_SEQUENZA", SqlFieldNameExt="AU_SEQUENZA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? AuSequenza  { get; set; }

[Display(Name = "Quantita Media Usata", ShortName="", Description = "Quantità media utilizzata", Prompt="")]
[ErpDogField("AU_QUANTITA_MEDIA_USATA", SqlFieldNameExt="AU_QUANTITA_MEDIA_USATA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? AuQuantitaMediaUsata  { get; set; }

[Display(Name = "Quantita Extra", ShortName="", Description = "Quantità extra da considerare", Prompt="")]
[ErpDogField("AU_QUANTITA_EXTRA", SqlFieldNameExt="AU_QUANTITA_EXTRA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? AuQuantitaExtra  { get; set; }

[Display(Name = "Unita Di Misura", ShortName="", Description = "Unità di misura", Prompt="")]
[ErpDogField("AU_UNITA_DI_MISURA", SqlFieldNameExt="AU_UNITA_DI_MISURA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? AuUnitaDiMisura  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note brevi", Prompt="")]
[ErpDogField("AU_NOTE", SqlFieldNameExt="AU_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(80, ErrorMessage = "Inserire massimo 80 caratteri")]
[DataType(DataType.Text)]
public string? AuNote  { get; set; }

[Display(Name = "Costo Medio", ShortName="", Description = "Costo medio di tale utilizzo", Prompt="")]
[ErpDogField("AU_COSTO_MEDIO", SqlFieldNameExt="AU_COSTO_MEDIO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? AuCostoMedio  { get; set; }

[Display(Name = "Descrizione Risorsa Usata", ShortName="", Description = "Descrizione testuale delle risorse utilizzate", Prompt="")]
[ErpDogField("AU_DESCRIZIONE_RISORSA_USATA", SqlFieldNameExt="AU_DESCRIZIONE_RISORSA_USATA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? AuDescrizioneRisorsaUsata  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Identificatore dell'istanza per la quale questa specifica rappresenta un'opzione (se applicabile)", Prompt="")]
[ErpDogField("AU_ID_GRUPPO", SqlFieldNameExt="AU_ID_GRUPPO", SqlFieldProperties="prop() xref(REL_ATTIVITA_USA.AU__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("RelAttivitaUsa", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? AuIdGruppo  { get; set; }
public ErpToolkit.Models.SIO.Act.RelAttivitaUsa? AuIdGruppoObj  { get; set; }
}
}
