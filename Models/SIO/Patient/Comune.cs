using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class Comune {
public const string Description = "Comuni";
public const string SqlTableName = "COMUNE";
public const string SqlTableNameExt = "COMUNE";
public const string SqlRowIdName = "CM__ID";
public const string SqlRowIdNameExt = "CM__ICODE";
public const string SqlPrefix = "CM_";
public const string SqlPrefixExt = "CM_";
public const string SqlXdataTableName = "CM_XDATA";
public const string SqlXdataTableNameExt = "CM_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 55; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Cm"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
[Display(Name = "Cm1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("CM__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Cm1Ienv { get; set; }
[Key]
[Display(Name = "Cm1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("CM__ICODE", SqlFieldNameExt="CM__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Cm1Icode { get; set; }
[Display(Name = "Cm1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("CM__DELETED", SqlFieldNameExt="CM__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Cm1Deleted { get; set; }
[Display(Name = "Cm1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("CM__TIMESTAMP", SqlFieldNameExt="CM__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Cm1Timestamp { get; set; }
[Display(Name = "Cm1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("CM__HOME", SqlFieldNameExt="CM__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Cm1Home { get; set; }
[Display(Name = "Cm1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("CM__VERSION", SqlFieldNameExt="CM__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Cm1Version { get; set; }
[Display(Name = "Cm1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("CM__INACTIVE", SqlFieldNameExt="CM__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Cm1Inactive { get; set; }
[Display(Name = "Cm1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("CM__EXTATT", SqlFieldNameExt="CM__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Cm1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice nazionale della città", Prompt="")]
[ErpDogField("CM_CODICE", SqlFieldNameExt="CM_CODICE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? CmCodice  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Nome esteso", Prompt="")]
[ErpDogField("CM_NOME", SqlFieldNameExt="CM_NOME", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? CmNome  { get; set; }

[Display(Name = "Cod Istat", ShortName="", Description = "Codice statistico per la città", Prompt="")]
[ErpDogField("CM_COD_ISTAT", SqlFieldNameExt="CM_COD_ISTAT", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? CmCodIstat  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note testuali", Prompt="")]
[ErpDogField("CM_NOTE", SqlFieldNameExt="CM_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? CmNote  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioCm1Icode|K|Cm1Icode","sioCm1RecDate|N|Cm1Mdate,Cm1Cdate"
        ,"sioCmNome|N|CmNome"
    };
}
}
}
