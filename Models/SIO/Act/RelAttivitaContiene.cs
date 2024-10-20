using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class RelAttivitaContiene : ModelErp {
public const string Description = "Corrispondenze tra tassonomie di attività";
public const string SqlTableName = "REL_ATTIVITA_CONTIENE";
public const string SqlTableNameExt = "REL_ATTIVITA_CONTIENE";
public const string SqlRowIdName = "AA__ID";
public const string SqlRowIdNameExt = "AA__ICODE";
public const string SqlPrefix = "AA_";
public const string SqlPrefixExt = "AA_";
public const string SqlXdataTableName = "AA_XDATA";
public const string SqlXdataTableNameExt = "AA_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
public const int INTCODE = 206; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Aa"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "Y"; //Is Relation Table: Yes or No

public char? action = null; public IDictionary<string, string> options = new Dictionary<string, string>();  // proprietà necessarie per la mantain del record

[Display(Name = "Aa1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("AA__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Aa1Ienv { get; set; }
[Key]
[Display(Name = "Aa1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("AA__ICODE", SqlFieldNameExt="AA__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Aa1Icode { get; set; }
[Display(Name = "Aa1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("AA__DELETED", SqlFieldNameExt="AA__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Aa1Deleted { get; set; }
[Display(Name = "Aa1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("AA__TIMESTAMP", SqlFieldNameExt="AA__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
//[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Aa1Timestamp { get; set; }
[Display(Name = "Aa1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("AA__HOME", SqlFieldNameExt="AA__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Aa1Home { get; set; }
[Display(Name = "Aa1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("AA__VERSION", SqlFieldNameExt="AA__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Aa1Version { get; set; }
[Display(Name = "Aa1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("AA__INACTIVE", SqlFieldNameExt="AA__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Aa1Inactive { get; set; }
[Display(Name = "Aa1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("AA__EXTATT", SqlFieldNameExt="AA__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Aa1Extatt { get; set; }


[Display(Name = "Id Attivita Padre", ShortName="", Description = "Identificatore del tipo di attività della prima tassonomia (cioè quella che viene aggregata)", Prompt="")]
[ErpDogField("AA_ID_ATTIVITA_PADRE", SqlFieldNameExt="AA_ID_ATTIVITA_PADRE", SqlFieldOptions="", Xref="Av1Icode", SqlFieldProperties="prop() xref(ATTIVITA.AV__ICODE) xdup() multbxref()")]
[AutocompleteClient("Attivita", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? AaIdAttivitaPadre  { get; set; }
public ErpToolkit.Models.SIO.Act.Attivita? AaIdAttivitaPadreObj  { get; set; }

[Display(Name = "Id Attivita Figlio", ShortName="", Description = "Identificatore del tipo di attività in cui la prima è stata aggregata", Prompt="")]
[ErpDogField("AA_ID_ATTIVITA_FIGLIO", SqlFieldNameExt="AA_ID_ATTIVITA_FIGLIO", SqlFieldOptions="", Xref="Av1Icode", SqlFieldProperties="prop() xref(ATTIVITA.AV__ICODE) xdup() multbxref()")]
[AutocompleteClient("Attivita", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? AaIdAttivitaFiglio  { get; set; }
public ErpToolkit.Models.SIO.Act.Attivita? AaIdAttivitaFiglioObj  { get; set; }

[Display(Name = "Sequenza", ShortName="", Description = "Numero di sequenza di TOAY rispetto a FROMAY", Prompt="")]
[ErpDogField("AA_SEQUENZA", SqlFieldNameExt="AA_SEQUENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
public short? AaSequenza  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note testuali", Prompt="")]
[ErpDogField("AA_NOTE", SqlFieldNameExt="AA_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(80, ErrorMessage = "Inserire massimo 80 caratteri")]
[DataType(DataType.Text)]
public string? AaNote  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioAa1Icode|K|AA__ICODE","sioAa1RecDate|N|AA__MDATE,AA__CDATE"
        ,"sioAaIdAttivitaPadre|N|AA_ID_ATTIVITA_PADRE"
        ,"sioAaIdAttivitaFiglio|N|AA_ID_ATTIVITA_FIGLIO"
    };
}
}
}
