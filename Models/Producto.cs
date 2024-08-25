using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class Producto
    {
        [Display(Name = "id", Order = 0)]
        public int id { get; set; }

        [Display(Name = "codPro", Order = 0)]
        public string codPro {  get; set; } = string.Empty;

        [Display(Name = "nomPro", Order = 0)]
        public string  nomPro { get; set; } = string.Empty;

        [Display(Name = "proveedor", Order = 0)]
        public int proveedor { get; set; }

        [Display(Name = "stock", Order = 0)]
        public int stock {  get; set; }

        [Display(Name = "precio", Order = 0)]
        public decimal precio {  get; set; }

        [Display(Name = "fechaCompra", Order = 0)]
        public DateTime fechaCompra {  get; set; }


    }
}
