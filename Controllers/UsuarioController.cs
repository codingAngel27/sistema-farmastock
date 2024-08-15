using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoFinal.Models;
using ProyectoFinal.Recursos;
using ProyectoFinal.Service;
using System.Data;
using System.Security.Claims;

namespace ProyectoFinal.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly UsuarioService _usuarioService;
        public UsuarioController(IConfiguration configuration, EmailService emailService, UsuarioService usuarioService)
        {
            _emailService = emailService;
            _configuration = configuration;
            _usuarioService = usuarioService;
        }
        public IActionResult Registrar()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registrar(Usuario u)
        {
            if (!ModelState.IsValid)
            {
                return View(u);
            }

            string mensaje = string.Empty;

            using (SqlConnection con = new SqlConnection(_configuration["ConnectionStrings:cnDB"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Nom", SqlDbType.VarChar).Value = u.nom;
                    cmd.Parameters.Add("@Ape", SqlDbType.VarChar).Value = u.ape;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = u.email;
                    cmd.Parameters.Add("@Clave", SqlDbType.VarChar).Value = Utils.EncriptarClave(u.clave);
                    cmd.Parameters.Add("@Telefono", SqlDbType.VarChar).Value = u.telefono;

                    con.Open();
                    await cmd.ExecuteNonQueryAsync();

                    // Generar y enviar el token
                    string token = Guid.NewGuid().ToString();
                    _emailService.Enviar(u.email, token);
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                    // Aquí podrías retornar la vista con el mensaje de error
                    ViewBag.mensaje = mensaje;
                    return View(u);
                }
                finally
                {
                    con.Close();
                }
            }
            ViewBag.mensaje = "Usuario registrado correctamente";
            return View();
        }
        public IActionResult Token(string valor)
        {
            if (string.IsNullOrEmpty(valor))
            {
                // Redirige a una página de error si el token es nulo o vacío
                return RedirectToAction("Error", "Home");
            }

            // Aquí puedes realizar una validación o un registro de que el enlace fue accedido

            // Retorna una vista con un mensaje de éxito
            ViewBag.Message = "Tu cuenta ha sido activada con éxito.";
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            Usuario usuario_encontrado = await _usuarioService.GetUsuario(correo, Utils.EncriptarClave(clave));
            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,usuario_encontrado.nom)
            };

            ClaimsIdentity ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(ci),
                properties
                );

            return RedirectToAction("Index", "Home");
        }
    }

}
