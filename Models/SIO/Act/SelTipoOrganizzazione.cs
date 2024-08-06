using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class SelTipoOrganizzazione {
public const string Description = "Classificazione delle strutture";
public const string SqlTableName = "TIPO_ORGANIZZAZIONE";
public const string SqlTableNameExt = "TIPO_ORGANIZZAZIONE";
public const string SqlRowIdName = "TZ__ID";
public const string SqlRowIdNameExt = "TZ__ICODE";
public const string SqlPrefix = "TZ_";
public const string SqlPrefixExt = "TZ_";
public const string SqlXdataTableName = "TZ_XDATA";
public const string SqlXdataTableNameExt = "TZ_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 91; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Tz"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TZ_CODICE", SqlFieldNameExt="TZ_CODICE", SqlFieldOptions="[UID]", SqlFieldProperties="prop() xref() xdup(TIPO_ORGANIZZAZIONE.TZ__ICODE[TZ__ICODE] {TZ_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? TzCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TZ_DESCRIZIONE", SqlFieldNameExt="TZ_DESCRIZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TzDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TZ_NOTE", SqlFieldNameExt="TZ_NOTE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TzNote  { get; set; }

[Display(Name = "Gruppo", ShortName="", Description = "Classe di aggregazione (se presente)", Prompt="")]
[ErpDogField("TZ_GRUPPO", SqlFieldNameExt="TZ_GRUPPO", SqlFieldOptions="", SqlFieldProperties="prop() xref(TIPO_ORGANIZZAZIONE.TZ__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoOrganizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> TzGruppo  { get; set; } = new List<string>();

[Display(Name = "Sequenza", ShortName="", Description = "Numero di sequenza nell'aggregazione (se presente)", Prompt="")]
[ErpDogField("TZ_SEQUENZA", SqlFieldNameExt="TZ_SEQUENZA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? TzSequenza  { get; set; }
}
}
