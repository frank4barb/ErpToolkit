using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class SelPaziente : ModelErp {
public const string Description = "Pazienti rilevanti per l'organizzazione sanitaria";
public const string SqlTableName = "PAZIENTE";
public const string SqlTableNameExt = "PAZIENTE";
public const string SqlRowIdName = "PA__ID";
public const string SqlRowIdNameExt = "PA__ICODE";
public const string SqlPrefix = "PA_";
public const string SqlPrefixExt = "PA_";
public const string SqlXdataTableName = "PA_XDATA";
public const string SqlXdataTableNameExt = "PA_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "SEL"; //Data Model Name of the Class
public const int INTCODE = 51; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Pa"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Cod Fiscale", ShortName="", Description = "Identificatore nazionale del paziente/individuo", Prompt="")]
[ErpDogField("PA_COD_FISCALE", SqlFieldNameExt="PA_COD_FISCALE", SqlFieldOptions="[XID]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaCodFiscale  { get; set; }

[Display(Name = "Cod Sanitario", ShortName="", Description = "Identificatore permanente del paziente nell'organizzazione sanitaria", Prompt="")]
[ErpDogField("PA_COD_SANITARIO", SqlFieldNameExt="PA_COD_SANITARIO", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(PAZIENTE.PA__ICODE[PA__ICODE] {PA_COD_SANITARIO=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? SelPaCodSanitario  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Nome del paziente", Prompt="")]
[ErpDogField("PA_NOME", SqlFieldNameExt="PA_NOME", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaNome  { get; set; }

[Display(Name = "Cognome", ShortName="", Description = "Cognome del paziente", Prompt="")]
[ErpDogField("PA_COGNOME", SqlFieldNameExt="PA_COGNOME", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaCognome  { get; set; }

[Display(Name = "Sesso", ShortName="", Description = "Sesso M / F / N", Prompt="")]
[ErpDogField("PA_SESSO", SqlFieldNameExt="PA_SESSO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[MultipleChoices(new[] { "M", "F", "N" }, MaxSelections=-1, LabelClassName="")]
[DataType(DataType.Text)]
public List<string> SelPaSesso  { get; set; } = new List<string>();

[Display(Name = "Data Nascita", ShortName="", Description = "Data di nascita", Prompt="")]
[ErpDogField("PA_DATA_NASCITA", SqlFieldNameExt="PA_DATA_NASCITA", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SelPaDataNascita  { get; set; } = new DateRange();

[Display(Name = "Ora Nascita", ShortName="", Description = "Ora di nascita", Prompt="")]
[ErpDogField("PA_ORA_NASCITA", SqlFieldNameExt="PA_ORA_NASCITA", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? SelPaOraNascita  { get; set; }

[Display(Name = "Id Comune Nascita", ShortName="", Description = "Codice del comune di nascita", Prompt="")]
[ErpDogField("PA_ID_COMUNE_NASCITA", SqlFieldNameExt="PA_ID_COMUNE_NASCITA", SqlFieldOptions="", Xref="Cm1Icode", SqlFieldProperties="prop() xref(COMUNE.CM__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Comune", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelPaIdComuneNascita  { get; set; } = new List<string>();

[Display(Name = "Id Nazione Nascita", ShortName="", Description = "Codice del paese di nascita", Prompt="")]
[ErpDogField("PA_ID_NAZIONE_NASCITA", SqlFieldNameExt="PA_ID_NAZIONE_NASCITA", SqlFieldOptions="", Xref="Nz1Icode", SqlFieldProperties="prop() xref(NAZIONE.NZ__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Nazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelPaIdNazioneNascita  { get; set; } = new List<string>();

[Display(Name = "Id Cittadinanza", ShortName="", Description = "Codice del paese di cittadinanza", Prompt="")]
[ErpDogField("PA_ID_CITTADINANZA", SqlFieldNameExt="PA_ID_CITTADINANZA", SqlFieldOptions="", Xref="Nz1Icode", SqlFieldProperties="prop() xref(NAZIONE.NZ__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Nazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelPaIdCittadinanza  { get; set; } = new List<string>();

[Display(Name = "Indirizzo Res", ShortName="", Description = "Indirizzo legale: strada (linea 1)", Prompt="")]
[ErpDogField("PA_INDIRIZZO_RES", SqlFieldNameExt="PA_INDIRIZZO_RES", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaIndirizzoRes  { get; set; }

[Display(Name = "Num Civico Res", ShortName="", Description = "Indirizzo legale: numero civico", Prompt="")]
[ErpDogField("PA_NUM_CIVICO_RES", SqlFieldNameExt="PA_NUM_CIVICO_RES", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaNumCivicoRes  { get; set; }

[Display(Name = "Cap Res", ShortName="", Description = "Indirizzo legale: codice postale", Prompt="")]
[ErpDogField("PA_CAP_RES", SqlFieldNameExt="PA_CAP_RES", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaCapRes  { get; set; }

[Display(Name = "Id Comune Res", ShortName="", Description = "Indirizzo legale: codice del comune", Prompt="")]
[ErpDogField("PA_ID_COMUNE_RES", SqlFieldNameExt="PA_ID_COMUNE_RES", SqlFieldOptions="", Xref="Cm1Icode", SqlFieldProperties="prop() xref(COMUNE.CM__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Comune", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelPaIdComuneRes  { get; set; } = new List<string>();

[Display(Name = "Id Distretto Res", ShortName="", Description = "Indirizzo legale : Codice di distretto", Prompt="")]
[ErpDogField("PA_ID_DISTRETTO_RES", SqlFieldNameExt="PA_ID_DISTRETTO_RES", SqlFieldOptions="", Xref="Di1Icode", SqlFieldProperties="prop() xref(DISTRETTO.DI__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Distretto", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelPaIdDistrettoRes  { get; set; } = new List<string>();

[Display(Name = "Id Nazione Dom", ShortName="", Description = "Codice del paese in cui il paziente risiede", Prompt="")]
[ErpDogField("PA_ID_NAZIONE_DOM", SqlFieldNameExt="PA_ID_NAZIONE_DOM", SqlFieldOptions="", Xref="Nz1Icode", SqlFieldProperties="prop() xref(NAZIONE.NZ__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Nazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelPaIdNazioneDom  { get; set; } = new List<string>();

[Display(Name = "Mail", ShortName="", Description = "Indirizzo email del paziente", Prompt="")]
[ErpDogField("PA_MAIL", SqlFieldNameExt="PA_MAIL", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaMail  { get; set; }

[Display(Name = "Indirizzo Dom", ShortName="", Description = "Indirizzo di residenza: strada (linea 1)", Prompt="")]
[ErpDogField("PA_INDIRIZZO_DOM", SqlFieldNameExt="PA_INDIRIZZO_DOM", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaIndirizzoDom  { get; set; }

[Display(Name = "Num Civico Dom", ShortName="", Description = "Indirizzo di residenza: numero civico", Prompt="")]
[ErpDogField("PA_NUM_CIVICO_DOM", SqlFieldNameExt="PA_NUM_CIVICO_DOM", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaNumCivicoDom  { get; set; }

[Display(Name = "Cap Dom", ShortName="", Description = "Indirizzo di residenza: codice postale", Prompt="")]
[ErpDogField("PA_CAP_DOM", SqlFieldNameExt="PA_CAP_DOM", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaCapDom  { get; set; }

[Display(Name = "Id Comune Dom", ShortName="", Description = "Indirizzo di residenza: codice del comune", Prompt="")]
[ErpDogField("PA_ID_COMUNE_DOM", SqlFieldNameExt="PA_ID_COMUNE_DOM", SqlFieldOptions="", Xref="Cm1Icode", SqlFieldProperties="prop() xref(COMUNE.CM__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Comune", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelPaIdComuneDom  { get; set; } = new List<string>();

[Display(Name = "Telefono", ShortName="", Description = "Indirizzo di residenza: numero di telefono (1)", Prompt="")]
[ErpDogField("PA_TELEFONO", SqlFieldNameExt="PA_TELEFONO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaTelefono  { get; set; }

[Display(Name = "Cellulare", ShortName="", Description = "Indirizzo di residenza: numero di telefono (2)", Prompt="")]
[ErpDogField("PA_CELLULARE", SqlFieldNameExt="PA_CELLULARE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaCellulare  { get; set; }

[Display(Name = "Id Distretto Dom", ShortName="", Description = "Indirizzo di residenza : Codice di distretto", Prompt="")]
[ErpDogField("PA_ID_DISTRETTO_DOM", SqlFieldNameExt="PA_ID_DISTRETTO_DOM", SqlFieldOptions="", Xref="Di1Icode", SqlFieldProperties="prop() xref(DISTRETTO.DI__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Distretto", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelPaIdDistrettoDom  { get; set; } = new List<string>();

[Display(Name = "Note", ShortName="", Description = "Note generiche sul paziente", Prompt="")]
[ErpDogField("PA_NOTE", SqlFieldNameExt="PA_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelPaNote  { get; set; }

[Display(Name = "Data Decesso", ShortName="", Description = "Data di morte", Prompt="")]
[ErpDogField("PA_DATA_DECESSO", SqlFieldNameExt="PA_DATA_DECESSO", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SelPaDataDecesso  { get; set; } = new DateRange();

[Display(Name = "Ora Decesso", ShortName="", Description = "Ora di morte", Prompt="")]
[ErpDogField("PA_ORA_DECESSO", SqlFieldNameExt="PA_ORA_DECESSO", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? SelPaOraDecesso  { get; set; }

[Display(Name = "Id Nazione Res", ShortName="", Description = "Codice del comune di residenza del paziente", Prompt="")]
[ErpDogField("PA_ID_NAZIONE_RES", SqlFieldNameExt="PA_ID_NAZIONE_RES", SqlFieldOptions="", Xref="Nz1Icode", SqlFieldProperties="prop() xref(NAZIONE.NZ__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Nazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelPaIdNazioneRes  { get; set; } = new List<string>();

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
    return new List<string>() { "sioPa1Icode|K|PA__ICODE","sioPa1RecDate|N|PA__MDATE,PA__CDATE"
        ,"sioPaIdDistrettoRes|N|PA_ID_DISTRETTO_RES"
        ,"sioPaIdDistrettoDom|N|PA_ID_DISTRETTO_DOM"
        ,"sioPaIdComuneNascita|N|PA_ID_COMUNE_NASCITA"
        ,"sioPaIdComuneRes|N|PA_ID_COMUNE_RES"
        ,"sioPaIdComuneDom|N|PA_ID_COMUNE_DOM"
        ,"sioPaIdCittadinanza|N|PA_ID_CITTADINANZA"
        ,"sioPaIdNazioneNascita|N|PA_ID_NAZIONE_NASCITA"
        ,"sioPaCodFiscalepa1Versionpa1Deleted|U|PA_COD_FISCALE,PA__VERSION,PA__DELETED"
        ,"sioPaCodSanitariopa1Versionpa1Deleted|U|PA_COD_SANITARIO,PA__VERSION,PA__DELETED"
        ,"sioPaCognomepaNome|N|PA_COGNOME,PA_NOME"
    };
}
}
}
