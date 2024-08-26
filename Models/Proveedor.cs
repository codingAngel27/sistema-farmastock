
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class Proveedor
    {
        [Display(Name = "id", Order = 0)]
        public int id { get; set; }

        [Display(Name = "ruc", Order = 1)]
        public string? ruc {  get; set; }

        [Display(Name = "nomProvee", Order = 2)]
        public string? nomProvee { get; set; }

        [Display(Name = "email", Order = 3)]
        public string? email { get; set; }

        [Display(Name = "telefono", Order = 4)]
        public string? telefono { get; set; }

        [Display(Name = "direccion", Order = 5)]
        public string? direccion {  get; set; }


    }

}
