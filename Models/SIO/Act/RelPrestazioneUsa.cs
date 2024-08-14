using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.Act {
public class RelPrestazioneUsa : ModelErp {
public const string Description = "Risorse pianificate ed effettive utilizzate per l'esecuzione di una singola prestazione individuale";
public const string SqlTableName = "REL_PRESTAZIONE_USA";
public const string SqlTableNameExt = "REL_PRESTAZIONE_USA";
public const string SqlRowIdName = "PU__ID";
public const string SqlRowIdNameExt = "PU__ICODE";
public const string SqlPrefix = "PU_";
public const string SqlPrefixExt = "PU_";
public const string SqlXdataTableName = "PU_XDATA";
public const string SqlXdataTableNameExt = "PU_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 36; //Internal Table Code
public const string TBAREA = "Attività"; //Table Area
public const string PREFIX = "Pu"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "Y"; //Is Relation Table: Yes or No
[Display(Name = "Pu1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("PU__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Pu1Ienv { get; set; }
[Key]
[Display(Name = "Pu1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("PU__ICODE", SqlFieldNameExt="PU__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pu1Icode { get; set; }
[Display(Name = "Pu1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("PU__DELETED", SqlFieldNameExt="PU__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Pu1Deleted { get; set; }
[Display(Name = "Pu1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("PU__TIMESTAMP", SqlFieldNameExt="PU__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Pu1Timestamp { get; set; }
[Display(Name = "Pu1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("PU__HOME", SqlFieldNameExt="PU__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pu1Home { get; set; }
[Display(Name = "Pu1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("PU__VERSION", SqlFieldNameExt="PU__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pu1Version { get; set; }
[Display(Name = "Pu1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("PU__INACTIVE", SqlFieldNameExt="PU__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Pu1Inactive { get; set; }
[Display(Name = "Pu1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("PU__EXTATT", SqlFieldNameExt="PU__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Pu1Extatt { get; set; }


[Display(Name = "Id Prestazione", ShortName="", Description = "Codice dell'atto", Prompt="")]
[ErpDogField("PU_ID_PRESTAZIONE", SqlFieldNameExt="PU_ID_PRESTAZIONE", SqlFieldOptions="", Xref="Pr1Icode", SqlFieldProperties="prop() xref(PRESTAZIONE.PR__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[AutocompleteServer("Prestazione", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? PuIdPrestazione  { get; set; }
public ErpToolkit.Models.SIO.Act.Prestazione? PuIdPrestazioneObj  { get; set; }

[Display(Name = "Classe Risorsa", ShortName="", Description = "Classe di risorsa: E[quipment] - L[ocation] - M[aterial] - S[staff] - D[rug]", Prompt="")]
[ErpDogField("PU_CLASSE_RISORSA", SqlFieldNameExt="PU_CLASSE_RISORSA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup(TIPO_RISORSA.TS_CLASSE_RISORSA[REL_PRESTAZIONE_USA.PU_ID_TIPO_RISORSA] {PU_CLASSE_RISORSA=' '}) multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("E|L|M|D|S", ErrorMessage = "Inserisci una delle seguenti opzioni: E|L|M|D|S")]
public string? PuClasseRisorsa  { get; set; }

[Display(Name = "Id Tipo Risorsa", ShortName="", Description = "Codice del tipo di risorsa", Prompt="")]
[ErpDogField("PU_ID_TIPO_RISORSA", SqlFieldNameExt="PU_ID_TIPO_RISORSA", SqlFieldOptions="", Xref="Ts1Icode", SqlFieldProperties="prop() xref(TIPO_RISORSA.TS__ICODE) xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue("")]
[AutocompleteClient("TipoRisorsa", "AutocompleteGetAll", 1)]
[DataType(DataType.Text)]
public string? PuIdTipoRisorsa  { get; set; }
public ErpToolkit.Models.SIO.Resource.TipoRisorsa? PuIdTipoRisorsaObj  { get; set; }

[Display(Name = "Id Risorsa", ShortName="", Description = "Codice della risorsa effettiva (se applicabile)", Prompt="")]
[ErpDogField("PU_ID_RISORSA_S", SqlFieldNameExt="PU_ID_RISORSA_S", SqlFieldOptions="", Xref="Pe1Icode", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE{PU_CLASSE_RISORSA='S'} | MATERIALE.MT__ICODE{PU_CLASSE_RISORSA='M'} | ATTREZZATURA.AT__ICODE{PU_CLASSE_RISORSA='E'} | SALA.SA__ICODE{PU_CLASSE_RISORSA='L'} | FARMACO.FM__ICODE{PU_CLASSE_RISORSA='D'}) xdup() multbxref(PU_CLASSE_RISORSA)")]
[DataType(DataType.Text)]
public string? PuIdRisorsaS  { get; set; }
public ErpToolkit.Models.SIO.Resource.Personale? PuIdRisorsaSObj  { get; set; }

[Display(Name = "Id Risorsa", ShortName="", Description = "Codice della risorsa effettiva (se applicabile)", Prompt="")]
[ErpDogField("PU_ID_RISORSA_M", SqlFieldNameExt="PU_ID_RISORSA_M", SqlFieldOptions="", Xref="Mt1Icode", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE{PU_CLASSE_RISORSA='S'} | MATERIALE.MT__ICODE{PU_CLASSE_RISORSA='M'} | ATTREZZATURA.AT__ICODE{PU_CLASSE_RISORSA='E'} | SALA.SA__ICODE{PU_CLASSE_RISORSA='L'} | FARMACO.FM__ICODE{PU_CLASSE_RISORSA='D'}) xdup() multbxref(PU_CLASSE_RISORSA)")]
[DataType(DataType.Text)]
public string? PuIdRisorsaM  { get; set; }
public ErpToolkit.Models.SIO.Resource.Materiale? PuIdRisorsaMObj  { get; set; }

[Display(Name = "Id Risorsa", ShortName="", Description = "Codice della risorsa effettiva (se applicabile)", Prompt="")]
[ErpDogField("PU_ID_RISORSA_E", SqlFieldNameExt="PU_ID_RISORSA_E", SqlFieldOptions="", Xref="At1Icode", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE{PU_CLASSE_RISORSA='S'} | MATERIALE.MT__ICODE{PU_CLASSE_RISORSA='M'} | ATTREZZATURA.AT__ICODE{PU_CLASSE_RISORSA='E'} | SALA.SA__ICODE{PU_CLASSE_RISORSA='L'} | FARMACO.FM__ICODE{PU_CLASSE_RISORSA='D'}) xdup() multbxref(PU_CLASSE_RISORSA)")]
[DataType(DataType.Text)]
public string? PuIdRisorsaE  { get; set; }
public ErpToolkit.Models.SIO.Resource.Attrezzatura? PuIdRisorsaEObj  { get; set; }

[Display(Name = "Id Risorsa", ShortName="", Description = "Codice della risorsa effettiva (se applicabile)", Prompt="")]
[ErpDogField("PU_ID_RISORSA_L", SqlFieldNameExt="PU_ID_RISORSA_L", SqlFieldOptions="", Xref="Sa1Icode", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE{PU_CLASSE_RISORSA='S'} | MATERIALE.MT__ICODE{PU_CLASSE_RISORSA='M'} | ATTREZZATURA.AT__ICODE{PU_CLASSE_RISORSA='E'} | SALA.SA__ICODE{PU_CLASSE_RISORSA='L'} | FARMACO.FM__ICODE{PU_CLASSE_RISORSA='D'}) xdup() multbxref(PU_CLASSE_RISORSA)")]
[DataType(DataType.Text)]
public string? PuIdRisorsaL  { get; set; }
public ErpToolkit.Models.SIO.Resource.Sala? PuIdRisorsaLObj  { get; set; }

[Display(Name = "Id Risorsa", ShortName="", Description = "Codice della risorsa effettiva (se applicabile)", Prompt="")]
[ErpDogField("PU_ID_RISORSA_D", SqlFieldNameExt="PU_ID_RISORSA_D", SqlFieldOptions="", Xref="Fm1Icode", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE{PU_CLASSE_RISORSA='S'} | MATERIALE.MT__ICODE{PU_CLASSE_RISORSA='M'} | ATTREZZATURA.AT__ICODE{PU_CLASSE_RISORSA='E'} | SALA.SA__ICODE{PU_CLASSE_RISORSA='L'} | FARMACO.FM__ICODE{PU_CLASSE_RISORSA='D'}) xdup() multbxref(PU_CLASSE_RISORSA)")]
[DataType(DataType.Text)]
public string? PuIdRisorsaD  { get; set; }
public ErpToolkit.Models.SIO.Resource.Farmaco? PuIdRisorsaDObj  { get; set; }

[Display(Name = "Id Risorsa", ShortName="", Description = "Codice della risorsa effettiva (se applicabile)", Prompt="")]
[ErpDogField("PU_ID_RISORSA", SqlFieldNameExt="PU_ID_RISORSA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref(PERSONALE.PE__ICODE{PU_CLASSE_RISORSA='S'} | MATERIALE.MT__ICODE{PU_CLASSE_RISORSA='M'} | ATTREZZATURA.AT__ICODE{PU_CLASSE_RISORSA='E'} | SALA.SA__ICODE{PU_CLASSE_RISORSA='L'} | FARMACO.FM__ICODE{PU_CLASSE_RISORSA='D'}) xdup() multbxref(PU_CLASSE_RISORSA)")]
public string? PuIdRisorsa  { get; set; }

[Display(Name = "Sequenza", ShortName="", Description = "Numero di sequenza della relazione", Prompt="")]
[ErpDogField("PU_SEQUENZA", SqlFieldNameExt="PU_SEQUENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public short? PuSequenza  { get; set; }

[Display(Name = "In Evidenza", ShortName="", Description = "Se impostato su \"Y\", evidenzia le risorse che potrebbero essere sostituite durante il processo di acquisizione", Prompt="")]
[ErpDogField("PU_IN_EVIDENZA", SqlFieldNameExt="PU_IN_EVIDENZA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("Y|N| ", ErrorMessage = "Inserisci una delle seguenti opzioni: Y|N| ")]
public string? PuInEvidenza  { get; set; }

[Display(Name = "Data Inizio Uso", ShortName="", Description = "Data di inizio dell'utilizzo", Prompt="")]
[ErpDogField("PU_DATA_INIZIO_USO", SqlFieldNameExt="PU_DATA_INIZIO_USO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public string? PuDataInizioUso  { get; set; }

[Display(Name = "Ora Inizio Uso", ShortName="", Description = "Ora di inizio dell'utilizzo", Prompt="")]
[ErpDogField("PU_ORA_INIZIO_USO", SqlFieldNameExt="PU_ORA_INIZIO_USO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? PuOraInizioUso  { get; set; }

[Display(Name = "Data Fine Uso", ShortName="", Description = "Data di fine dell'utilizzo", Prompt="")]
[ErpDogField("PU_DATA_FINE_USO", SqlFieldNameExt="PU_DATA_FINE_USO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("    /  /  ")]
[StringLength(10, ErrorMessage = "Inserire massimo 10 caratteri")]
[DataType(DataType.Date)]
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
public string? PuDataFineUso  { get; set; }

[Display(Name = "Ora Fine Uso", ShortName="", Description = "Ora di fine dell'utilizzo", Prompt="")]
[ErpDogField("PU_ORA_FINE_USO", SqlFieldNameExt="PU_ORA_FINE_USO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
[DataType(DataType.Time)]
[DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
public string? PuOraFineUso  { get; set; }

[Display(Name = "Quantita Prevista", ShortName="", Description = "Quantità pianificata da utilizzare", Prompt="")]
[ErpDogField("PU_QUANTITA_PREVISTA", SqlFieldNameExt="PU_QUANTITA_PREVISTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? PuQuantitaPrevista  { get; set; }

[Display(Name = "Unita Di Misura Prevista", ShortName="", Description = "Unità di misura della quantità pianificata", Prompt="")]
[ErpDogField("PU_UNITA_DI_MISURA_PREVISTA", SqlFieldNameExt="PU_UNITA_DI_MISURA_PREVISTA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? PuUnitaDiMisuraPrevista  { get; set; }

[Display(Name = "Quantita Usata", ShortName="", Description = "Quantità effettiva utilizzata", Prompt="")]
[ErpDogField("PU_QUANTITA_USATA", SqlFieldNameExt="PU_QUANTITA_USATA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? PuQuantitaUsata  { get; set; }

[Display(Name = "Unita Di Misura Usata", ShortName="", Description = "Unità di misura della quantità utilizzata", Prompt="")]
[ErpDogField("PU_UNITA_DI_MISURA_USATA", SqlFieldNameExt="PU_UNITA_DI_MISURA_USATA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? PuUnitaDiMisuraUsata  { get; set; }

[Display(Name = "Quantita Restituita", ShortName="", Description = "Quantità eventualmente restituita al fornitore o al magazzino", Prompt="")]
[ErpDogField("PU_QUANTITA_RESTITUITA", SqlFieldNameExt="PU_QUANTITA_RESTITUITA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? PuQuantitaRestituita  { get; set; }

[Display(Name = "Unita Di Misura Restituita", ShortName="", Description = "Unità di misura della quantità restituita al fornitore o al magazzino", Prompt="")]
[ErpDogField("PU_UNITA_DI_MISURA_RESTITUITA", SqlFieldNameExt="PU_UNITA_DI_MISURA_RESTITUITA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
[DataType(DataType.Text)]
public string? PuUnitaDiMisuraRestituita  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Note testuali aggiuntive", Prompt="")]
[ErpDogField("PU_NOTE", SqlFieldNameExt="PU_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(40, ErrorMessage = "Inserire massimo 40 caratteri")]
[DataType(DataType.Text)]
public string? PuNote  { get; set; }

[Display(Name = "Costo Risorsa", ShortName="", Description = "Costo effettivo di tale utilizzo", Prompt="")]
[ErpDogField("PU_COSTO_RISORSA", SqlFieldNameExt="PU_COSTO_RISORSA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("")]
public double? PuCostoRisorsa  { get; set; }

[Display(Name = "Descrizione Risorsa Usata", ShortName="", Description = "Descrizione testuale delle risorse utilizzate", Prompt="")]
[ErpDogField("PU_DESCRIZIONE_RISORSA_USATA", SqlFieldNameExt="PU_DESCRIZIONE_RISORSA_USATA", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(120, ErrorMessage = "Inserire massimo 120 caratteri")]
[DataType(DataType.Text)]
public string? PuDescrizioneRisorsaUsata  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioPu1Icode|K|Pu1Icode","sioPu1RecDate|N|Pu1Mdate,Pu1Cdate"
        ,"sioPuIdTipoRisorsaPuIdPrestazionePuDataInizioUso|N|PuIdTipoRisorsa,PuIdPrestazione,PuDataInizioUso"
        ,"sioPuIdPrestazionePuDataInizioUsoPuIdTipoRisorsaPuIdRisorsa|N|PuIdPrestazione,PuDataInizioUso,PuIdTipoRisorsa,PuIdRisorsa"
        ,"sioPuIdRisorsaPuIdPrestazionePuDataInizioUso|N|PuIdRisorsa,PuIdPrestazione,PuDataInizioUso"
        ,"sioPuDataInizioUso|N|PuDataInizioUso"
    };
}
}
}
