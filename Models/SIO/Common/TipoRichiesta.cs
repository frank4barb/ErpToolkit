using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Common {
public class TipoRichiesta : ModelErp {
public const string Description = "Tipo di richieste";
public const string SqlTableName = "TIPO_RICHIESTA";
public const string SqlTableNameExt = "TIPO_RICHIESTA";
public const string SqlRowIdName = "TI__ID";
public const string SqlRowIdNameExt = "TI__ICODE";
public const string SqlPrefix = "TI_";
public const string SqlPrefixExt = "TI_";
public const string SqlXdataTableName = "TI_XDATA";
public const string SqlXdataTableNameExt = "TI_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 48; //Internal Table Code
public const string TBAREA = "Organizzazione ospedaliera"; //Table Area
public const string PREFIX = "Ti"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
[Display(Name = "Ti1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("TI__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Ti1Ienv { get; set; }
[Key]
[Display(Name = "Ti1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("TI__ICODE", SqlFieldNameExt="TI__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ti1Icode { get; set; }
[Display(Name = "Ti1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("TI__DELETED", SqlFieldNameExt="TI__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ti1Deleted { get; set; }
[Display(Name = "Ti1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("TI__TIMESTAMP", SqlFieldNameExt="TI__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Ti1Timestamp { get; set; }
[Display(Name = "Ti1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("TI__HOME", SqlFieldNameExt="TI__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ti1Home { get; set; }
[Display(Name = "Ti1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("TI__VERSION", SqlFieldNameExt="TI__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Ti1Version { get; set; }
[Display(Name = "Ti1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("TI__INACTIVE", SqlFieldNameExt="TI__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Ti1Inactive { get; set; }
[Display(Name = "Ti1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("TI__EXTATT", SqlFieldNameExt="TI__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Ti1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TI_CODICE", SqlFieldNameExt="TI_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_RICHIESTA.TI__ICODE[TI__ICODE] {TI_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TiCodice  { get; set; }

[Display(Name = "Gruppo", ShortName="", Description = "Classe di comunicazione: 0 = Comunicazioni di sistema 1 = Messaggi utente - 2 = Relativi agli atti - Z = Utente-d", Prompt="")]
[ErpDogField("TI_GRUPPO", SqlFieldNameExt="TI_GRUPPO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "0", "1", "2", "Z" }, MaxSelections=1, LabelClassName="")]
public string? TiGruppo  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione", Prompt="")]
[ErpDogField("TI_DESCRIZIONE", SqlFieldNameExt="TI_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TiDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TI_NOTE", SqlFieldNameExt="TI_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TiNote  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioTi1Icode|K|Ti1Icode","sioTi1RecDate|N|Ti1Mdate,Ti1Cdate"
        ,"sioTiGruppoTi1VersionTi1Deleted|U|TiGruppo,Ti1Version,Ti1Deleted"
        ,"sioTiCodiceTi1VersionTi1Deleted|U|TiCodice,Ti1Version,Ti1Deleted"
    };
}
}
}
