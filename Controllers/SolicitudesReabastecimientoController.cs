using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using ProyectoFinal.Models;
using System.Net.Mail;
using System.Net;

namespace ProyectoFinal.Controllers
{
    public class SolicitudesReabastecimientoController : Controller
    {
        private readonly IConfiguration _configuration;
        public SolicitudesReabastecimientoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        

        IEnumerable<ProductoProveedor> listarSolicitud()
        {
            List<ProductoProveedor> lista = new List<ProductoProveedor>();
            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                SqlCommand cmd = new SqlCommand("usp_ListarSolicitudesReabastecimiento", cn);
                cn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    lista.Add(new ProductoProveedor
                    {
                        Id = r.GetInt32(0),
                       idProducto = r.GetInt32(1),
                       nomProducto = r.GetString(2),
                        idProveedor = r.GetInt32(3),
                        nomProveedor = r.GetString(4),
                        correo = r.GetString(5),
                        fechaCompra = r.GetDateTime(6),
                        descripcion = r.GetString(7),
                        cantidad = r.GetInt32(8),
                        
                    });
                }
            }
            return lista;
        }

        public async Task<IActionResult> Index(int pag)
        {
            int filas = 8;
            IEnumerable<ProductoProveedor> lista = listarSolicitud();
            int n = lista.Count();
            int paginas = n % filas > 0 ? n / filas + 1 : n / filas;

            ViewBag.paginas = paginas;
            ViewBag.pag = pag;

            return View(await Task.Run(() => lista.ToList().Skip(pag * filas).Take(filas)));
            
        }




        public ProductoProveedor buscarPorducto(string nomPro)
        {

            if (nomPro == null)
            {
                return new ProductoProveedor();
            }

            List<ProductoProveedor> productos = new List<ProductoProveedor>();
            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                SqlCommand cmd = new SqlCommand("usp_BuscarProductoYProveedorPorNombre", cn);
                cmd.Parameters.AddWithValue("@nomPro", nomPro);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new ProductoProveedor
                        {
                          idProducto = reader.GetInt32(0),
                          nomProducto = reader.GetString(1),
                          idProveedor = reader.GetInt32(2),
                          nomProveedor = reader.GetString(3),
                          correo = reader.GetString(4),
                        });
                    }
                }



                return productos.First();
                
            }
        }



        [HttpPost]
        public async Task<IActionResult> Create(ProductoProveedor prodProve)
        {
            if(!ModelState.IsValid)
            {
                return View(prodProve);
            }

            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_CrearSolicitudReabastecimiento", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idPro", prodProve.idProducto);
                    cmd.Parameters.AddWithValue("@cantidad", prodProve.cantidad);
                    cmd.Parameters.AddWithValue("@IdProveedor", prodProve.idProveedor);
                    cmd.Parameters.AddWithValue("@correo", prodProve.correo);
                    cmd.Parameters.AddWithValue("@fecha", prodProve.fechaCompra);
                    cmd.Parameters.AddWithValue("@descripcion", prodProve.descripcion);
                    cn.Open();

                    int cantidad = cmd.ExecuteNonQuery();





                    enviarCorreo(prodProve.correo,"solicitud", "Se solicita " + prodProve.cantidad  +  prodProve.nomProducto );

                    mensaje = $"se ha generado {cantidad} Solicitud";

                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
                ViewBag.mensaje = mensaje;
              
                return View(await Task.Run(() => new ProductoProveedor()));
            }
        }

        public async Task<IActionResult> Create(string nomPro)
        {
            return View(await Task.Run(() => buscarPorducto(nomPro)));
        }



        public void enviarCorreo(string correo,string asunto, string contenido)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("c95ae2fbdfc4cb@mailtrap.io");
            mailMessage.To.Add(correo);
            mailMessage.Subject = asunto;
            mailMessage.Body = contenido;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "sandbox.smtp.mailtrap.io";
            smtpClient.Port = 2525;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("c95ae2fbdfc4cb", "44228688ab00b4");
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email Sent Successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

    }
}
