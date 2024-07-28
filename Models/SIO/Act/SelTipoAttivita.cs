using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class SelTipoAttivita {
public const string Description = "Tassonomie e classe di tipi di attività ... intcode:[3] prefix:[TA_] has_xdt:[TA_XDATA] is_xdt:[0] ";
public const string SqlTableName = "TIPO_ATTIVITA";
public const string SqlTableNameExt = "TIPO_ATTIVITA";
public const string SqlRowIdName = "TA__ID";
public const string SqlRowIdNameExt = "TA__ICODE";
public const string SqlPrefix = "TA_";
public const string SqlPrefixExt = "TA_";
public const string SqlXdataTableName = "TA_XDATA";
public const string SqlXdataTableNameExt = "TA_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 3; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Ta"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TA_CODICE", SqlFieldNameExt="TA_CODICE", SqlFieldProperties="prop() xref() xdup(TIPO_ATTIVITA.TA__ICODE[TA__ICODE] {TA_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TaCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TA_DESCRIZIONE", SqlFieldNameExt="TA_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TaDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TA_NOTE", SqlFieldNameExt="TA_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TaNote  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Superclasse che raggruppa la classificazione corrente", Prompt="")]
[ErpDogField("TA_ID_GRUPPO", SqlFieldNameExt="TA_ID_GRUPPO", SqlFieldProperties="prop() xref(TIPO_ATTIVITA.TA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoAttivita", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> TaIdGruppo  { get; set; } = new List<string>();
}
}
