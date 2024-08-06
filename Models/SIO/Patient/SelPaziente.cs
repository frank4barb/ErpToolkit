using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class SelPaziente {
public const string Description = "Pazienti rilevanti per l'organizzazione sanitaria";
public const string SqlTableName = "PAZIENTE";
public const string SqlTableNameExt = "PAZIENTE";
public const string SqlRowIdName = "PA__ID";
public const string SqlRowIdNameExt = "PA__ICODE";
public const string SqlPrefix = "PA_";
public const string SqlPrefixExt = "PA_";
public const string SqlXdataTableName = "PA_XDATA";
public const string SqlXdataTableNameExt = "PA_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 51; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Pa"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Cod Fiscale", ShortName="", Description = "Identificatore nazionale del paziente/individuo", Prompt="")]
[ErpDogField("PA_COD_FISCALE", SqlFieldNameExt="PA_COD_FISCALE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaCodFiscale  { get; set; }

[Display(Name = "Cod Sanitario", ShortName="", Description = "Identificatore permanente del paziente nell'organizzazione sanitaria", Prompt="")]
[ErpDogField("PA_COD_SANITARIO", SqlFieldNameExt="PA_COD_SANITARIO", SqlFieldProperties="prop() xref() xdup(PAZIENTE.PA__ICODE[PA__ICODE] {PA_COD_SANITARIO=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? PaCodSanitario  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Nome del paziente", Prompt="")]
[ErpDogField("PA_NOME", SqlFieldNameExt="PA_NOME", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaNome  { get; set; }

[Display(Name = "Cognome", ShortName="", Description = "Cognome del paziente", Prompt="")]
[ErpDogField("PA_COGNOME", SqlFieldNameExt="PA_COGNOME", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaCognome  { get; set; }

[Display(Name = "Sesso", ShortName="", Description = "Sesso M / F / N", Prompt="")]
[ErpDogField("PA_SESSO", SqlFieldNameExt="PA_SESSO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("M|F|N", ErrorMessage = "Inserisci una delle seguenti opzioni: M|F|N")]
public string? PaSesso  { get; set; }

[Display(Name = "Data Nascita", ShortName="", Description = "Data di nascita", Prompt="")]
[ErpDogField("PA_DATA_NASCITA", SqlFieldNameExt="PA_DATA_NASCITA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange PaDataNascita  { get; set; } = new DateRange();

[Display(Name = "Ora Nascita", ShortName="", Description = "Ora di nascita", Prompt="")]
[ErpDogField("PA_ORA_NASCITA", SqlFieldNameExt="PA_ORA_NASCITA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? PaOraNascita  { get; set; }

[Display(Name = "Id Comune Nascita", ShortName="", Description = "Codice del comune di nascita", Prompt="")]
[ErpDogField("PA_ID_COMUNE_NASCITA", SqlFieldNameExt="PA_ID_COMUNE_NASCITA", SqlFieldProperties="prop() xref(COMUNE.CM__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Comune", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PaIdComuneNascita  { get; set; } = new List<string>();

[Display(Name = "Id Nazione Nascita", ShortName="", Description = "Codice del paese di nascita", Prompt="")]
[ErpDogField("PA_ID_NAZIONE_NASCITA", SqlFieldNameExt="PA_ID_NAZIONE_NASCITA", SqlFieldProperties="prop() xref(NAZIONE.NZ__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Nazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PaIdNazioneNascita  { get; set; } = new List<string>();

[Display(Name = "Id Cittadinanza", ShortName="", Description = "Codice del paese di cittadinanza", Prompt="")]
[ErpDogField("PA_ID_CITTADINANZA", SqlFieldNameExt="PA_ID_CITTADINANZA", SqlFieldProperties="prop() xref(NAZIONE.NZ__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Nazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PaIdCittadinanza  { get; set; } = new List<string>();

[Display(Name = "Indirizzo Res", ShortName="", Description = "Indirizzo legale: strada (linea 1)", Prompt="")]
[ErpDogField("PA_INDIRIZZO_RES", SqlFieldNameExt="PA_INDIRIZZO_RES", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaIndirizzoRes  { get; set; }

[Display(Name = "Num Civico Res", ShortName="", Description = "Indirizzo legale: numero civico", Prompt="")]
[ErpDogField("PA_NUM_CIVICO_RES", SqlFieldNameExt="PA_NUM_CIVICO_RES", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaNumCivicoRes  { get; set; }

[Display(Name = "Cap Res", ShortName="", Description = "Indirizzo legale: codice postale", Prompt="")]
[ErpDogField("PA_CAP_RES", SqlFieldNameExt="PA_CAP_RES", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaCapRes  { get; set; }

[Display(Name = "Id Comune Res", ShortName="", Description = "Indirizzo legale: codice del comune", Prompt="")]
[ErpDogField("PA_ID_COMUNE_RES", SqlFieldNameExt="PA_ID_COMUNE_RES", SqlFieldProperties="prop() xref(COMUNE.CM__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Comune", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PaIdComuneRes  { get; set; } = new List<string>();

[Display(Name = "Id Distretto Res", ShortName="", Description = "Indirizzo legale : Codice di distretto", Prompt="")]
[ErpDogField("PA_ID_DISTRETTO_RES", SqlFieldNameExt="PA_ID_DISTRETTO_RES", SqlFieldProperties="prop() xref(DISTRETTO.DI__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Distretto", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PaIdDistrettoRes  { get; set; } = new List<string>();

[Display(Name = "Id Nazione Dom", ShortName="", Description = "Codice del paese in cui il paziente risiede", Prompt="")]
[ErpDogField("PA_ID_NAZIONE_DOM", SqlFieldNameExt="PA_ID_NAZIONE_DOM", SqlFieldProperties="prop() xref(NAZIONE.NZ__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Nazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PaIdNazioneDom  { get; set; } = new List<string>();

[Display(Name = "Mail", ShortName="", Description = "Indirizzo email del paziente", Prompt="")]
[ErpDogField("PA_MAIL", SqlFieldNameExt="PA_MAIL", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaMail  { get; set; }

[Display(Name = "Indirizzo Dom", ShortName="", Description = "Indirizzo di residenza: strada (linea 1)", Prompt="")]
[ErpDogField("PA_INDIRIZZO_DOM", SqlFieldNameExt="PA_INDIRIZZO_DOM", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaIndirizzoDom  { get; set; }

[Display(Name = "Num Civico Dom", ShortName="", Description = "Indirizzo di residenza: numero civico", Prompt="")]
[ErpDogField("PA_NUM_CIVICO_DOM", SqlFieldNameExt="PA_NUM_CIVICO_DOM", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaNumCivicoDom  { get; set; }

[Display(Name = "Cap Dom", ShortName="", Description = "Indirizzo di residenza: codice postale", Prompt="")]
[ErpDogField("PA_CAP_DOM", SqlFieldNameExt="PA_CAP_DOM", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaCapDom  { get; set; }

[Display(Name = "Id Comune Dom", ShortName="", Description = "Indirizzo di residenza: codice del comune", Prompt="")]
[ErpDogField("PA_ID_COMUNE_DOM", SqlFieldNameExt="PA_ID_COMUNE_DOM", SqlFieldProperties="prop() xref(COMUNE.CM__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Comune", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PaIdComuneDom  { get; set; } = new List<string>();

[Display(Name = "Telefono", ShortName="", Description = "Indirizzo di residenza: numero di telefono (1)", Prompt="")]
[ErpDogField("PA_TELEFONO", SqlFieldNameExt="PA_TELEFONO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaTelefono  { get; set; }

[Display(Name = "Cellulare", ShortName="", Description = "Indirizzo di residenza: numero di telefono (2)", Prompt="")]
[ErpDogField("PA_CELLULARE", SqlFieldNameExt="PA_CELLULARE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaCellulare  { get; set; }

[Display(Name = "Id Distretto Dom", ShortName="", Description = "Indirizzo di residenza : Codice di distretto", Prompt="")]
[ErpDogField("PA_ID_DISTRETTO_DOM", SqlFieldNameExt="PA_ID_DISTRETTO_DOM", SqlFieldProperties="prop() xref(DISTRETTO.DI__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Distretto", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PaIdDistrettoDom  { get; set; } = new List<string>();

[Display(Name = "Note", ShortName="", Description = "Note generiche sul paziente", Prompt="")]
[ErpDogField("PA_NOTE", SqlFieldNameExt="PA_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PaNote  { get; set; }

[Display(Name = "Data Decesso", ShortName="", Description = "Data di morte", Prompt="")]
[ErpDogField("PA_DATA_DECESSO", SqlFieldNameExt="PA_DATA_DECESSO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange PaDataDecesso  { get; set; } = new DateRange();

[Display(Name = "Ora Decesso", ShortName="", Description = "Ora di morte", Prompt="")]
[ErpDogField("PA_ORA_DECESSO", SqlFieldNameExt="PA_ORA_DECESSO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? PaOraDecesso  { get; set; }

[Display(Name = "Id Nazione Res", ShortName="", Description = "Codice del comune di residenza del paziente", Prompt="")]
[ErpDogField("PA_ID_NAZIONE_RES", SqlFieldNameExt="PA_ID_NAZIONE_RES", SqlFieldProperties="prop() xref(NAZIONE.NZ__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Nazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PaIdNazioneRes  { get; set; } = new List<string>();
}
}
