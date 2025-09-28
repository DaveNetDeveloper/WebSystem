using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;
using Utilities;

namespace Application.Services
{
    public class CorreoService : ICorreoService
    {
        private readonly ITipoEnvioCorreoRepository _repoTipoEnvioCorreo;
        private readonly MailConfiguration _mailConfig;

        public CorreoService(ITipoEnvioCorreoRepository repo,
                             IOptions<MailConfiguration> mailConfig) {
            _repoTipoEnvioCorreo = repo;
            _mailConfig = mailConfig.Value;
        }

        public CorreoService(ITipoEnvioCorreoRepository repo)
        {
            _repoTipoEnvioCorreo = repo;
        }

        public Task<IEnumerable<TipoEnvioCorreo>> ObtenerTiposEnvioCorreo() { 
            return _repoTipoEnvioCorreo.GetAllAsync(); 
        }

        public Guid EnviarCorreo(Correo correo)
        {
            using (var mensaje = new MailMessage())
            {
                mensaje.From = new MailAddress(EncodeDecodeHelper.GetDecodeValue(_mailConfig.UsuarioSmtp));
                mensaje.To.Add(correo.Destinatario);
                mensaje.Subject = correo.Asunto;
                mensaje.Body = correo.Cuerpo;
                mensaje.IsBodyHtml = true;

                using (var clienteSmtp = new SmtpClient(EncodeDecodeHelper.GetDecodeValue(_mailConfig.ServidorSmtp), Convert.ToInt32(EncodeDecodeHelper.GetDecodeValue(_mailConfig.PuertoSmtp))))
                {
                    clienteSmtp.Credentials = new NetworkCredential(EncodeDecodeHelper.GetDecodeValue(_mailConfig.UsuarioSmtp), EncodeDecodeHelper.GetDecodeValue(_mailConfig.ContrasenaSmtp));
                    clienteSmtp.EnableSsl = true;
                    clienteSmtp.Send(mensaje);
                }
            }
            return correo.EmailToken.Value;
        }
    }
}