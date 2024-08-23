using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using ProyectoFinal.Models;
using System.Data;

namespace ProyectoFinal.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IConfiguration _configuration;
        public ProductoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //IENUMERABLE
        IEnumerable<ProductoModel> listarProductos()
        {
            List<ProductoModel> listaPro = new List<ProductoModel>();
            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_listarProducto", cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listaPro.Add(new ProductoModel
                    {
                        idProd = reader.GetInt32(0),
                        codPro = reader.GetString(1),
                        nomPro = reader.GetString(2),
                        nomProvee = reader.GetString(7),
                        stock = reader.GetInt32(4),
                        precio = reader.GetDecimal(5),
                        fechaCompra = reader.GetDateTime(6)
                    });
                }
                cn.Close();
            }
            return listaPro;
        }
        IEnumerable<ProductoModel> buscaProductos(String nombre)
        {
            List<ProductoModel> productos = new List<ProductoModel>();

            using (SqlConnection cn = new(_configuration["ConnectionStrings:cnDB"]))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_ProductoBuscar", cn);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    productos.Add(new ProductoModel
                    {
                        idProd = reader.GetInt32(0),
                        codPro = reader.GetString(1),
                        nomPro = reader.GetString(2),
                        nomProvee = reader.GetString(7),
                        stock = reader.GetInt32(4),
                        precio = reader.GetDecimal(5),
                        fechaCompra = reader.GetDateTime(6)

                    });
                }
            }
            return productos;
        }
        IEnumerable<ProveedorModel> listarProveedor()
        {
            List<ProveedorModel> listaProv = new List<ProveedorModel>();
            using (SqlConnection cn = new(_configuration["ConnectionStrings:cnDB"]))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_ProveedorListar", cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listaProv.Add(new ProveedorModel
                    {
                        idProvee = reader.GetInt32(0),
                        nomProvee = reader.GetString(1)
                    });
                }
            }
            return listaProv;
        }


        //IACTIONRESULT
        public async Task<IActionResult> Index(string nombre)
        {
            if (!String.IsNullOrEmpty(nombre))
             {
                 return View(await Task.Run(() => buscaProductos(nombre)));
             } 
             else
             {
                 return View(await Task.Run(() => listarProductos()));
             }
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.proveedor = new SelectList(await Task.Run(() => listarProveedor()), "idProvee", "nomProvee");
            return View(new ProductoModel());

        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductoModel regProducto)
        {
            if (!ModelState.IsValid)
            {
                return View(regProducto);
            }
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_Producto_Merge", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codPro", regProducto.codPro);
                    cmd.Parameters.AddWithValue("@nomPro", regProducto.nomPro);
                    ProveedorModel? regProveedor = listarProveedor().Where(x => x.idProvee == Int32.Parse(regProducto.nomProvee)).FirstOrDefault();
                    cmd.Parameters.AddWithValue("@proveedor", regProveedor.idProvee);
                    cmd.Parameters.AddWithValue("@stock", regProducto.stock);
                    cmd.Parameters.AddWithValue("@precio", regProducto.precio);
                    cmd.Parameters.AddWithValue("@fechaCompra", regProducto.fechaCompra);
                    cn.Open();
                    int cantidad = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha generado {cantidad} producto";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
                ViewBag.mensaje = mensaje;
                ViewBag.proveedor = new SelectList(await Task.Run(() => listarProveedor()), "idProvee", "nomProvee", regProducto.idProvee);
                return View(regProducto);
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            ProductoModel producto = listarProductos().FirstOrDefault(p => p.idProd == id);

            if (producto == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.proveedor = new SelectList(await Task.Run(() => listarProveedor()), "idProvee", "nomProvee", producto.proveedor);

            return View(producto);
        }

        [HttpPost]public async Task<IActionResult> Edit(ProductoModel regProducto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.proveedor = new SelectList(await Task.Run(() => listarProveedor()), "idProvee", "nomProvee", regProducto.proveedor);
                return View(regProducto);
            }
            string mensaje;
            string alertClass;
            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_Producto_Merge", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                   // cmd.Parameters.AddWithValue("@idProd", regProducto.idProd);
                    cmd.Parameters.AddWithValue("@codPro", regProducto.codPro);
                    cmd.Parameters.AddWithValue("@nomPro", regProducto.nomPro);
                    ProveedorModel? regProveedor = listarProveedor().FirstOrDefault(x => x.idProvee == Int32.Parse(regProducto.nomProvee));
                    cmd.Parameters.AddWithValue("@proveedor", regProveedor?.idProvee); 
                    cmd.Parameters.AddWithValue("@stock", regProducto.stock);
                    cmd.Parameters.AddWithValue("@precio", regProducto.precio);
                    cmd.Parameters.AddWithValue("@fechaCompra", regProducto.fechaCompra);

                    cn.Open();
                    int cantidad = cmd.ExecuteNonQuery();

                    mensaje = $"Se ha actualizado {cantidad} producto(s) exitosamente.";
                    alertClass = "alert-success"; 
                }
                catch (Exception ex)
                {
                    mensaje = $"Error: {ex.Message}";
                    alertClass = "alert-danger";
                }
            }

            ViewBag.proveedor = new SelectList(await Task.Run(() => listarProveedor()), "idProvee", "nomProvee", regProducto.proveedor);
            ViewBag.mensaje = mensaje;
            ViewBag.alertClass = alertClass;

            return View(regProducto);
        }

        // GET: Producto/Delete
        public async Task<IActionResult> Delete(int id)
        {
            ProductoModel producto = listarProductos().FirstOrDefault(p => p.idProd == id);

            if (producto == null)
            {
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        // POST: Producto/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string mensaje;
            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_ProductoEliminar", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProd", id);

                    cn.Open();
                    int cantidad = cmd.ExecuteNonQuery();

                    if (cantidad > 0)
                    {
                        mensaje = $"Se ha eliminado el producto con ID {id}.";
                    }
                    else
                    {
                        mensaje = "No se encontró el producto para eliminar.";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
            }
            ViewBag.mensaje = mensaje;
            return RedirectToAction("Index");
        }


        // GET: Producto/Details
        public async Task<IActionResult> Details(int id)
        {
            ProductoModel producto = listarProductos().FirstOrDefault(p => p.idProd == id);

            if (producto == null)
            {
                return RedirectToAction("Index");
            }
            return View(producto);
        }
    }
}
