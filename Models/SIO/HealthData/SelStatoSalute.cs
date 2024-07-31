using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.HealthData {
public class SelStatoSalute {
public const string Description = "Dati sanitari - Condizioni di salute generiche";
public const string SqlTableName = "STATO_SALUTE";
public const string SqlTableNameExt = "STATO_SALUTE";
public const string SqlRowIdName = "SS__ID";
public const string SqlRowIdNameExt = "SS__ICODE";
public const string SqlPrefix = "SS_";
public const string SqlPrefixExt = "SS_";
public const string SqlXdataTableName = "SS_XDATA";
public const string SqlXdataTableNameExt = "SS_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 40; //Internal Table Code
public const string TBAREA = "Dati clinici"; //Table Area
public const string PREFIX = "Ss"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//1026-1025//REL_PRESTAZIONE_DATO_CLINICO.PD_ID_DATO_CLINICO
public List<ErpToolkit.Models.SIO.HealthData.RelPrestazioneDatoClinico> RelPrestazioneDatoClinico4PdIdDatoClinico  { get; set; } = new List<ErpToolkit.Models.SIO.HealthData.RelPrestazioneDatoClinico>();

[Display(Name = "Classe", ShortName="", Description = "Classe del dato sanitario: 3: condizioni di salute generiche", Prompt="")]
[ErpDogField("SS_CLASSE", SqlFieldNameExt="SS_CLASSE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("3")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? SsClasse  { get; set; }

[Display(Name = "Id Paziente", ShortName="", Description = "Codice del paziente a cui si riferisce il dato sanitario", Prompt="")]
[ErpDogField("SS_ID_PAZIENTE", SqlFieldNameExt="SS_ID_PAZIENTE", SqlFieldProperties="prop() xref(PAZIENTE.PA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Paziente", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> SsIdPaziente  { get; set; } = new List<string>();

[Display(Name = "Id Episodio", ShortName="", Description = "Codice del contatto a cui il Dato Sanitario si riferisce", Prompt="")]
[ErpDogField("SS_ID_EPISODIO", SqlFieldNameExt="SS_ID_EPISODIO", SqlFieldProperties="prop() xref(EPISODIO.EP__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Episodio", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> SsIdEpisodio  { get; set; } = new List<string>();

[Display(Name = "Id Tipo Dato Clinico", ShortName="", Description = "Codice del tipo di Dato Sanitario", Prompt="")]
[ErpDogField("SS_ID_TIPO_DATO_CLINICO", SqlFieldNameExt="SS_ID_TIPO_DATO_CLINICO", SqlFieldProperties="prop() xref(TIPO_DATO_CLINICO.TC__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SsIdTipoDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Id Gruppo Dato Clinico", ShortName="", Description = "Classe del tipo di dato sanitario", Prompt="")]
[ErpDogField("SS_ID_GRUPPO_DATO_CLINICO", SqlFieldNameExt="SS_ID_GRUPPO_DATO_CLINICO", SqlFieldProperties="prop() xref(CATEGORIA_DATO_CLINICO.CC__ICODE) xdup(TIPO_DATO_CLINICO.TC_ID_CATEGORIA_DATO_CLINICO[STATO_SALUTE.SS_ID_TIPO_DATO_CLINICO]) multbxref()")]
[DefaultValue("")]
[AutocompleteClient("CategoriaDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SsIdGruppoDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Valore Minimo", ShortName="", Description = "Valori numerici minimi (se applicabile)", Prompt="")]
[ErpDogField("SS_VALORE_MINIMO", SqlFieldNameExt="SS_VALORE_MINIMO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? SsValoreMinimo  { get; set; }

[Display(Name = "Valore Massimo", ShortName="", Description = "Valori numerici massimi (se applicabile)", Prompt="")]
[ErpDogField("SS_VALORE_MASSIMO", SqlFieldNameExt="SS_VALORE_MASSIMO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? SsValoreMassimo  { get; set; }

[Display(Name = "Valore Scelta", ShortName="", Description = "Valore carattere [se applicabile, in base al tipo di risultato]", Prompt="")]
[ErpDogField("SS_VALORE_SCELTA", SqlFieldNameExt="SS_VALORE_SCELTA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(80, ErrorMessage = "Inserire massimo 80 caratteri")]
[DataType(DataType.Text)]
public string? SsValoreScelta  { get; set; }

[Display(Name = "Valore Testo", ShortName="", Description = "Valore testuale, se applicabile", Prompt="")]
[ErpDogField("SS_VALORE_TESTO", SqlFieldNameExt="SS_VALORE_TESTO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(4000, ErrorMessage = "Inserire massimo 4000 caratteri")]
[DataType(DataType.Text)]
public string? SsValoreTesto  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note [se applicabile, in base al tipo di risultato]", Prompt="")]
[ErpDogField("SS_NOTE", SqlFieldNameExt="SS_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(80, ErrorMessage = "Inserire massimo 80 caratteri")]
[DataType(DataType.Text)]
public string? SsNote  { get; set; }

[Display(Name = "Codice Referto", ShortName="", Description = "Criterio di codifica/unità di misura adottato (se applicabile)", Prompt="")]
[ErpDogField("SS_CODICE_REFERTO", SqlFieldNameExt="SS_CODICE_REFERTO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? SsCodiceReferto  { get; set; }

[Display(Name = "Data Acquisizione", ShortName="", Description = "Data di acquisizione del dato sanitario", Prompt="")]
[ErpDogField("SS_DATA_ACQUISIZIONE", SqlFieldNameExt="SS_DATA_ACQUISIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SsDataAcquisizione  { get; set; } = new DateRange();

[Display(Name = "Ora Acquisizione", ShortName="", Description = "Ora di acquisizione del dato sanitario", Prompt="")]
[ErpDogField("SS_ORA_ACQUISIZIONE", SqlFieldNameExt="SS_ORA_ACQUISIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? SsOraAcquisizione  { get; set; }

[Display(Name = "Stato Dato Clinico", ShortName="", Description = "Stato del dato: P[reliminare] - C[onfermato] - A[nnullato]", Prompt="")]
[ErpDogField("SS_STATO_DATO_CLINICO", SqlFieldNameExt="SS_STATO_DATO_CLINICO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("P")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("P|C|A", ErrorMessage = "Inserisci una delle seguenti opzioni: P|C|A")]
public string? SsStatoDatoClinico  { get; set; }

[Display(Name = "Data Validazione", ShortName="", Description = "Data di convalida del dato sanitario", Prompt="")]
[ErpDogField("SS_DATA_VALIDAZIONE", SqlFieldNameExt="SS_DATA_VALIDAZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SsDataValidazione  { get; set; } = new DateRange();

[Display(Name = "Ora Validazione", ShortName="", Description = "Ora di convalida del dato sanitario", Prompt="")]
[ErpDogField("SS_ORA_VALIDAZIONE", SqlFieldNameExt="SS_ORA_VALIDAZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? SsOraValidazione  { get; set; }

[Display(Name = "Sequenza", ShortName="", Description = "Numero di sequenza del dato nel report originale", Prompt="")]
[ErpDogField("SS_SEQUENZA", SqlFieldNameExt="SS_SEQUENZA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? SsSequenza  { get; set; }
}
}
