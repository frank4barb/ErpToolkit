using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class SelTipoCampione {
public const string Description = "Tipo di campione ... intcode:[100] prefix:[TP_] has_xdt:[TP_XDATA] is_xdt:[0] ";
public const string SqlTableName = "TIPO_CAMPIONE";
public const string SqlTableNameExt = "TIPO_CAMPIONE";
public const string SqlRowIdName = "TP__ID";
public const string SqlRowIdNameExt = "TP__ICODE";
public const string SqlPrefix = "TP_";
public const string SqlPrefixExt = "TP_";
public const string SqlXdataTableName = "TP_XDATA";
public const string SqlXdataTableNameExt = "TP_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 100; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Tp"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//123-119//REL_PRESTAZIONE_CAMPIONE.PC_ID_TIPO_CAMPIONE
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneCampione> RelPrestazioneCampione4PcIdTipoCampione  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneCampione>();
//371-370//REL_ATTIVITA_TIPO_CAMPIONE.AC_ID_TIPO_CAMPIONE
public List<ErpToolkit.Models.SIO.Act.RelAttivitaTipoCampione> RelAttivitaTipoCampione4AcIdTipoCampione  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaTipoCampione>();

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TP_CODICE", SqlFieldNameExt="TP_CODICE", SqlFieldProperties="prop() xref() xdup(TIPO_CAMPIONE.TP__ICODE[TP__ICODE] {TP_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? TpCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TP_DESCRIZIONE", SqlFieldNameExt="TP_DESCRIZIONE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TpDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TP_NOTE", SqlFieldNameExt="TP_NOTE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TpNote  { get; set; }

[Display(Name = "Contesto", ShortName="", Description = "Identificazione del contesto o dei contesti in cui il tipo di campione ha particolare rilevanza", Prompt="")]
[ErpDogField("TP_CONTESTO", SqlFieldNameExt="TP_CONTESTO", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(80, ErrorMessage = "Inserire massimo 80 caratteri")]
[DataType(DataType.Text)]
public string? TpContesto  { get; set; }

[Display(Name = "Contenitore", ShortName="", Description = "Descrizione del contenitore", Prompt="")]
[ErpDogField("TP_CONTENITORE", SqlFieldNameExt="TP_CONTENITORE", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? TpContenitore  { get; set; }

[Display(Name = "Attributi", ShortName="", Description = "Flag operativi, gestiti dall'applicazione", Prompt="")]
[ErpDogField("TP_ATTRIBUTI", SqlFieldNameExt="TP_ATTRIBUTI", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? TpAttributi  { get; set; }
}
}
