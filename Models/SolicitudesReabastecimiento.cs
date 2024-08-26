using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class SolicitudesReabastecimiento
    {
        [Display(Name = "id", Order = 0)]
        public int id {  get; set; }

        [Display(Name = "descripcion", Order = 0)]
        public string descripcion {  get; set; }

        [Display(Name = "fecha", Order = 0)]
        public DateTime fecha { get; set; }

        [Display(Name = "idPro", Order = 0)]
        public  int idPro {  get; set; }

        [Display(Name = "cantidad", Order = 0)]
        public int cantidad {  get; set; }

        [Display(Name = "idProveedor", Order = 0)]
        public int idProveedor {  get; set; }
    }
}
