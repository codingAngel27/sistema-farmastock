using ProyectoFinal.Models;
using ProyectoFinal.Data;
using Microsoft.EntityFrameworkCore;
namespace ProyectoFinal.Service
{
    public class UsuarioService
  
    {
        private readonly UsuarioContext _context;

        public UsuarioService(UsuarioContext context)
        {
            _context = context;
        }
        public async Task<Usuario> GetUsuario(string email, string clave)
        {
            Usuario usuario_encontrado = await _context.Usuario.Where(u => u.email == email && u.clave == clave).FirstOrDefaultAsync();

            return usuario_encontrado;
        }
    }
}
