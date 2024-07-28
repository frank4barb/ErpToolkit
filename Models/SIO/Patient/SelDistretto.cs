using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class SelDistretto {
public const string Description = "Distretto territoriale (circoscrizione) ... intcode:[128] prefix:[DI_] has_xdt:[DI_XDATA] is_xdt:[0] ";
public const string SqlTableName = "DISTRETTO";
public const string SqlTableNameExt = "DISTRETTO";
public const string SqlRowIdName = "DI__ID";
public const string SqlRowIdNameExt = "DI__ICODE";
public const string SqlPrefix = "DI_";
public const string SqlPrefixExt = "DI_";
public const string SqlXdataTableName = "DI_XDATA";
public const string SqlXdataTableNameExt = "DI_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 128; //Internal Table Code
public const string TBAREA = "Accoglienza"; //Table Area
public const string PREFIX = "Di"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice utente del distretto (CAP)", Prompt="")]
[ErpDogField("DI_CODICE", SqlFieldNameExt="DI_CODICE", SqlFieldProperties="prop() xref() xdup(DISTRETTO.DI__ICODE[DI__ICODE] {DI_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? DiCodice  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Descrizione estesa del distretto", Prompt="")]
[ErpDogField("DI_NOME", SqlFieldNameExt="DI_NOME", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? DiNome  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note sul distretto", Prompt="")]
[ErpDogField("DI_NOTE", SqlFieldNameExt="DI_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? DiNote  { get; set; }

[Display(Name = "Id Comune", ShortName="", Description = "Città in cui si trova il distretto", Prompt="")]
[ErpDogField("DI_ID_COMUNE", SqlFieldNameExt="DI_ID_COMUNE", SqlFieldProperties="prop() xref(COMUNE.CM__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Comune", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> DiIdComune  { get; set; } = new List<string>();
}
}
