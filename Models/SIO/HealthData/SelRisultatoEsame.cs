using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.HealthData {
public class SelRisultatoEsame : ModelErp {
public const string Description = "Dati sanitari - Risultato degli esami";
public const string SqlTableName = "RISULTATO_ESAME";
public const string SqlTableNameExt = "RISULTATO_ESAME";
public const string SqlRowIdName = "RE__ID";
public const string SqlRowIdNameExt = "RE__ICODE";
public const string SqlPrefix = "RE_";
public const string SqlPrefixExt = "RE_";
public const string SqlXdataTableName = "RE_XDATA";
public const string SqlXdataTableNameExt = "RE_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "SEL"; //Data Model Name of the Class
public const int INTCODE = 39; //Internal Table Code
public const string TBAREA = "Dati clinici"; //Table Area
public const string PREFIX = "Re"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//1026-1025//REL_PRESTAZIONE_DATO_CLINICO.PD_ID_DATO_CLINICO
public List<ErpToolkit.Models.SIO.HealthData.RelPrestazioneDatoClinico> RelPrestazioneDatoClinico4PdIdDatoClinico  { get; set; } = new List<ErpToolkit.Models.SIO.HealthData.RelPrestazioneDatoClinico>();

[Display(Name = "Classe", ShortName="", Description = "Classe del dato sanitario: 2: risultati degli esami", Prompt="")]
[ErpDogField("RE_CLASSE", SqlFieldNameExt="RE_CLASSE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? ReClasse  { get; set; }

[Display(Name = "Id Paziente", ShortName="", Description = "Codice del paziente a cui si riferisce il dato sanitario", Prompt="")]
[ErpDogField("RE_ID_PAZIENTE", SqlFieldNameExt="RE_ID_PAZIENTE", SqlFieldOptions="", Xref="Pa1Icode", SqlFieldProperties="prop() xref(PAZIENTE.PA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Paziente", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> ReIdPaziente  { get; set; } = new List<string>();

[Display(Name = "Id Episodio", ShortName="", Description = "Classe del tipo di dato sanitario", Prompt="")]
[ErpDogField("RE_ID_EPISODIO", SqlFieldNameExt="RE_ID_EPISODIO", SqlFieldOptions="", Xref="Cc1Icode", SqlFieldProperties="prop() xref(CATEGORIA_DATO_CLINICO.CC__ICODE) xdup(TIPO_DATO_CLINICO.TC_ID_CATEGORIA_DATO_CLINICO[RISULTATO_ESAME.RE_ID_GRUPPO_DATO_CLINICO]) multbxref()")]
[DefaultValue("")]
[AutocompleteClient("CategoriaDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> ReIdEpisodio  { get; set; } = new List<string>();

[Display(Name = "Id Tipo Dato Clinico", ShortName="", Description = "Codice del contatto a cui si riferisce il Dato Sanitario", Prompt="")]
[ErpDogField("RE_ID_TIPO_DATO_CLINICO", SqlFieldNameExt="RE_ID_TIPO_DATO_CLINICO", SqlFieldOptions="", Xref="Ep1Icode", SqlFieldProperties="prop() xref(EPISODIO.EP__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Episodio", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> ReIdTipoDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Id Gruppo Dato Clinico", ShortName="", Description = "Codice del tipo di Dato Sanitario", Prompt="")]
[ErpDogField("RE_ID_GRUPPO_DATO_CLINICO", SqlFieldNameExt="RE_ID_GRUPPO_DATO_CLINICO", SqlFieldOptions="", Xref="Tc1Icode", SqlFieldProperties="prop() xref(TIPO_DATO_CLINICO.TC__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> ReIdGruppoDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Valore Minimo", ShortName="", Description = "Valori numerici minimi (se applicabile)", Prompt="")]
[ErpDogField("RE_VALORE_MINIMO", SqlFieldNameExt="RE_VALORE_MINIMO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? ReValoreMinimo  { get; set; }

[Display(Name = "Valore Massimo", ShortName="", Description = "Valori numerici massimi (se applicabile)", Prompt="")]
[ErpDogField("RE_VALORE_MASSIMO", SqlFieldNameExt="RE_VALORE_MASSIMO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? ReValoreMassimo  { get; set; }

[Display(Name = "Valore Scelta", ShortName="", Description = "Valore carattere [se applicabile, in base al tipo di risultato]", Prompt="")]
[ErpDogField("RE_VALORE_SCELTA", SqlFieldNameExt="RE_VALORE_SCELTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? ReValoreScelta  { get; set; }

[Display(Name = "Valore Testo", ShortName="", Description = "Valore testuale, se applicabile", Prompt="")]
[ErpDogField("RE_VALORE_TESTO", SqlFieldNameExt="RE_VALORE_TESTO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? ReValoreTesto  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note [se applicabile, in base al tipo di risultato]", Prompt="")]
[ErpDogField("RE_NOTE", SqlFieldNameExt="RE_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? ReNote  { get; set; }

[Display(Name = "Codice Referto", ShortName="", Description = "Criterio di codifica/unità di misura adottato (se applicabile)", Prompt="")]
[ErpDogField("RE_CODICE_REFERTO", SqlFieldNameExt="RE_CODICE_REFERTO", SqlFieldOptions="[XID]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? ReCodiceReferto  { get; set; }

[Display(Name = "Data Acquisizione", ShortName="", Description = "Data di acquisizione del dato sanitario", Prompt="")]
[ErpDogField("RE_DATA_ACQUISIZIONE", SqlFieldNameExt="RE_DATA_ACQUISIZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange ReDataAcquisizione  { get; set; } = new DateRange();

[Display(Name = "Ora Acquisizione", ShortName="", Description = "Ora di acquisizione del dato sanitario", Prompt="")]
[ErpDogField("RE_ORA_ACQUISIZIONE", SqlFieldNameExt="RE_ORA_ACQUISIZIONE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? ReOraAcquisizione  { get; set; }

[Display(Name = "Stato Dato Clinico", ShortName="", Description = "Stato del dato: P[reliminare] - C[onfermato] - A[nnullato]", Prompt="")]
[ErpDogField("RE_STATO_DATO_CLINICO", SqlFieldNameExt="RE_STATO_DATO_CLINICO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("P")]
[MultipleChoices(new[] { "P", "C", "A" }, MaxSelections=-1, LabelClassName="")]
[DataType(DataType.Text)]
public List<string> ReStatoDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Data Validazione", ShortName="", Description = "Data di convalida del dato sanitario", Prompt="")]
[ErpDogField("RE_DATA_VALIDAZIONE", SqlFieldNameExt="RE_DATA_VALIDAZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange ReDataValidazione  { get; set; } = new DateRange();

[Display(Name = "Ora Validazione", ShortName="", Description = "Ora di convalida del dato sanitario", Prompt="")]
[ErpDogField("RE_ORA_VALIDAZIONE", SqlFieldNameExt="RE_ORA_VALIDAZIONE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? ReOraValidazione  { get; set; }

[Display(Name = "Sequenza", ShortName="", Description = "Numero di sequenza del dato nel report originale", Prompt="")]
[ErpDogField("RE_SEQUENZA", SqlFieldNameExt="RE_SEQUENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? ReSequenza  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        // verifica se almeno un campo indicizzato è valorizzato (test per validazioni complesse del modello) 
        bool found = false; 
        foreach (var idx in ListIndexes()) { 
            string fldLst = idx.Split("|")[2]; 
            foreach (var fld in fldLst.Split(",")) { 
                if (DogHelper.getPropertyValue(this, fld.Trim()) != null) found = true; 
                if (DogHelper.getPropertyValue(this, fld.Trim() + "[0]") != null) found = true; 
                if (DogHelper.getPropertyValue(this, fld.Trim() + ".StartDate") != null) found = true; 
                if (DogHelper.getPropertyValue(this, fld.Trim() + ".EndDate") != null) found = true; 
            } 
        } 
        if (!found) { isValidate = false;  modelState.AddModelError(string.Empty, "Deve essere compilato almeno un campo indicizzato."); } 
        //-- 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioRe1Icode|K|Re1Icode","sioRe1RecDate|N|Re1Mdate,Re1Cdate"
        ,"sioReIdTipoDatoClinicoReIdGruppoDatoClinicoReDataAcquisizione|N|ReIdTipoDatoClinico,ReIdGruppoDatoClinico,ReDataAcquisizione"
        ,"sioReIdPazienteReDataAcquisizione|N|ReIdPaziente,ReDataAcquisizione"
        ,"sioReIdGruppoDatoClinicoReStatoDatoClinicoReDataAcquisizione|N|ReIdGruppoDatoClinico,ReStatoDatoClinico,ReDataAcquisizione"
        ,"sioReCodiceReferto|N|ReCodiceReferto"
    };
}
}
}
