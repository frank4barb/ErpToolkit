using ErpToolkit.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MemoryPack;

namespace ErpToolkit.Models
{
   
    public class InputLogin
    {
        //[Required(ErrorMessage = "Required Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        //        [ErpMatricola]


        [Display(Name = "Matricola", Description = "Inserire la matricola di Login")]
        [Required(ErrorMessage = "La Matricola è richiesta")]
        public string Matricola { get; set; }
        [Display(Name = "Password", Description = "Inserire la password LDAP")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La password è richiesta")]
        public string Password { get; set; }

       
    }
}
