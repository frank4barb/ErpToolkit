using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Patient {
public class SelComune {
public const string Description = "Comuni ... intcode:[55] prefix:[CM_] has_xdt:[CM_XDATA] is_xdt:[0] ";
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

[Display(Name = "Codice", ShortName="", Description = "Codice nazionale della città", Prompt="")]
[ErpDogField("CM_CODICE", SqlFieldNameExt="CM_CODICE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? CmCodice  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Nome esteso", Prompt="")]
[ErpDogField("CM_NOME", SqlFieldNameExt="CM_NOME", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? CmNome  { get; set; }

[Display(Name = "Cod Istat", ShortName="", Description = "Codice statistico per la città", Prompt="")]
[ErpDogField("CM_COD_ISTAT", SqlFieldNameExt="CM_COD_ISTAT", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? CmCodIstat  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note testuali", Prompt="")]
[ErpDogField("CM_NOTE", SqlFieldNameExt="CM_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? CmNote  { get; set; }
}
}
