using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Common {
public class Organizzazione : ModelErp {
public const string Description = "Struttura sanitaria: centro sanitario, unità organizzativa, individuo, componente software, ecc.";
public const string SqlTableName = "ORGANIZZAZIONE";
public const string SqlTableNameExt = "ORGANIZZAZIONE";
public const string SqlRowIdName = "OR__ID";
public const string SqlRowIdNameExt = "OR__ICODE";
public const string SqlPrefix = "OR_";
public const string SqlPrefixExt = "OR_";
public const string SqlXdataTableName = "OR_XDATA";
public const string SqlXdataTableNameExt = "OR_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
public const int INTCODE = 2; //Internal Table Code
public const string TBAREA = "Organizzazione ospedaliera"; //Table Area
public const string PREFIX = "Or"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

public char? action = null; public IDictionary<string, string> options = new Dictionary<string, string>();  // proprietà necessarie per la mantain del record

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
[Display(Name = "Or1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("OR__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Or1Ienv { get; set; }
[Key]
[Display(Name = "Or1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("OR__ICODE", SqlFieldNameExt="OR__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Or1Icode { get; set; }
[Display(Name = "Or1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("OR__DELETED", SqlFieldNameExt="OR__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Or1Deleted { get; set; }
[Display(Name = "Or1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("OR__TIMESTAMP", SqlFieldNameExt="OR__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Or1Timestamp { get; set; }
[Display(Name = "Or1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("OR__HOME", SqlFieldNameExt="OR__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Or1Home { get; set; }
[Display(Name = "Or1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("OR__VERSION", SqlFieldNameExt="OR__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Or1Version { get; set; }
[Display(Name = "Or1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("OR__INACTIVE", SqlFieldNameExt="OR__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Or1Inactive { get; set; }
[Display(Name = "Or1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("OR__EXTATT", SqlFieldNameExt="OR__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Or1Extatt { get; set; }


[Display(Name = "Classe Assistenza", ShortName="", Description = "Classe dell'agente: 0=Centro - 1=Unità - 2=Punto di Servizio (PS) - 3=Individuo 4=Agente SW (da A a Z, definito dall'utente)", Prompt="")]
[ErpDogField("OR_CLASSE_ASSISTENZA", SqlFieldNameExt="OR_CLASSE_ASSISTENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("3")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? OrClasseAssistenza  { get; set; }

[Display(Name = "Codice", ShortName="", Description = "Identificatore dell'agente", Prompt="")]
[ErpDogField("OR_CODICE", SqlFieldNameExt="OR_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(ORGANIZZAZIONE.OR__ICODE[OR__ICODE] {OR_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? OrCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione dell'agente", Prompt="")]
[ErpDogField("OR_DESCRIZIONE", SqlFieldNameExt="OR_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? OrDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note sull'agente", Prompt="")]
[ErpDogField("OR_NOTE", SqlFieldNameExt="OR_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? OrNote  { get; set; }

[Display(Name = "Email", ShortName="", Description = "Indirizzo e-mail dell'agente", Prompt="")]
[ErpDogField("OR_EMAIL", SqlFieldNameExt="OR_EMAIL", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
[StringLength(40, ErrorMessage = "Inserire massimo 40 caratteri")]
[DataType(DataType.Text)]
public string? OrEmail  { get; set; }

[Display(Name = "Tipo Assistenza", ShortName="", Description = "Tipo dell'agente nella classificazione generale", Prompt="")]
[ErpDogField("OR_TIPO_ASSISTENZA", SqlFieldNameExt="OR_TIPO_ASSISTENZA", SqlFieldOptions="", Xref="Tz1Icode", SqlFieldProperties="prop() xref(TIPO_ORGANIZZAZIONE.TZ__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteClient("TipoOrganizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? OrTipoAssistenza  { get; set; }
public ErpToolkit.Models.SIO.Act.TipoOrganizzazione? OrTipoAssistenzaObj  { get; set; }

[Display(Name = "Telefono", ShortName="", Description = "Numero di telefono dell'agente (quando applicabile)", Prompt="")]
[ErpDogField("OR_TELEFONO", SqlFieldNameExt="OR_TELEFONO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(15, ErrorMessage = "Inserire massimo 15 caratteri")]
[DataType(DataType.Text)]
public string? OrTelefono  { get; set; }

[Display(Name = "Id Personale", ShortName="", Description = "Codice del membro del personale interno corrispondente, se applicabile (solo per classe = 3)", Prompt="")]
[ErpDogField("OR_ID_PERSONALE", SqlFieldNameExt="OR_ID_PERSONALE", SqlFieldOptions="", Xref="Pe1Icode", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE) xdup() multbxref()")]
[AutocompleteClient("Personale", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? OrIdPersonale  { get; set; }
public ErpToolkit.Models.SIO.Resource.Personale? OrIdPersonaleObj  { get; set; }

[Display(Name = "Id Istituto", ShortName="", Description = "Codice del centro sanitario (classe = 0) a cui appartiene l'agente (se applicabile)", Prompt="")]
[ErpDogField("OR_ID_ISTITUTO", SqlFieldNameExt="OR_ID_ISTITUTO", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? OrIdIstituto  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? OrIdIstitutoObj  { get; set; }

[Display(Name = "Id Unita", ShortName="", Description = "Codice dell'unità (classe = 1) a cui appartiene l'agente (se applicabile)", Prompt="")]
[ErpDogField("OR_ID_UNITA", SqlFieldNameExt="OR_ID_UNITA", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? OrIdUnita  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? OrIdUnitaObj  { get; set; }

[Display(Name = "Id Postazione", ShortName="", Description = "Codice del punto di servizio interno (classe = 2) a cui appartiene l'agente (se applicabile)", Prompt="")]
[ErpDogField("OR_ID_POSTAZIONE", SqlFieldNameExt="OR_ID_POSTAZIONE", SqlFieldOptions="", Xref="Or1Icode", SqlFieldProperties="prop() xref(ORGANIZZAZIONE.OR__ICODE) xdup() multbxref()")]
[AutocompleteClient("Organizzazione", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? OrIdPostazione  { get; set; }
public ErpToolkit.Models.SIO.Common.Organizzazione? OrIdPostazioneObj  { get; set; }

[Display(Name = "Pwd Crypt", ShortName="", Description = "Password (criptata), priva di significato se è implementata l'autenticazione tramite certificati", Prompt="")]
[ErpDogField("OR_PWD_CRYPT", SqlFieldNameExt="OR_PWD_CRYPT", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(80, ErrorMessage = "Inserire massimo 80 caratteri")]
[DataType(DataType.Text)]
public string? OrPwdCrypt  { get; set; }

[Display(Name = "Attivo", ShortName="", Description = "Codice specificante se l'agente è logicamente attivo nell'organizzazione o è stato (temporaneamente) disabilitato (vuoto=attivo)", Prompt="")]
[ErpDogField("OR_ATTIVO", SqlFieldNameExt="OR_ATTIVO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? OrAttivo  { get; set; }

[Display(Name = "Identificativo", ShortName="", Description = "Riferimento di contatto, quando applicabile", Prompt="")]
[ErpDogField("OR_IDENTIFICATIVO", SqlFieldNameExt="OR_IDENTIFICATIVO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? OrIdentificativo  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioOr1Icode|K|OR__ICODE","sioOr1RecDate|N|OR__MDATE,OR__CDATE"
        ,"sioOrIdIstitutoorIdUnitaorIdPostazione|N|OR_ID_ISTITUTO,OR_ID_UNITA,OR_ID_POSTAZIONE"
        ,"sioOrIdPostazione|N|OR_ID_POSTAZIONE"
        ,"sioOrIdPersonale|N|OR_ID_PERSONALE"
        ,"sioOrTipoAssistenza|N|OR_TIPO_ASSISTENZA"
        ,"sioOrCodiceor1Versionor1Deleted|U|OR_CODICE,OR__VERSION,OR__DELETED"
        ,"sioOrIdUnita|N|OR_ID_UNITA"
        ,"sioOr1Version|U|OR__VERSION"
    };
}
}
}
