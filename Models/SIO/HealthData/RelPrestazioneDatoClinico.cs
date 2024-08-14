using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models.SIO.HealthData {
public class RelPrestazioneDatoClinico : ModelErp {
public const string Description = "Dettaglio delle relazioni tra prestazioni e dati sanitari (generazione, utilizzo)";
public const string SqlTableName = "REL_PRESTAZIONE_DATO_CLINICO";
public const string SqlTableNameExt = "REL_PRESTAZIONE_DATO_CLINICO";
public const string SqlRowIdName = "PD__ID";
public const string SqlRowIdNameExt = "PD__ICODE";
public const string SqlPrefix = "PD_";
public const string SqlPrefixExt = "PD_";
public const string SqlXdataTableName = "PD_XDATA";
public const string SqlXdataTableNameExt = "PD_XDATA";
public const string DATAMODEL = "SIO"; //Data Model Name of the Class
public const int INTCODE = 80; //Internal Table Code
public const string TBAREA = "Dati clinici"; //Table Area
public const string PREFIX = "Pd"; //Table Prefix
public const string LIVEDESC = "L"; //Table type: Live or Description
public const string IS_RELTABLE = "Y"; //Is Relation Table: Yes or No
[Display(Name = "Pd1Ienv", ShortName="", Description = "Parametri dell'ambiente Ienv", Prompt="")]
[ErpDogField("PD__IENV", SqlFieldNameExt="", SqlFieldProperties="")]
[DataType(DataType.Text)]
[StringLength(200, ErrorMessage = "Inserire massimo 200 caratteri")]
public string? Pd1Ienv { get; set; }
[Key]
[Display(Name = "Pd1Icode", ShortName="", Description = "Identificatore univoco dell'istanza (definito automaticamente quando il record viene generato)", Prompt="")]
[ErpDogField("PD__ICODE", SqlFieldNameExt="PD__ICODE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pd1Icode { get; set; }
[Display(Name = "Pd1Deleted", ShortName="", Description = "Se 'Y', l'istanza è logicamente cancellata", Prompt="")]
[ErpDogField("PD__DELETED", SqlFieldNameExt="PD__DELETED", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Pd1Deleted { get; set; }
[Display(Name = "Pd1Timestamp", ShortName="", Description = "Timestamp dell'ultima modifica dell'istanza", Prompt="")]
[ErpDogField("PD__TIMESTAMP", SqlFieldNameExt="PD__TIMESTAMP", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(8, ErrorMessage = "Inserire massimo 8 caratteri")]
public byte[]? Pd1Timestamp { get; set; }
[Display(Name = "Pd1Home", ShortName="", Description = "Posizione principale dell'istanza (cioè il nome del server contenente la copia master)", Prompt="")]
[ErpDogField("PD__HOME", SqlFieldNameExt="PD__HOME", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pd1Home { get; set; }
[Display(Name = "Pd1Version", ShortName="", Description = "Versione dell'istanza", Prompt="")]
[ErpDogField("PD__VERSION", SqlFieldNameExt="PD__VERSION", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(12, ErrorMessage = "Inserire massimo 12 caratteri")]
public string? Pd1Version { get; set; }
[Display(Name = "Pd1Inactive", ShortName="", Description = "Flag di inattività: se Y, l'istanza deve essere considerata come non attiva", Prompt="")]
[ErpDogField("PD__INACTIVE", SqlFieldNameExt="PD__INACTIVE", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
public string? Pd1Inactive { get; set; }
[Display(Name = "Pd1Extatt", ShortName="", Description = "Attributi estesi, definibili dinamicamente come documento XML", Prompt="")]
[ErpDogField("PD__EXTATT", SqlFieldNameExt="PD__EXTATT", SqlFieldProperties="prop()")]
[DataType(DataType.Text)]
public string? Pd1Extatt { get; set; }


[Display(Name = "Classe Dato Clinico", ShortName="", Description = "Partizione del singolo dato sanitario", Prompt="")]
[ErpDogField("PD_CLASSE_DATO_CLINICO", SqlFieldNameExt="PD_CLASSE_DATO_CLINICO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
[DefaultValue(" ")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("1|2|3|4", ErrorMessage = "Inserisci una delle seguenti opzioni: 1|2|3|4")]
public string? PdClasseDatoClinico  { get; set; }

[Display(Name = "Id Dato Clinico", ShortName="", Description = "Identificativo del singolo dato sanitario", Prompt="")]
[ErpDogField("PD_ID_DATO_CLINICO_1", SqlFieldNameExt="PD_ID_DATO_CLINICO_1", SqlFieldOptions="", Xref="Pv1Icode", SqlFieldProperties="prop() xref(PARAMETRO_VITALE.PV__ICODE{PD_CLASSE_DATO_CLINICO='1'} | RISULTATO_ESAME.RE__ICODE{PD_CLASSE_DATO_CLINICO='2'} | STATO_SALUTE.SS__ICODE{PD_CLASSE_DATO_CLINICO='3'} | DOCUMENTO_CLINICO.DC__ICODE{PD_CLASSE_DATO_CLINICO= '4'}) xdup() multbxref(PD_CLASSE_DATO_CLINICO)")]
[DataType(DataType.Text)]
public string? PdIdDatoClinico1  { get; set; }
public ErpToolkit.Models.SIO.HealthData.ParametroVitale? PdIdDatoClinico1Obj  { get; set; }

[Display(Name = "Id Dato Clinico", ShortName="", Description = "Identificativo del singolo dato sanitario", Prompt="")]
[ErpDogField("PD_ID_DATO_CLINICO_2", SqlFieldNameExt="PD_ID_DATO_CLINICO_2", SqlFieldOptions="", Xref="Re1Icode", SqlFieldProperties="prop() xref(PARAMETRO_VITALE.PV__ICODE{PD_CLASSE_DATO_CLINICO='1'} | RISULTATO_ESAME.RE__ICODE{PD_CLASSE_DATO_CLINICO='2'} | STATO_SALUTE.SS__ICODE{PD_CLASSE_DATO_CLINICO='3'} | DOCUMENTO_CLINICO.DC__ICODE{PD_CLASSE_DATO_CLINICO= '4'}) xdup() multbxref(PD_CLASSE_DATO_CLINICO)")]
[DataType(DataType.Text)]
public string? PdIdDatoClinico2  { get; set; }
public ErpToolkit.Models.SIO.HealthData.RisultatoEsame? PdIdDatoClinico2Obj  { get; set; }

[Display(Name = "Id Dato Clinico", ShortName="", Description = "Identificativo del singolo dato sanitario", Prompt="")]
[ErpDogField("PD_ID_DATO_CLINICO_3", SqlFieldNameExt="PD_ID_DATO_CLINICO_3", SqlFieldOptions="", Xref="Ss1Icode", SqlFieldProperties="prop() xref(PARAMETRO_VITALE.PV__ICODE{PD_CLASSE_DATO_CLINICO='1'} | RISULTATO_ESAME.RE__ICODE{PD_CLASSE_DATO_CLINICO='2'} | STATO_SALUTE.SS__ICODE{PD_CLASSE_DATO_CLINICO='3'} | DOCUMENTO_CLINICO.DC__ICODE{PD_CLASSE_DATO_CLINICO= '4'}) xdup() multbxref(PD_CLASSE_DATO_CLINICO)")]
[DataType(DataType.Text)]
public string? PdIdDatoClinico3  { get; set; }
public ErpToolkit.Models.SIO.HealthData.StatoSalute? PdIdDatoClinico3Obj  { get; set; }

[Display(Name = "Id Dato Clinico", ShortName="", Description = "Identificativo del singolo dato sanitario", Prompt="")]
[ErpDogField("PD_ID_DATO_CLINICO_4", SqlFieldNameExt="PD_ID_DATO_CLINICO_4", SqlFieldOptions="", Xref="Dc1Icode", SqlFieldProperties="prop() xref(PARAMETRO_VITALE.PV__ICODE{PD_CLASSE_DATO_CLINICO='1'} | RISULTATO_ESAME.RE__ICODE{PD_CLASSE_DATO_CLINICO='2'} | STATO_SALUTE.SS__ICODE{PD_CLASSE_DATO_CLINICO='3'} | DOCUMENTO_CLINICO.DC__ICODE{PD_CLASSE_DATO_CLINICO= '4'}) xdup() multbxref(PD_CLASSE_DATO_CLINICO)")]
[DataType(DataType.Text)]
public string? PdIdDatoClinico4  { get; set; }
public ErpToolkit.Models.SIO.HealthData.DocumentoClinico? PdIdDatoClinico4Obj  { get; set; }

[Display(Name = "Id Dato Clinico", ShortName="", Description = "Identificativo del singolo dato sanitario", Prompt="")]
[ErpDogField("PD_ID_DATO_CLINICO", SqlFieldNameExt="PD_ID_DATO_CLINICO", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref(PARAMETRO_VITALE.PV__ICODE{PD_CLASSE_DATO_CLINICO='1'} | RISULTATO_ESAME.RE__ICODE{PD_CLASSE_DATO_CLINICO='2'} | STATO_SALUTE.SS__ICODE{PD_CLASSE_DATO_CLINICO='3'} | DOCUMENTO_CLINICO.DC__ICODE{PD_CLASSE_DATO_CLINICO= '4'}) xdup() multbxref(PD_CLASSE_DATO_CLINICO)")]
[Required(ErrorMessage = "Inserire un valore nel campo")]
public string? PdIdDatoClinico  { get; set; }

[Display(Name = "Id Prestazione", ShortName="", Description = "Identificativo dell'atto", Prompt="")]
[ErpDogField("PD_ID_PRESTAZIONE", SqlFieldNameExt="PD_ID_PRESTAZIONE", SqlFieldOptions="", Xref="Pr1Icode", SqlFieldProperties="prop() xref(PRESTAZIONE.PR__ICODE) xdup() multbxref()")]
[DefaultValue("")]
[AutocompleteServer("Prestazione", "AutocompleteGetSelect", "AutocompletePreLoad", 1)]
[DataType(DataType.Text)]
public string? PdIdPrestazione  { get; set; }
public ErpToolkit.Models.SIO.Act.Prestazione? PdIdPrestazioneObj  { get; set; }

[Display(Name = "Tipo Relazione", ShortName="", Description = "Il Dato Sanitario è [G]enerato dall'atto - [R]ilevante per l'esecuzione", Prompt="")]
[ErpDogField("PD_TIPO_RELAZIONE", SqlFieldNameExt="PD_TIPO_RELAZIONE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue("R")]
[StringLength(1, ErrorMessage = "Inserire massimo 1 caratteri")]
[RegularExpression("R|G", ErrorMessage = "Inserisci una delle seguenti opzioni: R|G")]
public string? PdTipoRelazione  { get; set; }

[Display(Name = "Note", ShortName="", Description = "Ulteriori note testuali, relative al collegamento specifico", Prompt="")]
[ErpDogField("PD_NOTE", SqlFieldNameExt="PD_NOTE", SqlFieldOptions="", Xref="", SqlFieldProperties="prop() xref() xdup() multbxref()")]
[DefaultValue(" ")]
[StringLength(40, ErrorMessage = "Inserire massimo 40 caratteri")]
[DataType(DataType.Text)]
public string? PdNote  { get; set; }

public bool TryValidateInt(ModelStateDictionary modelState) 
    { 
        bool isValidate = true; 
        return isValidate; 
    } 

public static List<string> ListIndexes() { 
    return new List<string>() { "sioPd1Icode|K|Pd1Icode","sioPd1RecDate|N|Pd1Mdate,Pd1Cdate"
        ,"sioPdClasseDatoClinicoPdIdDatoClinicoPdIdPrestazionePd1VersionPd1Deleted|U|PdClasseDatoClinico,PdIdDatoClinico,PdIdPrestazione,Pd1Version,Pd1Deleted"
        ,"sioPdIdPrestazionePdClasseDatoClinicoPdIdDatoClinico|N|PdIdPrestazione,PdClasseDatoClinico,PdIdDatoClinico"
        ,"sioPdIdDatoClinico|N|PdIdDatoClinico"
    };
}
}
}
