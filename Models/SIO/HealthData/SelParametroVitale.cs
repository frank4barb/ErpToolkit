using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.HealthData {
public class SelParametroVitale : ModelErp {
public const string Description = "Dati sanitari - Parametri vitali";
public const string SqlTableName = "PARAMETRO_VITALE";
public const string SqlTableNameExt = "PARAMETRO_VITALE";
public const string SqlRowIdName = "PV__ID";
public const string SqlRowIdNameExt = "PV__ICODE";
public const string SqlPrefix = "PV_";
public const string SqlPrefixExt = "PV_";
public const string SqlXdataTableName = "PV_XDATA";
public const string SqlXdataTableNameExt = "PV_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 38; //Internal Table Code
public const string TBAREA = "Dati clinici"; //Table Area
public const string PREFIX = "Pv"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//1026-1025//REL_PRESTAZIONE_DATO_CLINICO.PD_ID_DATO_CLINICO
public List<ErpToolkit.Models.SIO.HealthData.RelPrestazioneDatoClinico> RelPrestazioneDatoClinico4PdIdDatoClinico  { get; set; } = new List<ErpToolkit.Models.SIO.HealthData.RelPrestazioneDatoClinico>();

[Display(Name = "Classe", ShortName="", Description = "Classe del dato sanitario: 1 - Parametri vitali", Prompt="")]
[ErpDogField("PV_CLASSE", SqlFieldNameExt="PV_CLASSE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PvClasse  { get; set; }

[Display(Name = "Id Paziente", ShortName="", Description = "Codice del paziente a cui si riferisce il dato sanitario", Prompt="")]
[ErpDogField("PV_ID_PAZIENTE", SqlFieldNameExt="PV_ID_PAZIENTE", SqlFieldOptions="", Xref="Pa1Icode", SqlFieldProperties="prop() xref(PAZIENTE.PA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Paziente", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> PvIdPaziente  { get; set; } = new List<string>();

[Display(Name = "Id Episodio", ShortName="", Description = "Codice del contatto a cui si riferisce il Dato Sanitario", Prompt="")]
[ErpDogField("PV_ID_EPISODIO", SqlFieldNameExt="PV_ID_EPISODIO", SqlFieldOptions="", Xref="Ep1Icode", SqlFieldProperties="prop() xref(EPISODIO.EP__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Episodio", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> PvIdEpisodio  { get; set; } = new List<string>();

[Display(Name = "Id Tipo Dato Clinico", ShortName="", Description = "Codice del tipo di Dato Sanitario", Prompt="")]
[ErpDogField("PV_ID_TIPO_DATO_CLINICO", SqlFieldNameExt="PV_ID_TIPO_DATO_CLINICO", SqlFieldOptions="", Xref="Tc1Icode", SqlFieldProperties="prop() xref(TIPO_DATO_CLINICO.TC__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PvIdTipoDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Id Gruppo Dato Clinico", ShortName="", Description = "Classe del tipo di dati sanitari", Prompt="")]
[ErpDogField("PV_ID_GRUPPO_DATO_CLINICO", SqlFieldNameExt="PV_ID_GRUPPO_DATO_CLINICO", SqlFieldOptions="", Xref="Cc1Icode", SqlFieldProperties="prop() xref(CATEGORIA_DATO_CLINICO.CC__ICODE) xdup(TIPO_DATO_CLINICO.TC_ID_CATEGORIA_DATO_CLINICO[PARAMETRO_VITALE.PV_ID_TIPO_DATO_CLINICO]) multbxref()")]
[DefaultValue("")]
[AutocompleteClient("CategoriaDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PvIdGruppoDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Valore Minimo", ShortName="", Description = "Valori numerici minimi (se applicabile)", Prompt="")]
[ErpDogField("PV_VALORE_MINIMO", SqlFieldNameExt="PV_VALORE_MINIMO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? PvValoreMinimo  { get; set; }

[Display(Name = "Valore Massimo", ShortName="", Description = "Valori numerici massimi (se applicabile)", Prompt="")]
[ErpDogField("PV_VALORE_MASSIMO", SqlFieldNameExt="PV_VALORE_MASSIMO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? PvValoreMassimo  { get; set; }

[Display(Name = "Valore Scelta", ShortName="", Description = "Valore carattere (se applicabile, in base al tipo di risultato)", Prompt="")]
[ErpDogField("PV_VALORE_SCELTA", SqlFieldNameExt="PV_VALORE_SCELTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PvValoreScelta  { get; set; }

[Display(Name = "Valore Testo", ShortName="", Description = "Valore testuale, se applicabile in base al tipo di dato", Prompt="")]
[ErpDogField("PV_VALORE_TESTO", SqlFieldNameExt="PV_VALORE_TESTO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PvValoreTesto  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note (se applicabile, in base al tipo di risultato)", Prompt="")]
[ErpDogField("PV_NOTE", SqlFieldNameExt="PV_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PvNote  { get; set; }

[Display(Name = "Codice Referto", ShortName="", Description = "Criterio di codifica/unità di misura adottato (se applicabile)", Prompt="")]
[ErpDogField("PV_CODICE_REFERTO", SqlFieldNameExt="PV_CODICE_REFERTO", SqlFieldOptions="[XID]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PvCodiceReferto  { get; set; }

[Display(Name = "Data Acquisizione", ShortName="", Description = "Data di acquisizione del dato sanitario", Prompt="")]
[ErpDogField("PV_DATA_ACQUISIZIONE", SqlFieldNameExt="PV_DATA_ACQUISIZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange PvDataAcquisizione  { get; set; } = new DateRange();

[Display(Name = "Ora Acquisizione", ShortName="", Description = "Ora di acquisizione del dato sanitario", Prompt="")]
[ErpDogField("PV_ORA_ACQUISIZIONE", SqlFieldNameExt="PV_ORA_ACQUISIZIONE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? PvOraAcquisizione  { get; set; }

[Display(Name = "Stato Dato Clinico", ShortName="", Description = "Stato del dato: P[reliminare] - C[onfermato] - A[nnullato]", Prompt="")]
[ErpDogField("PV_STATO_DATO_CLINICO", SqlFieldNameExt="PV_STATO_DATO_CLINICO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("P")]
[MultipleChoices(new[] { "P", "C", "A" }, MaxSelections=-1, LabelClassName="")]
[DataType(DataType.Text)]
public List<string> PvStatoDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Data Validazione", ShortName="", Description = "Data di convalida del dato sanitario", Prompt="")]
[ErpDogField("PV_DATA_VALIDAZIONE", SqlFieldNameExt="PV_DATA_VALIDAZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange PvDataValidazione  { get; set; } = new DateRange();

[Display(Name = "Ora Validazione", ShortName="", Description = "Ora di convalida del dato sanitario", Prompt="")]
[ErpDogField("PV_ORA_VALIDAZIONE", SqlFieldNameExt="PV_ORA_VALIDAZIONE", SqlFieldOptions="[TIME]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public TimeOnly? PvOraValidazione  { get; set; }

[Display(Name = "Sequenza", ShortName="", Description = "Numero di sequenza del dato nel rapporto originale", Prompt="")]
[ErpDogField("PV_SEQUENZA", SqlFieldNameExt="PV_SEQUENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? PvSequenza  { get; set; }

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
    return new List<string>() { "sioPv1Icode|K|Pv1Icode","sioPv1RecDate|N|Pv1Mdate,Pv1Cdate"
        ,"sioPvIdEpisodioPvIdTipoDatoClinicoPvDataAcquisizione|N|PvIdEpisodio,PvIdTipoDatoClinico,PvDataAcquisizione"
        ,"sioPvIdPazientePvDataAcquisizione|N|PvIdPaziente,PvDataAcquisizione"
        ,"sioPvIdTipoDatoClinicoPvStatoDatoClinicoPvDataAcquisizione|N|PvIdTipoDatoClinico,PvStatoDatoClinico,PvDataAcquisizione"
        ,"sioPvCodiceReferto|N|PvCodiceReferto"
    };
}
}
}
