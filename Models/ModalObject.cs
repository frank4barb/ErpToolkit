using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErpToolkit.Models
{
    public class ModalObject
    {
        [Required(ErrorMessage = "Required ObjData.data")]
        public object data { get; set; }
    }
}
