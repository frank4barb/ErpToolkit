using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Common {
public class SelTipoRichiesta : ModelErp {
public const string Description = "Tipo di richieste";
public const string SqlTableName = "TIPO_RICHIESTA";
public const string SqlTableNameExt = "TIPO_RICHIESTA";
public const string SqlRowIdName = "TI__ID";
public const string SqlRowIdNameExt = "TI__ICODE";
public const string SqlPrefix = "TI_";
public const string SqlPrefixExt = "TI_";
public const string SqlXdataTableName = "TI_XDATA";
public const string SqlXdataTableNameExt = "TI_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "SEL"; //Data Model Name of the Class
public const int INTCODE = 48; //Internal Table Code
public const string TBAREA = "Organizzazione ospedaliera"; //Table Area
public const string PREFIX = "Ti"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("TI_CODICE", SqlFieldNameExt="TI_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_RICHIESTA.TI__ICODE[TI__ICODE] {TI_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? SelTiCodice  { get; set; }

[Display(Name = "Gruppo", ShortName="", Description = "Classe di comunicazione: 0 = Comunicazioni di sistema 1 = Messaggi utente - 2 = Relativi agli atti - Z = Utente-d", Prompt="")]
[ErpDogField("TI_GRUPPO", SqlFieldNameExt="TI_GRUPPO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[MultipleChoices(new[] { "0", "1", "2", "Z" }, MaxSelections=-1, LabelClassName="")]
[DataType(DataType.Text)]
public List<string> SelTiGruppo  { get; set; } = new List<string>();

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione", Prompt="")]
[ErpDogField("TI_DESCRIZIONE", SqlFieldNameExt="TI_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelTiDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("TI_NOTE", SqlFieldNameExt="TI_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelTiNote  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
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
        if (!found) { isValidate = false;  modelState.AddModelError(string.Empty, "Deve essere compilato almeno un campo indicizzato."); } 
        //-- 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioTi1Icode|K|TI__ICODE","sioTi1RecDate|N|TI__MDATE,TI__CDATE"
        ,"sioTiGruppoti1Versionti1Deleted|U|TI_GRUPPO,TI__VERSION,TI__DELETED"
        ,"sioTiCodiceti1Versionti1Deleted|U|TI_CODICE,TI__VERSION,TI__DELETED"
    };
}
}
}
