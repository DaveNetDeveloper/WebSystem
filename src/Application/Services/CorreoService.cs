using System.Net;
using System.Net.Mail;
using System.Text;

using Application.Interfaces;
using Domain.Entities;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.Services
{
    public class CorreoService : ICorreoService
    {
        private readonly ITipoEnvioCorreoRepository _repoTipoEnvioCorreo;

        public CorreoService(ITipoEnvioCorreoRepository repo) {
            _repoTipoEnvioCorreo = repo;
        }

        public Task<IEnumerable<TipoEnvioCorreo>> ObtenerTiposEnvioCorreo() { 
            return _repoTipoEnvioCorreo.GetAllAsync(); 
        }

        public Guid EnviarCorreo(Correo correo, string servidorSmtp, string puertoSmtp, string usuarioSmtp, string contrasenaSmtp)
        {
            using (var mensaje = new MailMessage()) {
                mensaje.From = new MailAddress(usuarioSmtp);
                mensaje.To.Add(correo.Destinatario);
                mensaje.Subject = correo.Asunto;
                mensaje.Body = correo.Cuerpo;
                mensaje.IsBodyHtml = true;

                using (var clienteSmtp = new SmtpClient(servidorSmtp, Convert.ToInt32(puertoSmtp))) { 
                    clienteSmtp.Credentials = new NetworkCredential(usuarioSmtp, contrasenaSmtp);
                    clienteSmtp.EnableSsl = true;
                    clienteSmtp.Send(mensaje);
                }
            }
            return correo.EmailToken.Value;
        }
    }
}