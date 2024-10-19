using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class Attivita : ModelErp {
public const string Description = "Tipi di attività che possono essere richieste e/o eseguite";
public const string SqlTableName = "ATTIVITA";
public const string SqlTableNameExt = "ATTIVITA";
public const string SqlRowIdName = "AV__ID";
public const string SqlRowIdNameExt = "AV__ICODE";
public const string SqlPrefix = "AV_";
public const string SqlPrefixExt = "AV_";
public const string SqlXdataTableName = "AV_XDATA";
public const string SqlXdataTableNameExt = "AV_XDATA";
public const string MODEL = "SIO"; //Data Model Name of the Class
public const string CATEG = "TAB"; //Data Model Name of the Class
public const int INTCODE = 11; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Av"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No

public char? action = null; public IDictionary<string, string> options = new Dictionary<string, string>();  // proprietà necessarie per la mantain del record

//370-370//REL_ATTIVITA_TIPO_CAMPIONE.AC_ID_ATTIVITA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaTipoCampione> RelAttivitaTipoCampione4AcIdAttivita  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaTipoCampione>();
//1131-1131//REL_ATTIVITA_RICHIESTA_DA.AR_ID_ATTIVITA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaRichiestaDa> RelAttivitaRichiestaDa4ArIdAttivita  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaRichiestaDa>();
//1179-1179//REL_ATTIVITA_USA.AU_ID_ATTIVITA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa> RelAttivitaUsa4AuIdAttivita  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaUsa>();
//1992-1992//REL_ATTIVITA_EROGATA_DA.AE_ID_ATTIVITA
public List<ErpToolkit.Models.SIO.Act.RelAttivitaErogataDa> RelAttivitaErogataDa4AeIdAttivita  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaErogataDa>();
//2203-2203//REL_ATTIVITA_CONTIENE.AA_ID_ATTIVITA_PADRE
public List<ErpToolkit.Models.SIO.Act.RelAttivitaContiene> RelAttivitaContiene4AaIdAttivitaPadre  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaContiene>();
//2204-2203//REL_ATTIVITA_CONTIENE.AA_ID_ATTIVITA_FIGLIO
public List<ErpToolkit.Models.SIO.Act.RelAttivitaContiene> RelAttivitaContiene4AaIdAttivitaFiglio  { get; set; } = new List<ErpToolkit.Models.SIO.Act.RelAttivitaContiene>();
[Display(Name = "Av1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("AV__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Av1Ienv { get; set; }
[Key]
[Display(Name = "Av1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("AV__ICODE", SqlFieldNameExt="AV__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Av1Icode { get; set; }
[Display(Name = "Av1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("AV__DELETED", SqlFieldNameExt="AV__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Av1Deleted { get; set; }
[Display(Name = "Av1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("AV__TIMESTAMP", SqlFieldNameExt="AV__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Av1Timestamp { get; set; }
[Display(Name = "Av1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("AV__HOME", SqlFieldNameExt="AV__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Av1Home { get; set; }
[Display(Name = "Av1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("AV__VERSION", SqlFieldNameExt="AV__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Av1Version { get; set; }
[Display(Name = "Av1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("AV__INACTIVE", SqlFieldNameExt="AV__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Av1Inactive { get; set; }
[Display(Name = "Av1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("AV__EXTATT", SqlFieldNameExt="AV__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Av1Extatt { get; set; }


[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("AV_CODICE", SqlFieldNameExt="AV_CODICE", SqlFieldOptions="[UID]", Xref="", SqlFieldProperties="prop() xref() xdup(ATTIVITA.AV__ICODE[AV__ICODE] {AV_CODICE=' '}) multbxref()")]
[DefaultValue("")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? AvCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("AV_DESCRIZIONE", SqlFieldNameExt="AV_DESCRIZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(50, ErrorMessage = "Inserire massimo 50 caratteri")]
[DataType(DataType.Text)]
public string? AvDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("AV_NOTE", SqlFieldNameExt="AV_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? AvNote  { get; set; }

[Display(Name = "Filtro Regime Erogazione", ShortName="", Description = "Maschera con le classi di contatti per cui l'attività può essere eseguita", Prompt="")]
[ErpDogField("AV_FILTRO_REGIME_EROGAZIONE", SqlFieldNameExt="AV_FILTRO_REGIME_EROGAZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(4, ErrorMessage = "Inserire massimo 4 caratteri")]
[DataType(DataType.Text)]
public string? AvFiltroRegimeErogazione  { get; set; }

[Display(Name = "Costo Medio", ShortName="", Description = "Costo totale (medio) per l'esecuzione", Prompt="")]
[ErpDogField("AV_COSTO_MEDIO", SqlFieldNameExt="AV_COSTO_MEDIO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
public double? AvCostoMedio  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Codice dell'attività di cui questa è una sotto-attività", Prompt="")]
[ErpDogField("AV_ID_GRUPPO", SqlFieldNameExt="AV_ID_GRUPPO", SqlFieldOptions="", Xref="Av1Icode", SqlFieldProperties="prop() xref(ATTIVITA.AV__ICODE) xdup() multbxref()")]
[AutocompleteClient("Attivita", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? AvIdGruppo  { get; set; }
public ErpToolkit.Models.SIO.Act.Attivita? AvIdGruppoObj  { get; set; }

[Display(Name = "Attivita Preferenziale", ShortName="", Description = "Attività preferenziale eseguita quando il servizio viene richiesto Sì [Y] / No [N]", Prompt="")]
[ErpDogField("AV_ATTIVITA_PREFERENZIALE", SqlFieldNameExt="AV_ATTIVITA_PREFERENZIALE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("N")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "Y", "N" }, MaxSelections=1, LabelClassName="")]
public string? AvAttivitaPreferenziale  { get; set; }

[Display(Name = "Durata Validita", ShortName="", Description = "Livello clinico di validità (cioè il numero di ore durante le quali non ha senso clinico replicare l'attività)", Prompt="")]
[ErpDogField("AV_DURATA_VALIDITA", SqlFieldNameExt="AV_DURATA_VALIDITA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
public short? AvDurataValidita  { get; set; }

[Display(Name = "Durata Media", ShortName="", Description = "Tempo medio del ciclo completo dell'attività [ore]", Prompt="")]
[ErpDogField("AV_DURATA_MEDIA", SqlFieldNameExt="AV_DURATA_MEDIA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
public short? AvDurataMedia  { get; set; }

[Display(Name = "In Evidenza", ShortName="", Description = "Evidenziare gli atti effettivi per scopi di ricerca o speciali Sì [Y] - No [N]", Prompt="")]
[ErpDogField("AV_IN_EVIDENZA", SqlFieldNameExt="AV_IN_EVIDENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("N")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "Y", "N" }, MaxSelections=1, LabelClassName="")]
public string? AvInEvidenza  { get; set; }

[Display(Name = "Id Tipo Attivita", ShortName="", Description = "Codice della classe generale di attività predefinita", Prompt="")]
[ErpDogField("AV_ID_TIPO_ATTIVITA", SqlFieldNameExt="AV_ID_TIPO_ATTIVITA", SqlFieldOptions="", Xref="Ta1Icode", SqlFieldProperties="prop() xref(TIPO_ATTIVITA.TA__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[AutocompleteClient("TipoAttivita", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? AvIdTipoAttivita  { get; set; }
public ErpToolkit.Models.SIO.Act.TipoAttivita? AvIdTipoAttivitaObj  { get; set; }

[Display(Name = "Routine", ShortName="", Description = "Pianificazione routinaria (cioè automatica) Sì [Y] - No [N]", Prompt="")]
[ErpDogField("AV_ROUTINE", SqlFieldNameExt="AV_ROUTINE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("Y")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[MultipleChoices(new[] { "Y", "N" }, MaxSelections=1, LabelClassName="")]
public string? AvRoutine  { get; set; }

[Display(Name = "Note Estese", ShortName="", Description = "Nota estesa", Prompt="")]
[ErpDogField("AV_NOTE_ESTESE", SqlFieldNameExt="AV_NOTE_ESTESE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(240, ErrorMessage = "Inserire massimo 240 caratteri")]
[DataType(DataType.Text)]
public string? AvNoteEstese  { get; set; }

[Display(Name = "Attributi1", ShortName="", Description = "Flag per scopi operativi, gestiti autonomamente dalle applicazioni", Prompt="")]
[ErpDogField("AV_ATTRIBUTI1", SqlFieldNameExt="AV_ATTRIBUTI1", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(240, ErrorMessage = "Inserire massimo 240 caratteri")]
[DataType(DataType.Text)]
public string? AvAttributi1  { get; set; }

[Display(Name = "Attributi2", ShortName="", Description = "Ulteriore insieme di flag operativi, gestiti dalle applicazioni", Prompt="")]
[ErpDogField("AV_ATTRIBUTI2", SqlFieldNameExt="AV_ATTRIBUTI2", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(240, ErrorMessage = "Inserire massimo 240 caratteri")]
[DataType(DataType.Text)]
public string? AvAttributi2  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioAv1Icode|K|AV__ICODE","sioAv1RecDate|N|AV__MDATE,AV__CDATE"
        ,"sioAvIdGruppo|N|AV_ID_GRUPPO"
        ,"sioAvIdTipoAttivita|N|AV_ID_TIPO_ATTIVITA"
        ,"sioAv1Versionav1Deleted|U|AV__VERSION,AV__DELETED"
        ,"sioAvCodiceav1Versionav1Deleted|U|AV_CODICE,AV__VERSION,AV__DELETED"
    };
}
}
}
