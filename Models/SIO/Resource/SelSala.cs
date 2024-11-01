using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Resource {
public class SelSala : ModelErp {
public const string Description = "Risorse: località";
public const string SqlTableName = "SALA";
public const string SqlTableNameExt = "SALA";
public const string SqlRowIdName = "SA__ID";
public const string SqlRowIdNameExt = "SA__ICODE";
public const string SqlPrefix = "SA_";
public const string SqlPrefixExt = "SA_";
public const string SqlXdataTableName = "SA_XDATA";
public const string SqlXdataTableNameExt = "SA_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "SEL"; //Data Model Name of the Class
public const int INTCODE = 94; //Internal Table Code
public const string TBAREA = "Risorse"; //Table Area
public const string PREFIX = "Sa"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
//127-124//REL_PRESTAZIONE_USA.PU_ID_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa> SelRelPrestazioneUsa4PuIdRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelPrestazioneUsa>();
//1182-1179//REL_ATTIVITA_USA.AU_ID_RISORSA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa> SelRelAttivitaUsa4AuIdRisorsa  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa>();

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("SA_CODICE", SqlFieldNameExt="SA_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(SALA.SA__ICODE[SA__ICODE] {SA_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? SelSaCodice  { get; set; }

[Display(Name = "Classe Risorsa", ShortName="", Description = "Classe di risorse: L[ocations] (Località)", Prompt="")]
[ErpDogField("SA_CLASSE_RISORSA", SqlFieldNameExt="SA_CLASSE_RISORSA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_RISORSA.TS_CLASSE_RISORSA[SALA.SA_ID_TIPO_RISORSA]) multbxref()")]
[DataType(DataType.Text)]
public string? SelSaClasseRisorsa  { get; set; }

[Display(Name = "Id Tipo Risorsa", ShortName="", Description = "Codice del tipo di località", Prompt="")]
[ErpDogField("SA_ID_TIPO_RISORSA", SqlFieldNameExt="SA_ID_TIPO_RISORSA", SqlFieldOptions="", Xref="Ts1Icode", SqlFieldProperties="prop() xref(TIPO_RISORSA.TS__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoRisorsa", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> SelSaIdTipoRisorsa  { get; set; } = new List<string>();

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("SA_DESCRIZIONE", SqlFieldNameExt="SA_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelSaDescrizione  { get; set; }

[Display(Name = "Costo Unitario Uso", ShortName="", Description = "Costo unitario per l'utilizzo", Prompt="")]
[ErpDogField("SA_COSTO_UNITARIO_USO", SqlFieldNameExt="SA_COSTO_UNITARIO_USO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? SelSaCostoUnitarioUso  { get; set; }

[Display(Name = "Misura Unitaria Uso", ShortName="", Description = "Unità di misura per l'utilizzo", Prompt="")]
[ErpDogField("SA_MISURA_UNITARIA_USO", SqlFieldNameExt="SA_MISURA_UNITARIA_USO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelSaMisuraUnitariaUso  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("SA_NOTE", SqlFieldNameExt="SA_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelSaNote  { get; set; }

[Display(Name = "Disponibilita", ShortName="", Description = "Descrizione testuale dello stato attuale di disponibilità", Prompt="")]
[ErpDogField("SA_DISPONIBILITA", SqlFieldNameExt="SA_DISPONIBILITA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelSaDisponibilita  { get; set; }

[Display(Name = "Telefono Fornitore", ShortName="", Description = "Numero di telefono", Prompt="")]
[ErpDogField("SA_TELEFONO_FORNITORE", SqlFieldNameExt="SA_TELEFONO_FORNITORE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? SelSaTelefonoFornitore  { get; set; }

[Display(Name = "Data Ultima Manutenzione", ShortName="", Description = "Data dell'ultima manutenzione", Prompt="")]
[ErpDogField("SA_DATA_ULTIMA_MANUTENZIONE", SqlFieldNameExt="SA_DATA_ULTIMA_MANUTENZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SelSaDataUltimaManutenzione  { get; set; } = new DateRange();

[Display(Name = "Frequenza Manutenzione", ShortName="", Description = "Frequenza della manutenzione periodica [numero di ore di funzionamento]", Prompt="")]
[ErpDogField("SA_FREQUENZA_MANUTENZIONE", SqlFieldNameExt="SA_FREQUENZA_MANUTENZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? SelSaFrequenzaManutenzione  { get; set; }

[Display(Name = "Uso Medio Giornaliero", ShortName="", Description = "Numero medio di ore effettive di lavoro al giorno", Prompt="")]
[ErpDogField("SA_USO_MEDIO_GIORNALIERO", SqlFieldNameExt="SA_USO_MEDIO_GIORNALIERO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? SelSaUsoMedioGiornaliero  { get; set; }

[Display(Name = "Data Prossima Manutenzione", ShortName="", Description = "Data della prossima manutenzione prevista", Prompt="")]
[ErpDogField("SA_DATA_PROSSIMA_MANUTENZIONE", SqlFieldNameExt="SA_DATA_PROSSIMA_MANUTENZIONE", SqlFieldOptions="[DATE]", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DateRange]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public DateRange SelSaDataProssimaManutenzione  { get; set; } = new DateRange();

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
    return new List<string>() { "sioSa1Icode|K|SA__ICODE","sioSa1RecDate|N|SA__MDATE,SA__CDATE"
        ,"sioSaIdTipoRisorsa|N|SA_ID_TIPO_RISORSA"
        ,"sioSa1Versionsa1Deleted|U|SA__VERSION,SA__DELETED"
        ,"sioSaCodicesa1Versionsa1Deleted|U|SA_CODICE,SA__VERSION,SA__DELETED"
    };
}
}
}
