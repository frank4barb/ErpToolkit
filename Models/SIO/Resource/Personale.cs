using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Resource {
public class Personale : ModelErp {
public const string Description = "Risorse: personale";
public const string SqlTableName = "PERSONALE";
public const string SqlTableNameExt = "PERSONALE";
public const string SqlRowIdName = "PE__ID";
public const string SqlRowIdNameExt = "PE__ICODE";
public const string SqlPrefix = "PE_";
public const string SqlPrefixExt = "PE_";
public const string SqlXdataTableName = "PE_XDATA";
public const string SqlXdataTableNameExt = "PE_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 95; //Internal Table Code
public const string TBAREA = "Risorse"; //Table Area
public const string PREFIX = "Pe"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//127-124//REL_PRESTAZIONE_USA.PU_ID_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa> RelPrestazioneUsa4PuIdRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa>();
//1182-1179//REL_ATTIVITA_USA.AU_ID_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa> RelAttivitaUsa4AuIdRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa>();
[Display(Name = "Pe1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("PE__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Pe1Ienv { get; set; }
[Key]
[Display(Name = "Pe1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("PE__ICODE", SqlFieldNameExt="PE__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pe1Icode { get; set; }
[Display(Name = "Pe1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("PE__DELETED", SqlFieldNameExt="PE__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Pe1Deleted { get; set; }
[Display(Name = "Pe1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("PE__TIMESTAMP", SqlFieldNameExt="PE__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Pe1Timestamp { get; set; }
[Display(Name = "Pe1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("PE__HOME", SqlFieldNameExt="PE__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pe1Home { get; set; }
[Display(Name = "Pe1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("PE__VERSION", SqlFieldNameExt="PE__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pe1Version { get; set; }
[Display(Name = "Pe1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("PE__INACTIVE", SqlFieldNameExt="PE__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Pe1Inactive { get; set; }
[Display(Name = "Pe1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("PE__EXTATT", SqlFieldNameExt="PE__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Pe1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "ID del membro dello staff nell'organizzazione", Prompt="")]
[ErpDogField("PE_CODICE", SqlFieldNameExt="PE_CODICE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? PeCodice  { get; set; }

[Display(Name = "Classe Risorsa", ShortName="", Description = "Classe di risorse S[tato]", Prompt="")]
[ErpDogField("PE_CLASSE_RISORSA", SqlFieldNameExt="PE_CLASSE_RISORSA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_RISORSA.TS_CLASSE_RISORSA[PERSONALE.PE_ID_TIPO_RISORSA]) multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("S")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[DataType(DataType.Text)]
public string? PeClasseRisorsa  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("PE_DESCRIZIONE", SqlFieldNameExt="PE_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? PeDescrizione  { get; set; }

[Display(Name = "Id Tipo Risorsa", ShortName="", Description = "Codice del tipo di membro dello staff (classificazione operativa)", Prompt="")]
[ErpDogField("PE_ID_TIPO_RISORSA", SqlFieldNameExt="PE_ID_TIPO_RISORSA", SqlFieldOptions="", Xref="Ts1Icode", SqlFieldProperties="prop() xref(TIPO_RISORSA.TS__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[AutocompleteClient("TipoRisorsa", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PeIdTipoRisorsa  { get; set; }
public ErpToolkit.Models.SIO.Resource.TipoRisorsa? PeIdTipoRisorsaObj  { get; set; }

[Display(Name = "Costo Unitario Uso", ShortName="", Description = "Costo unitario per l'utilizzo", Prompt="")]
[ErpDogField("PE_COSTO_UNITARIO_USO", SqlFieldNameExt="PE_COSTO_UNITARIO_USO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? PeCostoUnitarioUso  { get; set; }

[Display(Name = "Misura Unitaria Uso", ShortName="", Description = "Unità di misura per l'utilizzo", Prompt="")]
[ErpDogField("PE_MISURA_UNITARIA_USO", SqlFieldNameExt="PE_MISURA_UNITARIA_USO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? PeMisuraUnitariaUso  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("PE_NOTE", SqlFieldNameExt="PE_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? PeNote  { get; set; }

[Display(Name = "Disponibilita", ShortName="", Description = "Descrizione testuale dello stato attuale di disponibilità", Prompt="")]
[ErpDogField("PE_DISPONIBILITA", SqlFieldNameExt="PE_DISPONIBILITA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(20, ErrorMessage = "Inserire massimo 20 caratteri")]
[DataType(DataType.Text)]
public string? PeDisponibilita  { get; set; }

[Display(Name = "Telefono", ShortName="", Description = "Numero di telefono dell'ufficio del membro dello staff", Prompt="")]
[ErpDogField("PE_TELEFONO", SqlFieldNameExt="PE_TELEFONO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(15, ErrorMessage = "Inserire massimo 15 caratteri")]
[DataType(DataType.Text)]
public string? PeTelefono  { get; set; }

[Display(Name = "Cellulare", ShortName="", Description = "Numero di telefono privato", Prompt="")]
[ErpDogField("PE_CELLULARE", SqlFieldNameExt="PE_CELLULARE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(15, ErrorMessage = "Inserire massimo 15 caratteri")]
[DataType(DataType.Text)]
public string? PeCellulare  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Nome del membro dello staff", Prompt="")]
[ErpDogField("PE_NOME", SqlFieldNameExt="PE_NOME", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(30, ErrorMessage = "Inserire massimo 30 caratteri")]
[DataType(DataType.Text)]
public string? PeNome  { get; set; }

[Display(Name = "Cognome", ShortName="", Description = "Cognome del membro dello staff", Prompt="")]
[ErpDogField("PE_COGNOME", SqlFieldNameExt="PE_COGNOME", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(30, ErrorMessage = "Inserire massimo 30 caratteri")]
[DataType(DataType.Text)]
public string? PeCognome  { get; set; }

[Display(Name = "Codice Fiscale", ShortName="", Description = "Identificatore nazionale (ad esempio, codice fiscale) del membro dello staff", Prompt="")]
[ErpDogField("PE_CODICE_FISCALE", SqlFieldNameExt="PE_CODICE_FISCALE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
[StringLength(36, ErrorMessage = "Inserire massimo 36 caratteri")]
[DataType(DataType.Text)]
public string? PeCodiceFiscale  { get; set; }

[Display(Name = "Email", ShortName="", Description = "Indirizzo email del membro dello staff", Prompt="")]
[ErpDogField("PE_EMAIL", SqlFieldNameExt="PE_EMAIL", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(40, ErrorMessage = "Inserire massimo 40 caratteri")]
[DataType(DataType.Text)]
public string? PeEmail  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioPe1Icode|K|Pe1Icode","sioPe1RecDate|N|Pe1Mdate,Pe1Cdate"
        ,"sioPeCodiceFiscale|N|PeCodiceFiscale"
        ,"sioPeCodicePe1Deleted|U|PeCodice,Pe1Deleted"
        ,"sioPeIdTipoRisorsa|N|PeIdTipoRisorsa"
        ,"sioPe1VersionPe1Deleted|U|Pe1Version,Pe1Deleted"
    };
}
}
}
