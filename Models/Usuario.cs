using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class Usuario
    {
        [Display(Name = "Id", Order = 0)]
        public int id { get; set; }
        [Display(Name = "Nombres", Order = 1)]
        public string? nom { get; set; }
        [Display(Name = "Apellidos", Order = 2)]
        public string? ape { get; set; }
        [Display(Name = "Email", Order = 3)]
        public string? email { get; set; }
        [Display(Name = "Contraseña", Order = 4)]
        public string? clave { get; set; }
        [Display(Name = "Telefono", Order = 5)]
        public string? telefono { get; set; }
    }
}
