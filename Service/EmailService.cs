using System.Net.Mail;
using System.Net;

namespace ProyectoFinal.Service
{
    public class EmailService
    {
        public void Enviar(string correo, string token)
        {
            Correo(correo, token);
        }

        void Correo(string correo_receptor, string token)
        {
            string correo_emisor = "farmastockpe@outlook.com";
            string clave_emisor = "holamundo01";

            MailAddress receptor = new MailAddress(correo_receptor);
            MailAddress emisor = new MailAddress(correo_emisor);

            MailMessage email = new MailMessage(emisor, receptor)
            {
                Subject = "ANGEL DEV validacion de cuenta",
                Body = $"Por favor, activa tu cuenta haciendo clic en el siguiente enlace: https://localhost:7250/Usuario/Token?valor={token}"
            };

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                Credentials = new NetworkCredential(correo_emisor, clave_emisor),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            try
            {
                smtp.Send(email);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
                // Opcional: lanzar una excepción o manejar el error según sea necesario
            }
        }
    }
}
