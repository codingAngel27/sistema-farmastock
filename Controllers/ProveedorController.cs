using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using ProyectoFinal.Data;
using ProyectoFinal.Models;
using ProyectoFinal.Service;

namespace ProyectoFinal.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IConfiguration _configuration;
        //private readonly ProveedorService _service;
       

        public ProveedorController(IConfiguration configuration)
        {
            _configuration = configuration;
            //_service = service;
          
        }


        IEnumerable<Proveedor> listarProveedor()
        {
            List<Proveedor> lista = new List<Proveedor>();
            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                SqlCommand cmd = new SqlCommand("usp_listarProveedor", cn);
                cn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    lista.Add(new Proveedor
                    {
                        id = r.GetInt32(0),
                        ruc = r.GetString(1),
                        nomProvee = r.GetString(2),
                        email = r.GetString(3),
                        telefono = r.GetString(4),
                        direccion = r.GetString(5),

                    });
                }
                
            }
            return lista;
        }

        IEnumerable<Proveedor> BuscarProveedor(string nomProvee)
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                SqlCommand cmd = new SqlCommand("usp_BuscarProveedorPorNombre", cn);
                cmd.Parameters.AddWithValue("@nomProvee", nomProvee);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        proveedores.Add(new Proveedor
                        {
                            id = reader.GetInt32(0),
                            ruc = reader.GetString(1),
                            nomProvee = reader.GetString(2),
                            email = reader.GetString(3),
                            telefono = reader.GetString(4),
                            direccion = reader.GetString(5),
                        });
                    }
                }
               
                
            }
            return  proveedores;
        }

        public async Task<IActionResult> Create()
        {
            return View(new Proveedor());
        }

      

        public async Task<IActionResult> Index(string nomProvee, int pag)
        {
            ViewBag.nomProvee = new SelectList(listarProveedor(), "nomProvee", "nomProvee");

            if (nomProvee == null)
            {
                nomProvee = string.Empty;
            }

            int filas = 10;
            IEnumerable<Proveedor> lista = BuscarProveedor(nomProvee);
            int n = lista.Count();
            int paginas = n % filas > 0 ? n / filas + 1 : n / filas;

            ViewBag.paginas = paginas;
            ViewBag.pag = pag;

            return View(await Task.Run(() => lista.ToList().Skip(pag * filas).Take(filas)));
        }


        public async Task<IActionResult>Edit(int id)
        {
            Proveedor? pro = listarProveedor().Where(x => x.id == id).FirstOrDefault();
            if (pro == null)
            {
                return RedirectToAction("Index");
            }

            return View(pro);
        }

        public async Task<IActionResult>BuscarProveedorIn(string nomProvee,int pag)
            
        {
            ViewBag.nomProvee = new SelectList(listarProveedor(), "nomProvee", "nomProvee");

            if (nomProvee == null)
            {
                nomProvee = string.Empty;
            }

            int filas = 10;
            IEnumerable<Proveedor> lista = BuscarProveedor(nomProvee);
            int n = lista.Count();
            int paginas = n % filas > 0 ? n / filas + 1 : n / filas;

            ViewBag.paginas = paginas;
            ViewBag.pag = pag;

            return View(await Task.Run(() => lista.ToList().Skip(pag * filas).Take(filas)));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
            {
                return View(proveedor);
            }
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_CrearProveedor", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ruc", proveedor.ruc);
                    cmd.Parameters.AddWithValue("@nomProvee", proveedor.nomProvee);
                    cmd.Parameters.AddWithValue("@email", proveedor.email);
                    cmd.Parameters.AddWithValue("@telefono", proveedor.telefono);
                    cmd.Parameters.AddWithValue("@direccion", proveedor.direccion);
                    cn.Open();
                    int cantidad = cmd.ExecuteNonQuery();
                    mensaje = $"se ha ingresado {cantidad} Proveedor";
                }
                catch (Exception ex)
                {
                    mensaje += ex.Message;
                }
                ViewBag.mensaje = mensaje;
                return View(proveedor);


            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Proveedor proveedor)
        {
            if(!ModelState .IsValid)
            {
                return View(proveedor);
            }
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))

            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_EditarProveedor", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", proveedor.id);
                    cmd.Parameters.AddWithValue("@ruc", proveedor.ruc);
                    cmd.Parameters.AddWithValue("@nomProvee", proveedor.nomProvee);
                    cmd.Parameters.AddWithValue("@email", proveedor.email);
                    cmd.Parameters.AddWithValue("@telefono", proveedor.telefono);
                    cmd.Parameters.AddWithValue("@direccion", proveedor.direccion);
                    cn.Open();
                    int cantidad = cmd.ExecuteNonQuery();
                    mensaje = $"se ha Actualizado {cantidad} Proveedor";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
                ViewBag.mensaje = mensaje;
            }
            return View(proveedor);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            string mensaje = string.Empty;

            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_EliminarProveedor", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cn.Open();

                   
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    mensaje = filasAfectadas > 0 ? "Proveedor eliminado con éxito" : "No se encontró el proveedor";
                }
                catch (Exception ex)
                {
                    mensaje = $"Error al eliminar proveedor: {ex.Message}";
                }
                ViewBag.mensaje = mensaje;
            }

            
            return RedirectToAction("BuscarProveedorIn");
        }


        public async Task<IActionResult> Details(int id)
        {
            Proveedor proveedor = null;

            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                SqlCommand cmd = new SqlCommand("usp_BuscarProveedorPorID", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        proveedor = new Proveedor
                        {
                            id = reader.GetInt32(0),
                            ruc = reader.GetString(1),
                            nomProvee = reader.GetString(2),
                            email = reader.GetString(3),
                            telefono = reader.GetString(4),
                            direccion = reader.GetString(5),
                        };
                    }
                }
            }

            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

    }

}


