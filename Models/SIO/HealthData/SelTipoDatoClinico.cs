using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.HealthData {
public class SelTipoDatoClinico : ModelErp {
public const string Description = "Classificazioni generali dei tipi di dati sanitari";
public const string SqlTableName = "TIPO_DATO_CLINICO";
public const string SqlTableNameExt = "TIPO_DATO_CLINICO";
public const string SqlRowIdName = "TC__ID";
public const string SqlRowIdNameExt = "TC__ICODE";
public const string SqlPrefix = "TC_";
public const string SqlPrefixExt = "TC_";
public const string SqlXdataTableName = "TC_XDATA";
public const string SqlXdataTableNameExt = "TC_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "SEL"; //Data Model Name of the Class
public const int INTCODE = 14; //Internal Table Code
public const string TBAREA = "Dati clinici"; //Table Area
public const string PREFIX = "Tc"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TC_CODICE", SqlFieldNameExt="TC_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_DATO_CLINICO.TC__ICODE[TC__ICODE] {TC_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? SelTcCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TC_DESCRIZIONE", SqlFieldNameExt="TC_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelTcDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TC_NOTE", SqlFieldNameExt="TC_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelTcNote  { get; set; }

[Display(Name = "Id Categoria Dato Clinico", ShortName="", Description = "Codice della classe dell'elemento del record sanitario", Prompt="")]
[ErpDogField("TC_ID_CATEGORIA_DATO_CLINICO", SqlFieldNameExt="TC_ID_CATEGORIA_DATO_CLINICO", SqlFieldOptions="", Xref="Cc1Icode", SqlFieldProperties="prop() xref(CATEGORIA_DATO_CLINICO.CC__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("CategoriaDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelTcIdCategoriaDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Unita Di Misura", ShortName="", Description = "Unità di misura", Prompt="")]
[ErpDogField("TC_UNITA_DI_MISURA", SqlFieldNameExt="TC_UNITA_DI_MISURA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelTcUnitaDiMisura  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Codice del tipo aggregato di HRI di cui questo elemento fa parte", Prompt="")]
[ErpDogField("TC_ID_GRUPPO", SqlFieldNameExt="TC_ID_GRUPPO", SqlFieldOptions="", Xref="Tc1Icode", SqlFieldProperties="prop() xref(TIPO_DATO_CLINICO.TC__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelTcIdGruppo  { get; set; } = new List<string>();

[Display(Name = "Sequenza", ShortName="", Description = "Ordine sequenziale degli HD aggregati (se presente)", Prompt="")]
[ErpDogField("TC_SEQUENZA", SqlFieldNameExt="TC_SEQUENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? SelTcSequenza  { get; set; }

[Display(Name = "Attributi1", ShortName="", Description = "Flag operativi, gestiti dall'applicazione", Prompt="")]
[ErpDogField("TC_ATTRIBUTI1", SqlFieldNameExt="TC_ATTRIBUTI1", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelTcAttributi1  { get; set; }

[Display(Name = "Attributi2", ShortName="", Description = "Ulteriori flag operativi, gestiti dalle applicazioni", Prompt="")]
[ErpDogField("TC_ATTRIBUTI2", SqlFieldNameExt="TC_ATTRIBUTI2", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelTcAttributi2  { get; set; }

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
    return new List<string>() { "sioTc1Icode|K|TC__ICODE","sioTc1RecDate|N|TC__MDATE,TC__CDATE"
        ,"sioTcIdCategoriaDatoClinico|N|TC_ID_CATEGORIA_DATO_CLINICO"
        ,"sioTcIdGruppotcSequenza|N|TC_ID_GRUPPO,TC_SEQUENZA"
        ,"sioTc1Versiontc1Deleted|U|TC__VERSION,TC__DELETED"
        ,"sioTcCodicetc1Versiontc1Deleted|U|TC_CODICE,TC__VERSION,TC__DELETED"
    };
}
}
}
