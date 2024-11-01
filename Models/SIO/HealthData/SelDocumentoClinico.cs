using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.HealthData {
public class SelDocumentoClinico : ModelErp {
public const string Description = "Dati sanitari - Altri tipi di dati";
public const string SqlTableName = "DOCUMENTO_CLINICO";
public const string SqlTableNameExt = "DOCUMENTO_CLINICO";
public const string SqlRowIdName = "DC__ID";
public const string SqlRowIdNameExt = "DC__ICODE";
public const string SqlPrefix = "DC_";
public const string SqlPrefixExt = "DC_";
public const string SqlXdataTableName = "DC_XDATA";
public const string SqlXdataTableNameExt = "DC_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "SEL"; //Data Model Name of the Class
public const int INTCODE = 41; //Internal Table Code
public const string TBAREA = "Dati clinici"; //Table Area
public const string PREFIX = "Dc"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//1026-1025//REL_PRESTAZIONE_DATO_CLINICO.PD_ID_DATO_CLINICO
public List<ErpToolkit.Models.SIO.HealthData.RelPrestazioneDatoClinico> SelRelPrestazioneDatoClinico4PdIdDatoClinico  { get; set; } = new List<ErpToolkit.Models.SIO.HealthData.RelPrestazioneDatoClinico>();

[Display(Name = "Classe", ShortName="", Description = "Classe del dato sanitario: 4: altri tipi di dati", Prompt="")]
[ErpDogField("DC_CLASSE", SqlFieldNameExt="DC_CLASSE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelDcClasse  { get; set; }

[Display(Name = "Id Paziente", ShortName="", Description = "Codice del paziente a cui si riferisce il dato sanitario", Prompt="")]
[ErpDogField("DC_ID_PAZIENTE", SqlFieldNameExt="DC_ID_PAZIENTE", SqlFieldOptions="", Xref="Pa1Icode", SqlFieldProperties="prop() xref(PAZIENTE.PA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Paziente", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> SelDcIdPaziente  { get; set; } = new List<string>();

[Display(Name = "Id Episodio", ShortName="", Description = "Codice del contatto a cui si riferisce il Dato Sanitario", Prompt="")]
[ErpDogField("DC_ID_EPISODIO", SqlFieldNameExt="DC_ID_EPISODIO", SqlFieldOptions="", Xref="Ep1Icode", SqlFieldProperties="prop() xref(EPISODIO.EP__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Episodio", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> SelDcIdEpisodio  { get; set; } = new List<string>();

[Display(Name = "Id Tipo Dato Clinico", ShortName="", Description = "Codice del tipo di Dato Sanitario", Prompt="")]
[ErpDogField("DC_ID_TIPO_DATO_CLINICO", SqlFieldNameExt="DC_ID_TIPO_DATO_CLINICO", SqlFieldOptions="", Xref="Tc1Icode", SqlFieldProperties="prop() xref(TIPO_DATO_CLINICO.TC__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelDcIdTipoDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Id Gruppo Dato Clinico", ShortName="", Description = "Classe del tipo di Dati di Salute", Prompt="")]
[ErpDogField("DC_ID_GRUPPO_DATO_CLINICO", SqlFieldNameExt="DC_ID_GRUPPO_DATO_CLINICO", SqlFieldOptions="", Xref="Cc1Icode", SqlFieldProperties="prop() xref(CATEGORIA_DATO_CLINICO.CC__ICODE) xdup(TIPO_DATO_CLINICO.TC_ID_CATEGORIA_DATO_CLINICO[DOCUMENTO_CLINICO.DC_ID_TIPO_DATO_CLINICO]) multbxref()")]
[DefaultValue("")]
[AutocompleteClient("CategoriaDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelDcIdGruppoDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Valore Minimo", ShortName="", Description = "Valori numerici minimi (se applicabile)", Prompt="")]
[ErpDogField("DC_VALORE_MINIMO", SqlFieldNameExt="DC_VALORE_MINIMO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? SelDcValoreMinimo  { get; set; }

[Display(Name = "Valore Massimo", ShortName="", Description = "Valori numerici massimi (se applicabile)", Prompt="")]
[ErpDogField("DC_VALORE_MASSIMO", SqlFieldNameExt="DC_VALORE_MASSIMO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? SelDcValoreMassimo  { get; set; }

[Display(Name = "Valore Scelta", ShortName="", Description = "Valore carattere (se applicabile, in base al tipo di risultato)", Prompt="")]
[ErpDogField("DC_VALORE_SCELTA", SqlFieldNameExt="DC_VALORE_SCELTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelDcValoreScelta  { get; set; }

[Display(Name = "Valore Testo", ShortName="", Description = "Valore testuale, se applicabile", Prompt="")]
[ErpDogField("DC_VALORE_TESTO", SqlFieldNameExt="DC_VALORE_TESTO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelDcValoreTesto  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note (se applicabile, in base al tipo di risultato)", Prompt="")]
[ErpDogField("DC_NOTE", SqlFieldNameExt="DC_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelDcNote  { get; set; }

[Display(Name = "Codice Referto", ShortName="", Description = "Criterio di codifica/unità di misura adottato (se applicabile)", Prompt="")]
[ErpDogField("DC_CODICE_REFERTO", SqlFieldNameExt="DC_CODICE_REFERTO", SqlFieldOptions="[XID]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelDcCodiceReferto  { get; set; }

[Display(Name = "Data Acquisizione", ShortName="", Description = "Data di acquisizione del dato sanitario", Prompt="")]
[ErpDogField("DC_DATA_ACQUISIZIONE", SqlFieldNameExt="DC_DATA_ACQUISIZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SelDcDataAcquisizione  { get; set; } = new DateRange();

[Display(Name = "Ora Acquisizione", ShortName="", Description = "Ora di acquisizione del dato sanitario", Prompt="")]
[ErpDogField("DC_ORA_ACQUISIZIONE", SqlFieldNameExt="DC_ORA_ACQUISIZIONE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
public TimeOnly? SelDcOraAcquisizione  { get; set; }

[Display(Name = "Stato Dato Clinico", ShortName="", Description = "Stato del dato: P[reliminare] - C[onfermato] - A[nnullato]", Prompt="")]
[ErpDogField("DC_STATO_DATO_CLINICO", SqlFieldNameExt="DC_STATO_DATO_CLINICO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("P")]
[MultipleChoices(new[] { "P", "C", "A" }, MaxSelections=-1, LabelClassName="")]
[DataType(DataType.Text)]
public List<string> SelDcStatoDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Data Validazione", ShortName="", Description = "Data di convalida del dato sanitario", Prompt="")]
[ErpDogField("DC_DATA_VALIDAZIONE", SqlFieldNameExt="DC_DATA_VALIDAZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SelDcDataValidazione  { get; set; } = new DateRange();

[Display(Name = "Ora Validazione", ShortName="", Description = "Ora di convalida del dato sanitario", Prompt="")]
[ErpDogField("DC_ORA_VALIDAZIONE", SqlFieldNameExt="DC_ORA_VALIDAZIONE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
public TimeOnly? SelDcOraValidazione  { get; set; }

[Display(Name = "Sequenza", ShortName="", Description = "Numero di sequenza del dato nel report originale", Prompt="")]
[ErpDogField("DC_SEQUENZA", SqlFieldNameExt="DC_SEQUENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? SelDcSequenza  { get; set; }

public override bool TryValidateInt(ModelStateDictionary modelState, string? prefix = null) 
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
        if (!found) { isValidate = false;  modelState.AddModelError(prefix ?? string.Empty, "Deve essere compilato almeno un campo indicizzato."); } 
        //-- 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioDc1Icode|K|DC__ICODE","sioDc1RecDate|N|DC__MDATE,DC__CDATE"
        ,"sioDcIdEpisodiodcIdTipoDatoClinicodcDataAcquisizione|N|DC_ID_EPISODIO,DC_ID_TIPO_DATO_CLINICO,DC_DATA_ACQUISIZIONE"
        ,"sioDcIdPazientedcDataAcquisizione|N|DC_ID_PAZIENTE,DC_DATA_ACQUISIZIONE"
        ,"sioDcIdTipoDatoClinicodcStatoDatoClinicodcDataAcquisizione|N|DC_ID_TIPO_DATO_CLINICO,DC_STATO_DATO_CLINICO,DC_DATA_ACQUISIZIONE"
        ,"sioDcCodiceReferto|N|DC_CODICE_REFERTO"
    };
}
}
}
