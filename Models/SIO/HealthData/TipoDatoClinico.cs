using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.HealthData {
public class TipoDatoClinico : ModelErp {
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
[Display(Name = "Tc1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("TC__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Tc1Ienv { get; set; }
[Key]
[Display(Name = "Tc1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("TC__ICODE", SqlFieldNameExt="TC__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Tc1Icode { get; set; }
[Display(Name = "Tc1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("TC__DELETED", SqlFieldNameExt="TC__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Tc1Deleted { get; set; }
[Display(Name = "Tc1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("TC__TIMESTAMP", SqlFieldNameExt="TC__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Tc1Timestamp { get; set; }
[Display(Name = "Tc1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("TC__HOME", SqlFieldNameExt="TC__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Tc1Home { get; set; }
[Display(Name = "Tc1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("TC__VERSION", SqlFieldNameExt="TC__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Tc1Version { get; set; }
[Display(Name = "Tc1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("TC__INACTIVE", SqlFieldNameExt="TC__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Tc1Inactive { get; set; }
[Display(Name = "Tc1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("TC__EXTATT", SqlFieldNameExt="TC__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Tc1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TC_CODICE", SqlFieldNameExt="TC_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_DATO_CLINICO.TC__ICODE[TC__ICODE] {TC_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TcCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TC_DESCRIZIONE", SqlFieldNameExt="TC_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TcDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TC_NOTE", SqlFieldNameExt="TC_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TcNote  { get; set; }

[Display(Name = "Id Categoria Dato Clinico", ShortName="", Description = "Codice della classe dell'elemento del record sanitario", Prompt="")]
[ErpDogField("TC_ID_CATEGORIA_DATO_CLINICO", SqlFieldNameExt="TC_ID_CATEGORIA_DATO_CLINICO", SqlFieldOptions="", Xref="Cc1Icode", SqlFieldProperties="prop() xref(CATEGORIA_DATO_CLINICO.CC__ICODE) xdup() multbxref()")]
[AutocompleteClient("CategoriaDatoClinico", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? TcIdCategoriaDatoClinico  { get; set; }
public ErpToolkit.Models.SIO.HealthData.CategoriaDatoClinico? TcIdCategoriaDatoClinicoObj  { get; set; }

[Display(Name = "Unita Di Misura", ShortName="", Description = "Unità di misura", Prompt="")]
[ErpDogField("TC_UNITA_DI_MISURA", SqlFieldNameExt="TC_UNITA_DI_MISURA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TcUnitaDiMisura  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Codice del tipo aggregato di HRI di cui questo elemento fa parte", Prompt="")]
[ErpDogField("TC_ID_GRUPPO", SqlFieldNameExt="TC_ID_GRUPPO", SqlFieldOptions="", Xref="Tc1Icode", SqlFieldProperties="prop() xref(TIPO_DATO_CLINICO.TC__ICODE) xdup() multbxref()")]
[AutocompleteClient("TipoDatoClinico", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? TcIdGruppo  { get; set; }
public ErpToolkit.Models.SIO.HealthData.TipoDatoClinico? TcIdGruppoObj  { get; set; }

[Display(Name = "Sequenza", ShortName="", Description = "Ordine sequenziale degli HD aggregati (se presente)", Prompt="")]
[ErpDogField("TC_SEQUENZA", SqlFieldNameExt="TC_SEQUENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
public short? TcSequenza  { get; set; }

[Display(Name = "Attributi1", ShortName="", Description = "Flag operativi, gestiti dall'applicazione", Prompt="")]
[ErpDogField("TC_ATTRIBUTI1", SqlFieldNameExt="TC_ATTRIBUTI1", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(240, ErrorMessage = "Inserire massimo 240 caratteri")]
[DataType(DataType.Text)]
public string? TcAttributi1  { get; set; }

[Display(Name = "Attributi2", ShortName="", Description = "Ulteriori flag operativi, gestiti dalle applicazioni", Prompt="")]
[ErpDogField("TC_ATTRIBUTI2", SqlFieldNameExt="TC_ATTRIBUTI2", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(240, ErrorMessage = "Inserire massimo 240 caratteri")]
[DataType(DataType.Text)]
public string? TcAttributi2  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioTc1Icode|K|Tc1Icode","sioTc1RecDate|N|Tc1Mdate,Tc1Cdate"
        ,"sioTcIdCategoriaDatoClinico|N|TcIdCategoriaDatoClinico"
        ,"sioTcIdGruppoTcSequenza|N|TcIdGruppo,TcSequenza"
        ,"sioTc1VersionTc1Deleted|U|Tc1Version,Tc1Deleted"
        ,"sioTcCodiceTc1VersionTc1Deleted|U|TcCodice,Tc1Version,Tc1Deleted"
    };
}
}
}
