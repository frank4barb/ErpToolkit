using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Common {
public class SelRichiesta : ModelErp {
public const string Description = "Comunicazione e/o richiesta di prestazioni";
public const string SqlTableName = "RICHIESTA";
public const string SqlTableNameExt = "RICHIESTA";
public const string SqlRowIdName = "RI__ID";
public const string SqlRowIdNameExt = "RI__ICODE";
public const string SqlPrefix = "RI_";
public const string SqlPrefixExt = "RI_";
public const string SqlXdataTableName = "RI_XDATA";
public const string SqlXdataTableNameExt = "RI_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 46; //Internal Table Code
public const string TBAREA = "Organizzazione ospedaliera"; //Table Area
public const string PREFIX = "Ri"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

[Display(Name = "Id Unita Richiedente", ShortName="", Description = "Codice dell'unità che ha originato la comunicazione", Prompt="")]
[ErpDogField("RI_ID_UNITA_RICHIEDENTE", SqlFieldNameExt="RI_ID_UNITA_RICHIEDENTE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> RiIdUnitaRichiedente  { get; set; } = new List<string>();

[Display(Name = "Id Postazione Richiedente", ShortName="", Description = "Codice del punto di servizio che ha originato la comunicazione", Prompt="")]
[ErpDogField("RI_ID_POSTAZIONE_RICHIEDENTE", SqlFieldNameExt="RI_ID_POSTAZIONE_RICHIEDENTE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> RiIdPostazioneRichiedente  { get; set; } = new List<string>();

[Display(Name = "Id Istituto Richiedente", ShortName="", Description = "Codice dell'organizzazione che ha originato la comunicazione", Prompt="")]
[ErpDogField("RI_ID_ISTITUTO_RICHIEDENTE", SqlFieldNameExt="RI_ID_ISTITUTO_RICHIEDENTE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> RiIdIstitutoRichiedente  { get; set; } = new List<string>();

[Display(Name = "Id Operatore Richiedente", ShortName="", Description = "Codice (se disponibile) dell'agente che ha effettivamente inserito la comunicazione", Prompt="")]
[ErpDogField("RI_ID_OPERATORE_RICHIEDENTE", SqlFieldNameExt="RI_ID_OPERATORE_RICHIEDENTE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> RiIdOperatoreRichiedente  { get; set; } = new List<string>();

[Display(Name = "Data Richiesta", ShortName="", Description = "Data non prima della quale la comunicazione deve essere trasmessa / Data di completamento quando eseguita", Prompt="")]
[ErpDogField("RI_DATA_RICHIESTA", SqlFieldNameExt="RI_DATA_RICHIESTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange RiDataRichiesta  { get; set; } = new DateRange();

[Display(Name = "Ora Richiesta", ShortName="", Description = "Ora non prima della quale la comunicazione deve essere trasmessa / Ora di completamento quando eseguita", Prompt="")]
[ErpDogField("RI_ORA_RICHIESTA", SqlFieldNameExt="RI_ORA_RICHIESTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? RiOraRichiesta  { get; set; }

[Display(Name = "Urgenza", ShortName="", Description = "Livello di urgenza da 1 a 5 [1: il più alto]", Prompt="")]
[ErpDogField("RI_URGENZA", SqlFieldNameExt="RI_URGENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? RiUrgenza  { get; set; }

[Display(Name = "Oggetto", ShortName="", Description = "Oggetto della comunicazione", Prompt="")]
[ErpDogField("RI_OGGETTO", SqlFieldNameExt="RI_OGGETTO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? RiOggetto  { get; set; }

[Display(Name = "Stato Richiesta", ShortName="", Description = "Stato della comunicazione: In attesa / Sospesa / Completata (o annullata) / X: trasmessa solo a alcuni indirizzi", Prompt="")]
[ErpDogField("RI_STATO_RICHIESTA", SqlFieldNameExt="RI_STATO_RICHIESTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("P")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("P|C|X|H|A", ErrorMessage = "Inserisci una delle seguenti opzioni: P|C|X|H|A")]
public string? RiStatoRichiesta  { get; set; }

[Display(Name = "Classe Richiesta", ShortName="", Description = "Classe della comunicazione: Da 0 a 9 riservata al sistema A a Z riservata agli utenti", Prompt="")]
[ErpDogField("RI_CLASSE_RICHIESTA", SqlFieldNameExt="RI_CLASSE_RICHIESTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_RICHIESTA.TI_GRUPPO[RICHIESTA.RI_ID_TIPO_RICHIESTA]) multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("0|1|2|3|4|5|6|7|8|9|Z", ErrorMessage = "Inserisci una delle seguenti opzioni: 0|1|2|3|4|5|6|7|8|9|Z")]
public string? RiClasseRichiesta  { get; set; }

[Display(Name = "Id Tipo Richiesta", ShortName="", Description = "Codice del tipo specifico di comunicazione", Prompt="")]
[ErpDogField("RI_ID_TIPO_RICHIESTA", SqlFieldNameExt="RI_ID_TIPO_RICHIESTA", SqlFieldOptions="", Xref="Ti1Icode", SqlFieldProperties="prop() xref(TIPO_RICHIESTA.TI__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoRichiesta", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> RiIdTipoRichiesta  { get; set; } = new List<string>();

[Display(Name = "Id Paziente", ShortName="", Description = "Codice del paziente principale a cui si riferisce la comunicazione (se presente)", Prompt="")]
[ErpDogField("RI_ID_PAZIENTE", SqlFieldNameExt="RI_ID_PAZIENTE", SqlFieldOptions="", Xref="Pa1Icode", SqlFieldProperties="prop() xref(PAZIENTE.PA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Paziente", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> RiIdPaziente  { get; set; } = new List<string>();

[Display(Name = "Id Episodio", ShortName="", Description = "Codice del contatto del paziente principale a cui si riferisce la comunicazione (se presente)", Prompt="")]
[ErpDogField("RI_ID_EPISODIO", SqlFieldNameExt="RI_ID_EPISODIO", SqlFieldOptions="", Xref="Ep1Icode", SqlFieldProperties="prop() xref(EPISODIO.EP__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Episodio", "AutocompleteGetSelect", "AutocompletePreLoad", 10)]
[DataType(DataType.Text)]
public List<string> RiIdEpisodio  { get; set; } = new List<string>();

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
    return new List<string>() { "sioRi1Icode|K|Ri1Icode","sioRi1RecDate|N|Ri1Mdate,Ri1Cdate"
        ,"sioRiDataRichiesta|N|RiDataRichiesta"
        ,"sioRiIdOperatoreRichiedente|N|RiIdOperatoreRichiedente"
        ,"sioRiIdTipoRichiestaRiStatoRichiesta|N|RiIdTipoRichiesta,RiStatoRichiesta"
        ,"sioRiIdEpisodio|N|RiIdEpisodio"
        ,"sioRiIdPaziente|N|RiIdPaziente"
        ,"sioRiIdIstitutoRichiedente|N|RiIdIstitutoRichiedente"
        ,"sioRiIdPostazioneRichiedente|N|RiIdPostazioneRichiedente"
        ,"sioRiIdUnitaRichiedente|N|RiIdUnitaRichiedente"
    };
}
}
}
