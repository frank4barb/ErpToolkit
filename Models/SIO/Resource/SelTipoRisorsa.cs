using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Resource {
public class SelTipoRisorsa {
public const string Description = "Tipi di risorse disponibili/utilizzate nell'organizzazione sanitaria";
public const string SqlTableName = "TIPO_RISORSA";
public const string SqlTableNameExt = "TIPO_RISORSA";
public const string SqlRowIdName = "TS__ID";
public const string SqlRowIdNameExt = "TS__ICODE";
public const string SqlPrefix = "TS_";
public const string SqlPrefixExt = "TS_";
public const string SqlXdataTableName = "TS_XDATA";
public const string SqlXdataTableNameExt = "TS_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 20; //Internal Table Code
public const string TBAREA = "Risorse"; //Table Area
public const string PREFIX = "Ts"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//126-124//REL_PRESTAZIONE_USA.PU_ID_TIPO_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa> RelPrestazioneUsa4PuIdTipoRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa>();
//1181-1179//REL_ATTIVITA_USA.AU_ID_TIPO_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa> RelAttivitaUsa4AuIdTipoRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa>();

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TS_CODICE", SqlFieldNameExt="TS_CODICE", SqlFieldProperties="prop() xref() xdup(TIPO_RISORSA.TS__ICODE[TS__ICODE] {TS_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TsCodice  { get; set; }

[Display(Name = "Classe Risorsa", ShortName="", Description = "Classe: E[quipments] - L[ocations] - S[taff] - M[aterial] - [G]eneric", Prompt="")]
[ErpDogField("TS_CLASSE_RISORSA", SqlFieldNameExt="TS_CLASSE_RISORSA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("M")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("E|L|M|D|S|G", ErrorMessage = "Inserisci una delle seguenti opzioni: E|L|M|D|S|G")]
public string? TsClasseRisorsa  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Codice del super-tipo di risorsa (cioè l'aggregazione nella gerarchia), se presente", Prompt="")]
[ErpDogField("TS_ID_GRUPPO", SqlFieldNameExt="TS_ID_GRUPPO", SqlFieldProperties="prop() xref(TIPO_RISORSA.TS__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoRisorsa", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> TsIdGruppo  { get; set; } = new List<string>();

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TS_DESCRIZIONE", SqlFieldNameExt="TS_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TsDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TS_NOTE", SqlFieldNameExt="TS_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TsNote  { get; set; }

[Display(Name = "Unita Di Misura", ShortName="", Description = "Unità di misura", Prompt="")]
[ErpDogField("TS_UNITA_DI_MISURA", SqlFieldNameExt="TS_UNITA_DI_MISURA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TsUnitaDiMisura  { get; set; }
}
}
