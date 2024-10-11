using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class SelTipoCampione : ModelErp {
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
public const string CATEG = "SEL"; //Data Model Name of the Class
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
[ErpDogField("TP_CODICE", SqlFieldNameExt="TP_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_CAMPIONE.TP__ICODE[TP__ICODE] {TP_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? TpCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TP_DESCRIZIONE", SqlFieldNameExt="TP_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TpDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TP_NOTE", SqlFieldNameExt="TP_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TpNote  { get; set; }

[Display(Name = "Contesto", ShortName="", Description = "Identificazione del contesto o dei contesti in cui il tipo di campione ha particolare rilevanza", Prompt="")]
[ErpDogField("TP_CONTESTO", SqlFieldNameExt="TP_CONTESTO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TpContesto  { get; set; }

[Display(Name = "Contenitore", ShortName="", Description = "Descrizione del contenitore", Prompt="")]
[ErpDogField("TP_CONTENITORE", SqlFieldNameExt="TP_CONTENITORE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TpContenitore  { get; set; }

[Display(Name = "Attributi", ShortName="", Description = "Flag operativi, gestiti dall'applicazione", Prompt="")]
[ErpDogField("TP_ATTRIBUTI", SqlFieldNameExt="TP_ATTRIBUTI", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? TpAttributi  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        // verifica se almeno un campo indicizzato è valorizzato (test per validazioni complesse del modello) 
        bool found = false; 
        foreach (var idx in ListIndexes()) { 
            string fldLst = idx.Split("|")[2]; 
            foreach (var fld in fldLst.Split(",")) { 
                if (DogHelper.getPropertyValue(this, fld.Trim()) != null) found = true; 
                if (DogHelper.getPropertyValue(this, fld.Trim() + "[0]") != null) found = true; 
                if (DogHelper.getPropertyValue(this, fld.Trim() + ".StartDate") != null) found = true; 
                if (DogHelper.getPropertyValue(this, fld.Trim() + ".EndDate") != null) found = true; 
            } 
        } 
        if (!found) { isValidate = false;  modelState.AddModelError(string.Empty, "Deve essere compilato almeno un campo indicizzato."); } 
        //-- 
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
