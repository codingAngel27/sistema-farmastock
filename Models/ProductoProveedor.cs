using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models

{
    public class ProductoProveedor
    {
        public int Id { get; set; }
        public int idProducto { get; set; }
        public string nomProducto { get; set; } = string.Empty;
        public int idProveedor { get; set; }
        public string nomProveedor { get; set; } = string.Empty;
        public int cantidad { get; set; }
        public string correo { get; set; } = string.Empty;
        public string descripcion {  get; set; } = string.Empty;
        public DateTime fechaCompra { get; set; } = DateTime.Now;
    }
}
