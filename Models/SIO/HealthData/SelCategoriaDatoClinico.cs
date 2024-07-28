using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.HealthData {
public class SelCategoriaDatoClinico {
public const string Description = "Classificazione dei tipi di dati sanitari ... intcode:[16] prefix:[CC_] has_xdt:[CC_XDATA] is_xdt:[0] ";
public const string SqlTableName = "CATEGORIA_DATO_CLINICO";
public const string SqlTableNameExt = "CATEGORIA_DATO_CLINICO";
public const string SqlRowIdName = "CC__ID";
public const string SqlRowIdNameExt = "CC__ICODE";
public const string SqlPrefix = "CC_";
public const string SqlPrefixExt = "CC_";
public const string SqlXdataTableName = "CC_XDATA";
public const string SqlXdataTableNameExt = "CC_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 16; //Internal Table Code
public const string TBAREA = "Dati clinici"; //Table Area
public const string PREFIX = "Cc"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("CC_CODICE", SqlFieldNameExt="CC_CODICE", SqlFieldProperties="prop() xref() xdup(CATEGORIA_DATO_CLINICO.CC__ICODE[CC__ICODE] {CC_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? CcCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("CC_DESCRIZIONE", SqlFieldNameExt="CC_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? CcDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("CC_NOTE", SqlFieldNameExt="CC_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? CcNote  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Codice della superclasse che raggruppa la classe attuale", Prompt="")]
[ErpDogField("CC_ID_GRUPPO", SqlFieldNameExt="CC_ID_GRUPPO", SqlFieldProperties="prop() xref(CATEGORIA_DATO_CLINICO.CC__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("CategoriaDatoClinico", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> CcIdGruppo  { get; set; } = new List<string>();
}
}
