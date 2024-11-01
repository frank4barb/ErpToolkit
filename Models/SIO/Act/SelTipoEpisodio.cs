using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class SelTipoEpisodio : ModelErp {
public const string Description = "Classe di episodi";
public const string SqlTableName = "TIPO_EPISODIO";
public const string SqlTableNameExt = "TIPO_EPISODIO";
public const string SqlRowIdName = "TE__ID";
public const string SqlRowIdNameExt = "TE__ICODE";
public const string SqlPrefix = "TE_";
public const string SqlPrefixExt = "TE_";
public const string SqlXdataTableName = "TE_XDATA";
public const string SqlXdataTableNameExt = "TE_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "SEL"; //Data Model Name of the Class
public const int INTCODE = 6; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Te"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TE_CODICE", SqlFieldNameExt="TE_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_EPISODIO.TE__ICODE[TE__ICODE] {TE_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? SelTeCodice  { get; set; }

[Display(Name = "Classe", ShortName="", Description = "Classe di contatto 1=Ricovero - 2=Day-hospital - 3=Ambulatorio", Prompt="")]
[ErpDogField("TE_CLASSE", SqlFieldNameExt="TE_CLASSE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[MultipleChoices(new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }, MaxSelections=-1, LabelClassName="")]
[DataType(DataType.Text)]
public List<string> SelTeClasse  { get; set; } = new List<string>();

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("TE_DESCRIZIONE", SqlFieldNameExt="TE_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelTeDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TE_NOTE", SqlFieldNameExt="TE_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelTeNote  { get; set; }

public override bool TryValidateInt(ModelStateDictionary modelState, string? prefix = null) 
    { 
        bool isValidate = true; 
        // verifica se almeno un campo indicizzato è valorizzato (test per validazioni complesse del modello) 
        bool found = false; 
        foreach (var idx in ListIndexes()) { 
            string fldLst = idx.Split("|")[2]; 
            foreach (var fld in fldLst.Split(",")) { 
                if (DogManager.getPropertyValue(this, "Sel" + UtilHelper.field2Property(fld.Trim())) != null) found = true; 
                if (DogManager.getPropertyValue(this, "Sel" + UtilHelper.field2Property(fld.Trim()) + "[0]") != null) found = true; 
                if (DogManager.getPropertyValue(this, "Sel" + UtilHelper.field2Property(fld.Trim()) + ".StartDate") != null) found = true; 
                if (DogManager.getPropertyValue(this, "Sel" + UtilHelper.field2Property(fld.Trim()) + ".EndDate") != null) found = true; 
            } 
        } 
        if (!found) { isValidate = false;  modelState.AddModelError(prefix ?? string.Empty, "Deve essere compilato almeno un campo indicizzato."); } 
        //-- 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioTe1Icode|K|TE__ICODE","sioTe1RecDate|N|TE__MDATE,TE__CDATE"
        ,"sioTeClassete1Versionte1Deleted|U|TE_CLASSE,TE__VERSION,TE__DELETED"
        ,"sioTeCodicete1Versionte1Deleted|U|TE_CODICE,TE__VERSION,TE__DELETED"
    };
}
}
}
