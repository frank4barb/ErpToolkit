using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class TipoEpisodio : ModelErp {
public const string Description = "Classe di episodi";
public const string SqlTableName = "TIPO_EPISODIO";
public const string SqlTableNameExt = "TIPO_EPISODIO";
public const string SqlRowIdName = "TE__ID";
public const string SqlRowIdNameExt = "TE__ICODE";
public const string SqlPrefix = "TE_";
public const string SqlPrefixExt = "TE_";
public const string SqlXdataTableName = "TE_XDATA";
public const string SqlXdataTableNameExt = "TE_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
public const int INTCODE = 6; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Te"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

public char? action = null; public IDictionary<string, string> options = new Dictionary<string, string>();  // proprietà necessarie per la mantain del record

[Display(Name = "Te1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("TE__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Te1Ienv { get; set; }
[Key]
[Display(Name = "Te1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("TE__ICODE", SqlFieldNameExt="TE__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Te1Icode { get; set; }
[Display(Name = "Te1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("TE__DELETED", SqlFieldNameExt="TE__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Te1Deleted { get; set; }
[Display(Name = "Te1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("TE__TIMESTAMP", SqlFieldNameExt="TE__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
//[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Te1Timestamp { get; set; }
[Display(Name = "Te1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("TE__HOME", SqlFieldNameExt="TE__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Te1Home { get; set; }
[Display(Name = "Te1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("TE__VERSION", SqlFieldNameExt="TE__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Te1Version { get; set; }
[Display(Name = "Te1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("TE__INACTIVE", SqlFieldNameExt="TE__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Te1Inactive { get; set; }
[Display(Name = "Te1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("TE__EXTATT", SqlFieldNameExt="TE__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Te1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TE_CODICE", SqlFieldNameExt="TE_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_EPISODIO.TE__ICODE[TE__ICODE] {TE_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TeCodice  { get; set; }

[Display(Name = "Classe", ShortName="", Description = "Classe di contatto 1=Ricovero - 2=Day-hospital - 3=Ambulatorio", Prompt="")]
[ErpDogField("TE_CLASSE", SqlFieldNameExt="TE_CLASSE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }, MaxSelections=1, LabelClassName="")]
public string? TeClasse  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TE_DESCRIZIONE", SqlFieldNameExt="TE_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TeDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TE_NOTE", SqlFieldNameExt="TE_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TeNote  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioTe1Icode|K|TE__ICODE","sioTe1RecDate|N|TE__MDATE,TE__CDATE"
        ,"sioTeClassete1Versionte1Deleted|U|TE_CLASSE,TE__VERSION,TE__DELETED"
        ,"sioTeCodicete1Versionte1Deleted|U|TE_CODICE,TE__VERSION,TE__DELETED"
    };
}
}
}
