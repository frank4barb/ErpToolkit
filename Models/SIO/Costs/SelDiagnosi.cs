using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Costs {
public class SelDiagnosi {
public const string Description = "Classificazioni diagnostiche adottate nelle organizzazioni sanitarie (ad esempio, DRG, AVG, ICD9, ecc.)";
public const string SqlTableName = "DIAGNOSI";
public const string SqlTableNameExt = "DIAGNOSI";
public const string SqlRowIdName = "DG__ID";
public const string SqlRowIdNameExt = "DG__ICODE";
public const string SqlPrefix = "DG_";
public const string SqlPrefixExt = "DG_";
public const string SqlXdataTableName = "DG_XDATA";
public const string SqlXdataTableNameExt = "DG_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 63; //Internal Table Code
public const string TBAREA = "Controllo di gestione"; //Table Area
public const string PREFIX = "Dg"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Tipo Diagnosi", ShortName="", Description = "Codice del tipo di classificazione a cui l'istanza appartiene", Prompt="")]
[ErpDogField("DG_TIPO_DIAGNOSI", SqlFieldNameExt="DG_TIPO_DIAGNOSI", SqlFieldOptions="", Xref="Td1Icode", SqlFieldProperties="prop() xref(TIPO_DIAGNOSI.TD__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoDiagnosi", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> DgTipoDiagnosi  { get; set; } = new List<string>();

[Display(Name = "Classe", ShortName="", Description = "Classificazione di aggregazione diagnostica definita dall'utente: 1: DRG 2: ICD9 3: ICD9-CM 4: APG, 5: AFO; 6: Specialità HC, ecc.", Prompt="")]
[ErpDogField("DG_CLASSE", SqlFieldNameExt="DG_CLASSE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? DgClasse  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione", Prompt="")]
[ErpDogField("DG_DESCRIZIONE", SqlFieldNameExt="DG_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? DgDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("DG_NOTE", SqlFieldNameExt="DG_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? DgNote  { get; set; }

[Display(Name = "Codice", ShortName="", Description = "Codice definito dall'utente per la classificazione", Prompt="")]
[ErpDogField("DG_CODICE", SqlFieldNameExt="DG_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(DIAGNOSI.DG__ICODE[DG__ICODE] {DG_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? DgCodice  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Identificatore del codice di aggregazione nella gerarchia (se presente)", Prompt="")]
[ErpDogField("DG_ID_GRUPPO", SqlFieldNameExt="DG_ID_GRUPPO", SqlFieldOptions="", Xref="Dg1Icode", SqlFieldProperties="prop() xref(DIAGNOSI.DG__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Diagnosi", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> DgIdGruppo  { get; set; } = new List<string>();

[Display(Name = "Tipo Drg", ShortName="", Description = "Tipo di DRG [M]edico - [C]hirurgico", Prompt="")]
[ErpDogField("DG_TIPO_DRG", SqlFieldNameExt="DG_TIPO_DRG", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("M")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("M|S", ErrorMessage = "Inserisci una delle seguenti opzioni: M|S")]
public string? DgTipoDrg  { get; set; }

[Display(Name = "Tipo Icd9", ShortName="", Description = "Tipo di ICD9-CM [D]iagnostico - [O]perativo (se applicabile)", Prompt="")]
[ErpDogField("DG_TIPO_ICD9", SqlFieldNameExt="DG_TIPO_ICD9", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("D|O| ", ErrorMessage = "Inserisci una delle seguenti opzioni: D|O| ")]
public string? DgTipoIcd9  { get; set; }

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
    return new List<string>() { "sioDg1Icode|K|Dg1Icode","sioDg1RecDate|N|Dg1Mdate,Dg1Cdate"
        ,"sioDgTipoDiagnosiDg1VersionDg1Deleted|U|DgTipoDiagnosi,Dg1Version,Dg1Deleted"
        ,"sioDgIdGruppo|N|DgIdGruppo"
        ,"sioDgCodiceDg1VersionDg1Deleted|U|DgCodice,Dg1Version,Dg1Deleted"
    };
}
}
}
