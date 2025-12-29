using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;
using Utilities;
using static Domain.Entities.TipoEnvioCorreo;

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

        public async Task<TipoEnvioCorreo> ObtenerTipoEnvioCorreo(TipoEnvioCorreos tipoEnvio)
        {
            var tipos = await _repoTipoEnvioCorreo.GetAllAsync();
            var tipo = tipos.Where(t => t.nombre == tipoEnvio.ToString()).SingleOrDefault(); 
            return tipo;
        }

        public Guid? EnviarCorreo_Nuevo(CorreoN correo)
        {
            var guidRelated = Guid.NewGuid();
            using (var mensaje = new MailMessage())
            {
                mensaje.From = new MailAddress(EncodeDecodeHelper.GetDecodeValue(_mailConfig.UsuarioSmtp));
                mensaje.To.Add(correo.Destinatario);
                mensaje.Subject = correo.Asunto;
                mensaje.Body = correo.Cuerpo;
                mensaje.IsBodyHtml = true;

                bool hasAttachment = correo.FicheroAdjunto != null && correo.FicheroAdjunto.Archivo.Length > 0;

                if (hasAttachment)
                {
                    var stream = new MemoryStream(correo.FicheroAdjunto.Archivo);
                    var attachment = new Attachment(stream, correo.FicheroAdjunto.NombreArchivo, correo.FicheroAdjunto.ContentType);
                    mensaje.Attachments.Add(attachment);
                }

                using var clienteSmtp = new SmtpClient(EncodeDecodeHelper.GetDecodeValue(_mailConfig.ServidorSmtp))
                {
                    Port = Convert.ToInt32(EncodeDecodeHelper.GetDecodeValue(_mailConfig.PuertoSmtp)),
                    Credentials = new NetworkCredential(EncodeDecodeHelper.GetDecodeValue(_mailConfig.UsuarioSmtp),
                                                        EncodeDecodeHelper.GetDecodeValue(_mailConfig.ContrasenaSmtp)),
                    EnableSsl = true
                };

                clienteSmtp.SendAsync(mensaje, guidRelated.ToString());

                if (hasAttachment)
                {
                    foreach (var adjunto in mensaje.Attachments)
                        adjunto.ContentStream.Dispose();
                }
            }
            return guidRelated;
        }

        /// <summary>
        /// Envia un correo electronico con los datos especificados por parámetro
        /// </summary>
        /// <param name="correo"> Objeto con los detalles del correo a enviar </param>
        /// <returns> Devuelve el EmailToken asociado al correo enviado </returns>
        public Guid? EnviarCorreo(Correo correo)
        {
            using (var mensaje = new MailMessage())
            {
                mensaje.From = new MailAddress(EncodeDecodeHelper.GetDecodeValue(_mailConfig.UsuarioSmtp));
                mensaje.To.Add(correo.Destinatario);
                mensaje.Subject = correo.Asunto;
                mensaje.Body = correo.Cuerpo;
                mensaje.IsBodyHtml = true;

                bool hasAttachment = correo.FicheroAdjunto != null && correo.FicheroAdjunto.Archivo.Length > 0;

                if (hasAttachment)
                {
                    var stream = new MemoryStream(correo.FicheroAdjunto.Archivo);
                    var attachment = new Attachment(stream, correo.FicheroAdjunto.NombreArchivo, correo.FicheroAdjunto.ContentType);
                    mensaje.Attachments.Add(attachment);
                }

                using var clienteSmtp = new SmtpClient(EncodeDecodeHelper.GetDecodeValue(_mailConfig.ServidorSmtp)) 
                {
                    Port = Convert.ToInt32(EncodeDecodeHelper.GetDecodeValue(_mailConfig.PuertoSmtp)),
                    Credentials = new NetworkCredential(EncodeDecodeHelper.GetDecodeValue(_mailConfig.UsuarioSmtp),
                                                        EncodeDecodeHelper.GetDecodeValue(_mailConfig.ContrasenaSmtp)),
                    EnableSsl = true
                };
                clienteSmtp.Send(mensaje);

                if (hasAttachment) {
                    foreach (var adjunto in mensaje.Attachments)
                        adjunto.ContentStream.Dispose();
                }
            }
            return correo.EmailToken;
        }
    }
}