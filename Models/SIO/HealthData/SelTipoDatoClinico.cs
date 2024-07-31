using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.HealthData {
public class SelTipoDatoClinico {
public const string Description = "Classificazioni generali dei tipi di dati sanitari";
public const string SqlTableName = "TIPO_DATO_CLINICO";
public const string SqlTableNameExt = "TIPO_DATO_CLINICO";
public const string SqlRowIdName = "TC__ID";
public const string SqlRowIdNameExt = "TC__ICODE";
public const string SqlPrefix = "TC_";
public const string SqlPrefixExt = "TC_";
public const string SqlXdataTableName = "TC_XDATA";
public const string SqlXdataTableNameExt = "TC_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 14; //Internal Table Code
public const string TBAREA = "Dati clinici"; //Table Area
public const string PREFIX = "Tc"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TC_CODICE", SqlFieldNameExt="TC_CODICE", SqlFieldProperties="prop() xref() xdup(TIPO_DATO_CLINICO.TC__ICODE[TC__ICODE] {TC_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TcCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TC_DESCRIZIONE", SqlFieldNameExt="TC_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TcDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TC_NOTE", SqlFieldNameExt="TC_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TcNote  { get; set; }

[Display(Name = "Id Categoria Dato Clinico", ShortName="", Description = "Codice della classe dell'elemento del record sanitario", Prompt="")]
[ErpDogField("TC_ID_CATEGORIA_DATO_CLINICO", SqlFieldNameExt="TC_ID_CATEGORIA_DATO_CLINICO", SqlFieldProperties="prop() xref(CATEGORIA_DATO_CLINICO.CC__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("CategoriaDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> TcIdCategoriaDatoClinico  { get; set; } = new List<string>();

[Display(Name = "Unita Di Misura", ShortName="", Description = "Unità di misura", Prompt="")]
[ErpDogField("TC_UNITA_DI_MISURA", SqlFieldNameExt="TC_UNITA_DI_MISURA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TcUnitaDiMisura  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Codice del tipo aggregato di HRI di cui questo elemento fa parte", Prompt="")]
[ErpDogField("TC_ID_GRUPPO", SqlFieldNameExt="TC_ID_GRUPPO", SqlFieldProperties="prop() xref(TIPO_DATO_CLINICO.TC__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> TcIdGruppo  { get; set; } = new List<string>();

[Display(Name = "Sequenza", ShortName="", Description = "Ordine sequenziale degli HD aggregati (se presente)", Prompt="")]
[ErpDogField("TC_SEQUENZA", SqlFieldNameExt="TC_SEQUENZA", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? TcSequenza  { get; set; }

[Display(Name = "Attributi1", ShortName="", Description = "Flag operativi, gestiti dall'applicazione", Prompt="")]
[ErpDogField("TC_ATTRIBUTI1", SqlFieldNameExt="TC_ATTRIBUTI1", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(240, ErrorMessage = "Inserire massimo 240 caratteri")]
[DataType(DataType.Text)]
public string? TcAttributi1  { get; set; }

[Display(Name = "Attributi2", ShortName="", Description = "Ulteriori flag operativi, gestiti dalle applicazioni", Prompt="")]
[ErpDogField("TC_ATTRIBUTI2", SqlFieldNameExt="TC_ATTRIBUTI2", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(240, ErrorMessage = "Inserire massimo 240 caratteri")]
[DataType(DataType.Text)]
public string? TcAttributi2  { get; set; }
}
}
