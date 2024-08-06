using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Costs {
public class SelTipoDiagnosi {
public const string Description = "Tipi generali di classificazioni diagnostiche.";
public const string SqlTableName = "TIPO_DIAGNOSI";
public const string SqlTableNameExt = "TIPO_DIAGNOSI";
public const string SqlRowIdName = "TD__ID";
public const string SqlRowIdNameExt = "TD__ICODE";
public const string SqlPrefix = "TD_";
public const string SqlPrefixExt = "TD_";
public const string SqlXdataTableName = "TD_XDATA";
public const string SqlXdataTableNameExt = "TD_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 114; //Internal Table Code
public const string TBAREA = "Controllo di gestione"; //Table Area
public const string PREFIX = "Td"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TD_CODICE", SqlFieldNameExt="TD_CODICE", SqlFieldOptions="[UID]", SqlFieldProperties="prop() xref() xdup(TIPO_DIAGNOSI.TD__ICODE[TD__ICODE] {TD_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? TdCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TD_DESCRIZIONE", SqlFieldNameExt="TD_DESCRIZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TdDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TD_NOTE", SqlFieldNameExt="TD_NOTE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TdNote  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Superclasse che raggruppa la classificazione corrente", Prompt="")]
[ErpDogField("TD_ID_GRUPPO", SqlFieldNameExt="TD_ID_GRUPPO", SqlFieldOptions="", SqlFieldProperties="prop() xref(TIPO_DIAGNOSI.TD__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoDiagnosi", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> TdIdGruppo  { get; set; } = new List<string>();
}
}
