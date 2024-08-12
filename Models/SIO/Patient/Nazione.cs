using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class Nazione {
public const string Description = "Nazioni";
public const string SqlTableName = "NAZIONE";
public const string SqlTableNameExt = "NAZIONE";
public const string SqlRowIdName = "NZ__ID";
public const string SqlRowIdNameExt = "NZ__ICODE";
public const string SqlPrefix = "NZ_";
public const string SqlPrefixExt = "NZ_";
public const string SqlXdataTableName = "NZ_XDATA";
public const string SqlXdataTableNameExt = "NZ_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 58; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Nz"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
[Display(Name = "Nz1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("NZ__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Nz1Ienv { get; set; }
[Key]
[Display(Name = "Nz1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("NZ__ICODE", SqlFieldNameExt="NZ__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Nz1Icode { get; set; }
[Display(Name = "Nz1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("NZ__DELETED", SqlFieldNameExt="NZ__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Nz1Deleted { get; set; }
[Display(Name = "Nz1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("NZ__TIMESTAMP", SqlFieldNameExt="NZ__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Nz1Timestamp { get; set; }
[Display(Name = "Nz1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("NZ__HOME", SqlFieldNameExt="NZ__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Nz1Home { get; set; }
[Display(Name = "Nz1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("NZ__VERSION", SqlFieldNameExt="NZ__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Nz1Version { get; set; }
[Display(Name = "Nz1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("NZ__INACTIVE", SqlFieldNameExt="NZ__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Nz1Inactive { get; set; }
[Display(Name = "Nz1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("NZ__EXTATT", SqlFieldNameExt="NZ__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Nz1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice ufficiale (esterno) del paese", Prompt="")]
[ErpDogField("NZ_CODICE", SqlFieldNameExt="NZ_CODICE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? NzCodice  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Nome esteso", Prompt="")]
[ErpDogField("NZ_NOME", SqlFieldNameExt="NZ_NOME", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
[StringLength(25, ErrorMessage = "Inserire massimo 25 caratteri")]
[DataType(DataType.Text)]
public string? NzNome  { get; set; }

[Display(Name = "Cod Istat", ShortName="", Description = "Codice statistico", Prompt="")]
[ErpDogField("NZ_COD_ISTAT", SqlFieldNameExt="NZ_COD_ISTAT", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? NzCodIstat  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("NZ_NOTE", SqlFieldNameExt="NZ_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? NzNote  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioNz1Icode|K|Nz1Icode","sioNz1RecDate|N|Nz1Mdate,Nz1Cdate"
        ,"sioNzNomeNz1VersionNz1Deleted|U|NzNome,Nz1Version,Nz1Deleted"
        ,"sioNzCodiceNz1VersionNz1Deleted|U|NzCodice,Nz1Version,Nz1Deleted"
    };
}
}
}
