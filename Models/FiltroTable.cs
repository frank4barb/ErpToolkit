using ErpToolkit.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models
{
    public class FiltroTable
    {
        //System.ComponentModel.DataAnnotations.DataTypeAttribute

        [Display(Name = "Sanitario", Description = "Ricerca per Sanitario. Non è necessario specificare data inizio e fine")]
        [ErpSanitario]
        public string? Sanitario { get; set; }
        [Display(Name = "Testo libero", Description = "Inserire il testo libero da ricercare tra data inizio e fine. Minimo 3 caratteri.")]
        [DataType(DataType.Text)]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Inserire minimo 3 caratteri, massimo 30")]
        public string? TestoLibero { get; set; }
        [Display(Name = "Data inizio", Description = "Inserire la data di inizio ricerca. Obbigatoria in caso di ricerca per solo testo libero")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [ErpControlloDataInizio("DataFine")]
        public string? DataInizio { get; set; }
        [Display(Name = "Data fine", Description = "Inserire la data di fine ricerca. Obbigatoria in caso di ricerca per solo testo libero")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [ErpControlloDataFine("DataInizio")]
        public string? DataFine { get; set; }

    }
}
