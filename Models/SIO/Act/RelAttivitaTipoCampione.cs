using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class RelAttivitaTipoCampione : ModelErp {
public const string Description = "Tipo di campione rilevante per un certo tipo di attività";
public const string SqlTableName = "REL_ATTIVITA_TIPO_CAMPIONE";
public const string SqlTableNameExt = "REL_ATTIVITA_TIPO_CAMPIONE";
public const string SqlRowIdName = "AC__ID";
public const string SqlRowIdNameExt = "AC__ICODE";
public const string SqlPrefix = "AC_";
public const string SqlPrefixExt = "AC_";
public const string SqlXdataTableName = "AC_XDATA";
public const string SqlXdataTableNameExt = "AC_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
public const int INTCODE = 10; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Ac"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "Y"; //Is Relation Table: Yes or No

public char? action = null; public IDictionary<string, string> options = new Dictionary<string, string>();  // proprietà necessarie per la mantain del record

[Display(Name = "Ac1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("AC__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Ac1Ienv { get; set; }
[Key]
[Display(Name = "Ac1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("AC__ICODE", SqlFieldNameExt="AC__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ac1Icode { get; set; }
[Display(Name = "Ac1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("AC__DELETED", SqlFieldNameExt="AC__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ac1Deleted { get; set; }
[Display(Name = "Ac1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("AC__TIMESTAMP", SqlFieldNameExt="AC__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Ac1Timestamp { get; set; }
[Display(Name = "Ac1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("AC__HOME", SqlFieldNameExt="AC__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ac1Home { get; set; }
[Display(Name = "Ac1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("AC__VERSION", SqlFieldNameExt="AC__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ac1Version { get; set; }
[Display(Name = "Ac1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("AC__INACTIVE", SqlFieldNameExt="AC__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ac1Inactive { get; set; }
[Display(Name = "Ac1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("AC__EXTATT", SqlFieldNameExt="AC__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Ac1Extatt { get; set; }


[Display(Name = "Id Attivita", ShortName="", Description = "Codice del tipo di attività", Prompt="")]
[ErpDogField("AC_ID_ATTIVITA", SqlFieldNameExt="AC_ID_ATTIVITA", SqlFieldOptions="", Xref="Av1Icode", SqlFieldProperties="prop() xref(ATTIVITA.AV__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteClient("Attivita", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? AcIdAttivita  { get; set; }
public ErpToolkit.Models.SIO.Act.Attivita? AcIdAttivitaObj  { get; set; }

[Display(Name = "Id Tipo Campione", ShortName="", Description = "Codice del tipo di campione", Prompt="")]
[ErpDogField("AC_ID_TIPO_CAMPIONE", SqlFieldNameExt="AC_ID_TIPO_CAMPIONE", SqlFieldOptions="", Xref="Tp1Icode", SqlFieldProperties="prop() xref(TIPO_CAMPIONE.TP__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteClient("TipoCampione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? AcIdTipoCampione  { get; set; }
public ErpToolkit.Models.SIO.Act.TipoCampione? AcIdTipoCampioneObj  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("AC_NOTE", SqlFieldNameExt="AC_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? AcNote  { get; set; }

[Display(Name = "Tipo", ShortName="", Description = "Generato da / Necessario per l'esecuzione", Prompt="")]
[ErpDogField("AC_TIPO", SqlFieldNameExt="AC_TIPO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("E")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "G", "E" }, MaxSelections=1, LabelClassName="")]
public string? AcTipo  { get; set; }

[Display(Name = "Campione Preferenziale", ShortName="", Description = "Tipo di campione preferenziale (predefinito) per quel tipo di attività", Prompt="")]
[ErpDogField("AC_CAMPIONE_PREFERENZIALE", SqlFieldNameExt="AC_CAMPIONE_PREFERENZIALE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("N")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "Y", "N" }, MaxSelections=1, LabelClassName="")]
public string? AcCampionePreferenziale  { get; set; }

[Display(Name = "Campione Specifico", ShortName="", Description = "Se 'Y', è necessario un campione dedicato, e il campione non può essere condiviso tra diverse attività (predefinito N)", Prompt="")]
[ErpDogField("AC_CAMPIONE_SPECIFICO", SqlFieldNameExt="AC_CAMPIONE_SPECIFICO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("N")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "Y", "N" }, MaxSelections=1, LabelClassName="")]
public string? AcCampioneSpecifico  { get; set; }

[Display(Name = "Regole Campionamento", ShortName="", Description = "Criteri da adottare quando si raccolgono più campioni (informazioni testuali, dedicate dall'utente)", Prompt="")]
[ErpDogField("AC_REGOLE_CAMPIONAMENTO", SqlFieldNameExt="AC_REGOLE_CAMPIONAMENTO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(240, ErrorMessage = "Inserire massimo 240 caratteri")]
[DataType(DataType.Text)]
public string? AcRegoleCampionamento  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioAc1Icode|K|Ac1Icode","sioAc1RecDate|N|Ac1Mdate,Ac1Cdate"
        ,"sioAcIdAttivitaAcTipo|N|AcIdAttivita,AcTipo"
        ,"sioAcIdTipoCampione|N|AcIdTipoCampione"
    };
}
}
}
