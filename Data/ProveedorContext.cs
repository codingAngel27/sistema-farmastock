using Microsoft.EntityFrameworkCore;


namespace ProyectoFinal.Data
{
    public class ProveedorContext: DbContext
    {
        public ProveedorContext(DbContextOptions<ProveedorContext> options) : base(options) {}

        public DbSet<ProyectoFinal.Models.Proveedor> Proveedor { get; set; }
    }
}
