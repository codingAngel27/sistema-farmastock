using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class ProductoModel
    {
        public int idProd { get; set; }

        public string? codPro { get; set; }
       
        public string? nomPro { get; set; }

        public int proveedor { get; set; }

        public int stock { get; set; }

        public decimal? precio { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-M-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fechaCompra { get; set; }

        [Display(Name = "idProvee")] public int idProvee { get; set; }
        [Display(Name = "nomProvee")] public string? nomProvee { get; set; }

    }
}
