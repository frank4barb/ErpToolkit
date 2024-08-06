using ErpToolkit.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class SelAttivita {
public const string Description = "Tipi di attività che possono essere richieste e/o eseguite";
public const string SqlTableName = "ATTIVITA";
public const string SqlTableNameExt = "ATTIVITA";
public const string SqlRowIdName = "AV__ID";
public const string SqlRowIdNameExt = "AV__ICODE";
public const string SqlPrefix = "AV_";
public const string SqlPrefixExt = "AV_";
public const string SqlXdataTableName = "AV_XDATA";
public const string SqlXdataTableNameExt = "AV_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 11; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Av"; //Table Prefix
public const string LIVEDESC = "D"; //Table type: Live or Description
public const string IS_RELTABLE = "N"; //Is Relation Table: Yes or No
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

[Display(Name = "Codice", ShortName="", Description = "Codice assegnato dall'utente", Prompt="")]
[ErpDogField("AV_CODICE", SqlFieldNameExt="AV_CODICE", SqlFieldOptions="[UID]", SqlFieldProperties="prop() xref() xdup(ATTIVITA.AV__ICODE[AV__ICODE] {AV_CODICE=' '}) multbxref()")]
[DataType(DataType.Text)]
public string? AvCodice  { get; set; }

[Display(Name = "Descrizione", ShortName="", Description = "Descrizione estesa", Prompt="")]
[ErpDogField("AV_DESCRIZIONE", SqlFieldNameExt="AV_DESCRIZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? AvDescrizione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note", Prompt="")]
[ErpDogField("AV_NOTE", SqlFieldNameExt="AV_NOTE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? AvNote  { get; set; }

[Display(Name = "Filtro Regime Erogazione", ShortName="", Description = "Maschera con le classi di contatti per cui l'attività può essere eseguita", Prompt="")]
[ErpDogField("AV_FILTRO_REGIME_EROGAZIONE", SqlFieldNameExt="AV_FILTRO_REGIME_EROGAZIONE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? AvFiltroRegimeErogazione  { get; set; }

[Display(Name = "Costo Medio", ShortName="", Description = "Costo totale (medio) per l'esecuzione", Prompt="")]
[ErpDogField("AV_COSTO_MEDIO", SqlFieldNameExt="AV_COSTO_MEDIO", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? AvCostoMedio  { get; set; }

[Display(Name = "Id Gruppo", ShortName="", Description = "Codice dell'attività di cui questa è una sotto-attività", Prompt="")]
[ErpDogField("AV_ID_GRUPPO", SqlFieldNameExt="AV_ID_GRUPPO", SqlFieldOptions="", SqlFieldProperties="prop() xref(ATTIVITA.AV__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("Attivita", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> AvIdGruppo  { get; set; } = new List<string>();

[Display(Name = "Attivita Preferenziale", ShortName="", Description = "Attività preferenziale eseguita quando il servizio viene richiesto Sì [Y] / No [N]", Prompt="")]
[ErpDogField("AV_ATTIVITA_PREFERENZIALE", SqlFieldNameExt="AV_ATTIVITA_PREFERENZIALE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("N")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("Y|N", ErrorMessage = "Inserisci una delle seguenti opzioni: Y|N")]
public string? AvAttivitaPreferenziale  { get; set; }

[Display(Name = "Durata Validita", ShortName="", Description = "Livello clinico di validità (cioè il numero di ore durante le quali non ha senso clinico replicare l'attività)", Prompt="")]
[ErpDogField("AV_DURATA_VALIDITA", SqlFieldNameExt="AV_DURATA_VALIDITA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? AvDurataValidita  { get; set; }

[Display(Name = "Durata Media", ShortName="", Description = "Tempo medio del ciclo completo dell'attività [ore]", Prompt="")]
[ErpDogField("AV_DURATA_MEDIA", SqlFieldNameExt="AV_DURATA_MEDIA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? AvDurataMedia  { get; set; }

[Display(Name = "In Evidenza", ShortName="", Description = "Evidenziare gli atti effettivi per scopi di ricerca o speciali Sì [Y] - No [N]", Prompt="")]
[ErpDogField("AV_IN_EVIDENZA", SqlFieldNameExt="AV_IN_EVIDENZA", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("N")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("Y|N", ErrorMessage = "Inserisci una delle seguenti opzioni: Y|N")]
public string? AvInEvidenza  { get; set; }

[Display(Name = "Id Tipo Attivita", ShortName="", Description = "Codice della classe generale di attività predefinita", Prompt="")]
[ErpDogField("AV_ID_TIPO_ATTIVITA", SqlFieldNameExt="AV_ID_TIPO_ATTIVITA", SqlFieldOptions="", SqlFieldProperties="prop() xref(TIPO_ATTIVITA.TA__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteClient("TipoAttivita", "AutocompleteGetAll", 10)]
[DataType(DataType.Text)]
public List<string> AvIdTipoAttivita  { get; set; } = new List<string>();

[Display(Name = "Routine", ShortName="", Description = "Pianificazione routinaria (cioè automatica) Sì [Y] - No [N]", Prompt="")]
[ErpDogField("AV_ROUTINE", SqlFieldNameExt="AV_ROUTINE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("Y")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("Y|N", ErrorMessage = "Inserisci una delle seguenti opzioni: Y|N")]
public string? AvRoutine  { get; set; }

[Display(Name = "Note Estese", ShortName="", Description = "Nota estesa", Prompt="")]
[ErpDogField("AV_NOTE_ESTESE", SqlFieldNameExt="AV_NOTE_ESTESE", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? AvNoteEstese  { get; set; }

[Display(Name = "Attributi1", ShortName="", Description = "Flag per scopi operativi, gestiti autonomamente dalle applicazioni", Prompt="")]
[ErpDogField("AV_ATTRIBUTI1", SqlFieldNameExt="AV_ATTRIBUTI1", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? AvAttributi1  { get; set; }

[Display(Name = "Attributi2", ShortName="", Description = "Ulteriore insieme di flag operativi, gestiti dalle applicazioni", Prompt="")]
[ErpDogField("AV_ATTRIBUTI2", SqlFieldNameExt="AV_ATTRIBUTI2", SqlFieldOptions="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DataType(DataType.Text)]
public string? AvAttributi2  { get; set; }
}
}
