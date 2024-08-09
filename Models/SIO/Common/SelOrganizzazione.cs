using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Common {
public class SelOrganizzazione {
public const string Description = "Struttura sanitaria: centro sanitario, unità organizzativa, individuo, componente software, ecc.";
public const string SqlTableName = "ORGANIZZAZIONE";
public const string SqlTableNameExt = "ORGANIZZAZIONE";
public const string SqlRowIdName = "OR__ID";
public const string SqlRowIdNameExt = "OR__ICODE";
public const string SqlPrefix = "OR_";
public const string SqlPrefixExt = "OR_";
public const string SqlXdataTableName = "OR_XDATA";
public const string SqlXdataTableNameExt = "OR_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 2; //Internal Table Code
public const string TBAREA = "Organizzazione ospedaliera"; //Table Area
public const string PREFIX = "Or"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//327-327//REL_ORGANIZZAZIONE_CONTIENE.OO_ID_ORGANIZZAZIONE_PADRE
public List<ErpToolkit.Models.SIO.Act.RelOrganizzazioneContiene> RelOrganizzazioneContiene4OoIdOrganizzazionePadre  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelOrganizzazioneContiene>();
//328-327//REL_ORGANIZZAZIONE_CONTIENE.OO_ID_ORGANIZZAZIONE_FIGLIO
public List<ErpToolkit.Models.SIO.Act.RelOrganizzazioneContiene> RelOrganizzazioneContiene4OoIdOrganizzazioneFiglio  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelOrganizzazioneContiene>();
//1132-1131//REL_ATTIVITA_RICHIESTA_DA.AR_ID_ISTITUTO
public List<ErpToolkit.Models.SIO.Act.RelAttivitaRichiestaDa> RelAttivitaRichiestaDa4ArIdIstituto  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaRichiestaDa>();
//1133-1131//REL_ATTIVITA_RICHIESTA_DA.AR_ID_UNITA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaRichiestaDa> RelAttivitaRichiestaDa4ArIdUnita  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaRichiestaDa>();
//1134-1131//REL_ATTIVITA_RICHIESTA_DA.AR_ID_POSTAZIONE
public List<ErpToolkit.Models.SIO.Act.RelAttivitaRichiestaDa> RelAttivitaRichiestaDa4ArIdPostazione  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaRichiestaDa>();
//1135-1131//REL_ATTIVITA_RICHIESTA_DA.AR_ID_OPERATORE
public List<ErpToolkit.Models.SIO.Act.RelAttivitaRichiestaDa> RelAttivitaRichiestaDa4ArIdOperatore  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaRichiestaDa>();
//1993-1992//REL_ATTIVITA_EROGATA_DA.AE_ID_UNITA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaErogataDa> RelAttivitaErogataDa4AeIdUnita  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaErogataDa>();

[Display(Name = "Classe Assistenza", ShortName="", Description = "Classe dell'agente: 0=Centro - 1=Unità - 2=Punto di Servizio (PS) - 3=Individuo 4=Agente SW (da A a Z, definito dall'utente)", Prompt="")]
[ErpDogField("OR_CLASSE_ASSISTENZA", SqlFieldNameExt="OR_CLASSE_ASSISTENZA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? OrClasseAssistenza  { get; set; }

[Display(Name = "Codice", ShortName="", Description = "Identificatore dell'agente", Prompt="")]
[ErpDogField("OR_CODICE", SqlFieldNameExt="OR_CODICE", SqlFieldOptions="[UID]", SqlFieldProperties="prop() xref() xdup(ORGANIZZAZIONE.OR__ICODE[OR__ICODE] {OR_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? OrCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione dell'agente", Prompt="")]
[ErpDogField("OR_DESCRIZIONE", SqlFieldNameExt="OR_DESCRIZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? OrDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note sull'agente", Prompt="")]
[ErpDogField("OR_NOTE", SqlFieldNameExt="OR_NOTE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? OrNote  { get; set; }

[Display(Name = "Email", ShortName="", Description = "Indirizzo e-mail dell'agente", Prompt="")]
[ErpDogField("OR_EMAIL", SqlFieldNameExt="OR_EMAIL", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? OrEmail  { get; set; }

[Display(Name = "Tipo Assistenza", ShortName="", Description = "Tipo dell'agente nella classificazione generale", Prompt="")]
[ErpDogField("OR_TIPO_ASSISTENZA", SqlFieldNameExt="OR_TIPO_ASSISTENZA", SqlFieldOptions="", SqlFieldProperties="prop() xref(TIPO_ORGANIZZAZIONE.TZ__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoOrganizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> OrTipoAssistenza  { get; set; } = new List<string>();

[Display(Name = "Telefono", ShortName="", Description = "Numero di telefono dell'agente (quando applicabile)", Prompt="")]
[ErpDogField("OR_TELEFONO", SqlFieldNameExt="OR_TELEFONO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? OrTelefono  { get; set; }

[Display(Name = "Id Personale", ShortName="", Description = "Codice del membro del personale interno corrispondente, se applicabile (solo per classe = 3)", Prompt="")]
[ErpDogField("OR_ID_PERSONALE", SqlFieldNameExt="OR_ID_PERSONALE", SqlFieldOptions="", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Personale", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> OrIdPersonale  { get; set; } = new List<string>();

[Display(Name = "Id Istituto", ShortName="", Description = "Codice del centro sanitario (classe = 0) a cui appartiene l'agente (se applicabile)", Prompt="")]
[ErpDogField("OR_ID_ISTITUTO", SqlFieldNameExt="OR_ID_ISTITUTO", SqlFieldOptions="", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> OrIdIstituto  { get; set; } = new List<string>();

[Display(Name = "Id Unita", ShortName="", Description = "Codice dell'unità (classe = 1) a cui appartiene l'agente (se applicabile)", Prompt="")]
[ErpDogField("OR_ID_UNITA", SqlFieldNameExt="OR_ID_UNITA", SqlFieldOptions="", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> OrIdUnita  { get; set; } = new List<string>();

[Display(Name = "Id Postazione", ShortName="", Description = "Codice del punto di servizio interno (classe = 2) a cui appartiene l'agente (se applicabile)", Prompt="")]
[ErpDogField("OR_ID_POSTAZIONE", SqlFieldNameExt="OR_ID_POSTAZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> OrIdPostazione  { get; set; } = new List<string>();

[Display(Name = "Pwd Crypt", ShortName="", Description = "Password (criptata), priva di significato se è implementata l'autenticazione tramite certificati", Prompt="")]
[ErpDogField("OR_PWD_CRYPT", SqlFieldNameExt="OR_PWD_CRYPT", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? OrPwdCrypt  { get; set; }

[Display(Name = "Attivo", ShortName="", Description = "Codice specificante se l'agente è logicamente attivo nell'organizzazione o è stato (temporaneamente) disabilitato (vuoto=attivo)", Prompt="")]
[ErpDogField("OR_ATTIVO", SqlFieldNameExt="OR_ATTIVO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? OrAttivo  { get; set; }

[Display(Name = "Identificativo", ShortName="", Description = "Riferimento di contatto, quando applicabile", Prompt="")]
[ErpDogField("OR_IDENTIFICATIVO", SqlFieldNameExt="OR_IDENTIFICATIVO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? OrIdentificativo  { get; set; }

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
    return new List<string>() { "sioOr1Icode|K|Or1Icode","sioOr1RecDate|N|Or1Mdate,Or1Cdate"
        ,"sioOrIdIstitutoOrIdUnitaOrIdPostazione|N|OrIdIstituto,OrIdUnita,OrIdPostazione"
        ,"sioOrIdPostazione|N|OrIdPostazione"
        ,"sioOrIdPersonale|N|OrIdPersonale"
        ,"sioOrTipoAssistenza|N|OrTipoAssistenza"
        ,"sioOrCodiceOr1VersionOr1Deleted|U|OrCodice,Or1Version,Or1Deleted"
        ,"sioOrIdUnita|N|OrIdUnita"
        ,"sioOr1Version|U|Or1Version"
    };
}
}
}
