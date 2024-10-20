using ErpToolkit.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ErpToolkit.Models {
    public class ModelErp {

        //xx// SGANCIO DAL MODELLO IL CONCETTO DI VISIBILITA'
        //xx////attributi di visualizzazione dei campi definiti a run-time
        //xx//public Dictionary<string, DogHelper.FieldAttr> AttrFields { get; set; } = new Dictionary<string, DogHelper.FieldAttr>();

        // proprietà necessarie per la mantain del record
        public char? action = null; 
        public IDictionary<string, string> options = new Dictionary<string, string>();  

        //metodi obbligatori
        public virtual bool TryValidateInt(ModelStateDictionary modelState)
        {
            return true;
        }


    }
}
