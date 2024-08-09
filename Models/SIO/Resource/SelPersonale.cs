using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Resource {
public class SelPersonale {
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

[Display(Name = "Codice", ShortName="", Description = "ID del membro dello staff nell'organizzazione", Prompt="")]
[ErpDogField("PE_CODICE", SqlFieldNameExt="PE_CODICE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PeCodice  { get; set; }

[Display(Name = "Classe Risorsa", ShortName="", Description = "Classe di risorse S[tato]", Prompt="")]
[ErpDogField("PE_CLASSE_RISORSA", SqlFieldNameExt="PE_CLASSE_RISORSA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup(TIPO_RISORSA.TS_CLASSE_RISORSA[PERSONALE.PE_ID_TIPO_RISORSA]) multbxref()")]
[DataType(DataType.Text)]
public string? PeClasseRisorsa  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("PE_DESCRIZIONE", SqlFieldNameExt="PE_DESCRIZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PeDescrizione  { get; set; }

[Display(Name = "Id Tipo Risorsa", ShortName="", Description = "Codice del tipo di membro dello staff (classificazione operativa)", Prompt="")]
[ErpDogField("PE_ID_TIPO_RISORSA", SqlFieldNameExt="PE_ID_TIPO_RISORSA", SqlFieldOptions="", SqlFieldProperties="prop() xref(TIPO_RISORSA.TS__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoRisorsa", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> PeIdTipoRisorsa  { get; set; } = new List<string>();

[Display(Name = "Costo Unitario Uso", ShortName="", Description = "Costo unitario per l'utilizzo", Prompt="")]
[ErpDogField("PE_COSTO_UNITARIO_USO", SqlFieldNameExt="PE_COSTO_UNITARIO_USO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? PeCostoUnitarioUso  { get; set; }

[Display(Name = "Misura Unitaria Uso", ShortName="", Description = "Unità di misura per l'utilizzo", Prompt="")]
[ErpDogField("PE_MISURA_UNITARIA_USO", SqlFieldNameExt="PE_MISURA_UNITARIA_USO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PeMisuraUnitariaUso  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("PE_NOTE", SqlFieldNameExt="PE_NOTE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PeNote  { get; set; }

[Display(Name = "Disponibilita", ShortName="", Description = "Descrizione testuale dello stato attuale di disponibilità", Prompt="")]
[ErpDogField("PE_DISPONIBILITA", SqlFieldNameExt="PE_DISPONIBILITA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PeDisponibilita  { get; set; }

[Display(Name = "Telefono", ShortName="", Description = "Numero di telefono dell'ufficio del membro dello staff", Prompt="")]
[ErpDogField("PE_TELEFONO", SqlFieldNameExt="PE_TELEFONO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PeTelefono  { get; set; }

[Display(Name = "Cellulare", ShortName="", Description = "Numero di telefono privato", Prompt="")]
[ErpDogField("PE_CELLULARE", SqlFieldNameExt="PE_CELLULARE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PeCellulare  { get; set; }

[Display(Name = "Nome", ShortName="", Description = "Nome del membro dello staff", Prompt="")]
[ErpDogField("PE_NOME", SqlFieldNameExt="PE_NOME", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PeNome  { get; set; }

[Display(Name = "Cognome", ShortName="", Description = "Cognome del membro dello staff", Prompt="")]
[ErpDogField("PE_COGNOME", SqlFieldNameExt="PE_COGNOME", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PeCognome  { get; set; }

[Display(Name = "Codice Fiscale", ShortName="", Description = "Identificatore nazionale (ad esempio, codice fiscale) del membro dello staff", Prompt="")]
[ErpDogField("PE_CODICE_FISCALE", SqlFieldNameExt="PE_CODICE_FISCALE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PeCodiceFiscale  { get; set; }

[Display(Name = "Email", ShortName="", Description = "Indirizzo email del membro dello staff", Prompt="")]
[ErpDogField("PE_EMAIL", SqlFieldNameExt="PE_EMAIL", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? PeEmail  { get; set; }

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
    return new List<string>() { "sioPe1Icode|K|Pe1Icode","sioPe1RecDate|N|Pe1Mdate,Pe1Cdate"
        ,"sioPeCodiceFiscale|N|PeCodiceFiscale"
        ,"sioPeCodicePe1Deleted|U|PeCodice,Pe1Deleted"
        ,"sioPeIdTipoRisorsa|N|PeIdTipoRisorsa"
        ,"sioPe1VersionPe1Deleted|U|Pe1Version,Pe1Deleted"
    };
}
}
}
