using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class TipoCampione : ModelErp {
public const string Description = "Tipo di campione";
public const string SqlTableName = "TIPO_CAMPIONE";
public const string SqlTableNameExt = "TIPO_CAMPIONE";
public const string SqlRowIdName = "TP__ID";
public const string SqlRowIdNameExt = "TP__ICODE";
public const string SqlPrefix = "TP_";
public const string SqlPrefixExt = "TP_";
public const string SqlXdataTableName = "TP_XDATA";
public const string SqlXdataTableNameExt = "TP_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
public const int INTCODE = 100; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Tp"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//123-119//REL_PRESTAZIONE_CAMPIONE.PC_ID_TIPO_CAMPIONE
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneCampione> RelPrestazioneCampione4PcIdTipoCampione  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneCampione>();
//371-370//REL_ATTIVITA_TIPO_CAMPIONE.AC_ID_TIPO_CAMPIONE
public List<ErpToolkit.Models.SIO.Act.RelAttivitaTipoCampione> RelAttivitaTipoCampione4AcIdTipoCampione  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaTipoCampione>();
[Display(Name = "Tp1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("TP__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Tp1Ienv { get; set; }
[Key]
[Display(Name = "Tp1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("TP__ICODE", SqlFieldNameExt="TP__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Tp1Icode { get; set; }
[Display(Name = "Tp1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("TP__DELETED", SqlFieldNameExt="TP__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Tp1Deleted { get; set; }
[Display(Name = "Tp1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("TP__TIMESTAMP", SqlFieldNameExt="TP__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Tp1Timestamp { get; set; }
[Display(Name = "Tp1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("TP__HOME", SqlFieldNameExt="TP__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Tp1Home { get; set; }
[Display(Name = "Tp1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("TP__VERSION", SqlFieldNameExt="TP__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Tp1Version { get; set; }
[Display(Name = "Tp1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("TP__INACTIVE", SqlFieldNameExt="TP__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Tp1Inactive { get; set; }
[Display(Name = "Tp1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("TP__EXTATT", SqlFieldNameExt="TP__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Tp1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TP_CODICE", SqlFieldNameExt="TP_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_CAMPIONE.TP__ICODE[TP__ICODE] {TP_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TpCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TP_DESCRIZIONE", SqlFieldNameExt="TP_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TpDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TP_NOTE", SqlFieldNameExt="TP_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TpNote  { get; set; }

[Display(Name = "Contesto", ShortName="", Description = "Identificazione del contesto o dei contesti in cui il tipo di campione ha particolare rilevanza", Prompt="")]
[ErpDogField("TP_CONTESTO", SqlFieldNameExt="TP_CONTESTO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(80, ErrorMessage = "Inserire massimo 80 caratteri")]
[DataType(DataType.Text)]
public string? TpContesto  { get; set; }

[Display(Name = "Contenitore", ShortName="", Description = "Descrizione del contenitore", Prompt="")]
[ErpDogField("TP_CONTENITORE", SqlFieldNameExt="TP_CONTENITORE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TpContenitore  { get; set; }

[Display(Name = "Attributi", ShortName="", Description = "Flag operativi, gestiti dall'applicazione", Prompt="")]
[ErpDogField("TP_ATTRIBUTI", SqlFieldNameExt="TP_ATTRIBUTI", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TpAttributi  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioTp1Icode|K|Tp1Icode","sioTp1RecDate|N|Tp1Mdate,Tp1Cdate"
        ,"sioTpContesto|N|TpContesto"
        ,"sioTp1VersionTp1Deleted|U|Tp1Version,Tp1Deleted"
        ,"sioTpCodiceTp1VersionTp1Deleted|U|TpCodice,Tp1Version,Tp1Deleted"
    };
}
}
}
