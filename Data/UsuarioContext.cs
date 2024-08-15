using Microsoft.EntityFrameworkCore;
namespace ProyectoFinal.Data
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext(DbContextOptions<UsuarioContext> options) : base(options) { }

        public DbSet<ProyectoFinal.Models.Usuario> Usuario { get; set; }
    }
}
