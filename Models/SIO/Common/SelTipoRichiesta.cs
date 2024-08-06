using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Common {
public class SelTipoRichiesta {
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

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TI_CODICE", SqlFieldNameExt="TI_CODICE", SqlFieldProperties="prop() xref() xdup(TIPO_RICHIESTA.TI__ICODE[TI__ICODE] {TI_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? TiCodice  { get; set; }

[Display(Name = "Gruppo", ShortName="", Description = "Classe di comunicazione: 0 = Comunicazioni di sistema 1 = Messaggi utente - 2 = Relativi agli atti - Z = Utente-d", Prompt="")]
[ErpDogField("TI_GRUPPO", SqlFieldNameExt="TI_GRUPPO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("0|1|2|Z", ErrorMessage = "Inserisci una delle seguenti opzioni: 0|1|2|Z")]
public string? TiGruppo  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione", Prompt="")]
[ErpDogField("TI_DESCRIZIONE", SqlFieldNameExt="TI_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TiDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TI_NOTE", SqlFieldNameExt="TI_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TiNote  { get; set; }
}
}
