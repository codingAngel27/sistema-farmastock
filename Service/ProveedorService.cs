using ProyectoFinal.Data;

namespace ProyectoFinal.Service
{
    public class ProveedorService
    {
        private readonly ProveedorContext _context;

        public ProveedorService(ProveedorContext context)
        {
            _context = context;
        }
    }
}
