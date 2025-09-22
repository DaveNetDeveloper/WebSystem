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

        public Guid EnviarCorreo(Correo correo, string nombreUsuario, string servidorSmtp, string puertoSmtp, string usuarioSmtp, string contraseñaSmtp)
        {
            var emailToken = Guid.NewGuid();

            var contenido = ConstruirCuerpoHTML(correo.TipoEnvio, nombreUsuario, correo.Destinatario, emailToken.ToString());
            
            if(null == contenido) {
                contenido =  new Correo {
                    Asunto = correo.Asunto,
                    Cuerpo = correo.Cuerpo
                }; 
            }

            using (var mensaje = new MailMessage()) {
                mensaje.From = new MailAddress(usuarioSmtp);
                mensaje.To.Add(correo.Destinatario);
                mensaje.Subject = contenido.Asunto;
                mensaje.Body = contenido.Cuerpo;
                mensaje.IsBodyHtml = true;

                using (var clienteSmtp = new SmtpClient(servidorSmtp, Convert.ToInt32(puertoSmtp))) { 
                    clienteSmtp.Credentials = new NetworkCredential(usuarioSmtp, contraseñaSmtp);
                    clienteSmtp.EnableSsl = true;
                    clienteSmtp.Send(mensaje);
                }
            }
            return emailToken;
        }
         
        private Correo ConstruirCuerpoHTML(TipoEnvioCorreos tipoEnvio, string nombreUsuario, string email, string emailToken)
        {
            string asunto = string.Empty;
            string logoUrl = "https://www.getautismactive.com/wp-content/uploads/2021/01/Test-Logo-Circle-black-transparent.png";

            StringBuilder body = new();
            switch (tipoEnvio)
            {
                case TipoEnvioCorreos.ValidaciónCuenta:
                    asunto = "Valida tu nueva cuenta";
                    body = BuildBodyValidateAccount(logoUrl, nombreUsuario, email, emailToken);
                    break;
                case TipoEnvioCorreos.Bienvenida:
                    asunto = "Bienvenidx a nuestra aplicación";
                    body = BuildBodyWelcome(logoUrl, nombreUsuario, email, emailToken);
                    break;
               
                case TipoEnvioCorreos.ContraseñaCambiada: 
                    asunto = "Tu contraseña ha cambiado";
                    body = BuildBodyPasswordChanged(logoUrl, nombreUsuario, email, emailToken);
                    break;
                case TipoEnvioCorreos.RememberSubscribe:
                    asunto = "Apuntate a nuestra Newsletter para estar a la última";
                    body = BuildBodyRememberSubscribe(logoUrl, nombreUsuario, email, emailToken);
                    break;
                case TipoEnvioCorreos.ResetContraseña:
                    return null;
                    break; 
                case TipoEnvioCorreos.SuscripciónActivada:
                        return null;
                    break;
                case TipoEnvioCorreos.ReservaProducto:
                    return null;
                    break;
                case TipoEnvioCorreos.InscripcionActividad:
                    return null;
                    break;
            } 
            return new Correo { Asunto = asunto, 
                                Cuerpo = body.ToString() };
        }

        private StringBuilder BuildBodyPasswordChanged(string logo, string nombreUsuario, string email, string emailToken)
        {
            StringBuilder body = new();

            body.AppendLine("<html>");
            body.AppendLine("<head>");
            body.AppendLine("<style>");
            body.AppendLine("/* Estilos CSS */");
            body.AppendLine("</style>");
            body.AppendLine("</head>");
            body.AppendLine("<body>");
            body.AppendLine("<div id='header'>");
            body.AppendLine("<img width='100px' src='" + logo + "' alt='Logo' />");
            body.AppendLine("<h1>Contraseña modificada</h1>");
            body.AppendLine("</div>");
            body.AppendLine("<div id='cuerpo'>");
            body.AppendLine("<p>Hola " + nombreUsuario + ",</p>");
            body.AppendLine("<p>La contraseña de tu cuenta se ha cambiado correctamente.</p>");
            body.AppendLine("<p>Haz clic en el siguiente botón para ir al inicio de sesión:</p>");
            body.AppendLine("<a href='https://localhost:7175/WebPages/login.html?email=" + email + "'>");
            body.AppendLine("<button type='button'>INICIAR SESIÓN</button>");
            body.AppendLine("</a>");
            body.AppendLine("</div>");
            body.AppendLine("</body>");
            body.AppendLine("</html>");

            return body;
        }

        private StringBuilder BuildBodyValidateAccount(string logo, string nombreUsuario, string email, string emailToken)
        {
            StringBuilder body = new();

            body.AppendLine("<html>");
            body.AppendLine("<head>");
            body.AppendLine("<style>");
            body.AppendLine("/* Estilos CSS */");
            body.AppendLine("</style>");
            body.AppendLine("</head>");
            body.AppendLine("<body>");
            body.AppendLine("<div id='header'>");
            body.AppendLine("<img width='100px' src='" + logo + "' alt='Logo' />");
            body.AppendLine("<h1>Activa tu cuenta</h1>");
            body.AppendLine("</div>");
            body.AppendLine("<div id='cuerpo'>");
            body.AppendLine("<p>Hola " + nombreUsuario + ",</p>");
            body.AppendLine("<p>Gracias por unirte a nuestra aplicación.</p>");
            body.AppendLine("<p>Haz clic en el siguiente botón para activar tu cuenta:</p>");
            body.AppendLine("<a href='https://localhost:7175/WebPages/validateAccount.html?email=" + email + "'>");
            body.AppendLine("<button type='button'>Activar Cuenta</button>");
            body.AppendLine("</a>");
            body.AppendLine("</div>");
            body.AppendLine("</body>");
            body.AppendLine("</html>");

            return body;
        }

        private StringBuilder BuildBodyWelcome(string logo, string nombreUsuario, string email, string emailToken)
        {
            StringBuilder body = new();

            body.AppendLine("<html>");
            body.AppendLine("<head>");
            body.AppendLine("<style>");
            body.AppendLine("/* Estilos CSS */");
            body.AppendLine("</style>");
            body.AppendLine("</head>");
            body.AppendLine("<body>");
            body.AppendLine("<div id='header'>");
            body.AppendLine("<img width='100px' src='" + logo + "' alt='Logo' />");
            body.AppendLine("<h1>Bienvenido a nuestra plataforma</h1>");
            body.AppendLine("</div>");
            body.AppendLine("<div id='cuerpo'>");
            body.AppendLine("<p>Hola " + nombreUsuario + ",</p>");
            body.AppendLine("<p>Gracias por unirte a nuestra aplicación.</p>");
            body.AppendLine("<p>Inicia sesión para explorar tus beneficios como vecinx.</p>");
            body.AppendLine("<a href='https://localhost:7175/WebPages/index.html'>");
            body.AppendLine("<button type='button'>Ir a la platforma</button>");
            body.AppendLine("</a>");
            body.AppendLine("</div>");
            body.AppendLine("</body>");
            body.AppendLine("</html>");

            return body;
        }

        private StringBuilder BuildBodyRememberSubscribe(string logo, string nombreUsuario, string email, string emailToken)
        {
            StringBuilder body = new();
            
            body.AppendLine("<html>");
            body.AppendLine("<head>");
                body.AppendLine("<style>");
                    body.AppendLine("/* Estilos CSS */");
                body.AppendLine("</style>");
            body.AppendLine("</head>");
            body.AppendLine("<body>");
            body.AppendLine("<div id='header'>");
                body.AppendLine("<img width='100px' src='" + logo + "' alt='Logo' />");
            //body.AppendLine("<h1>Bienvenido a nuestra plataforma</h1>");
            body.AppendLine("</div>");
            body.AppendLine("<div id='cuerpo'>");
                body.AppendLine("<p>Hola " + nombreUsuario + ",</p>");
                body.AppendLine("<p>Vemos que aún no formas parte de las personas que reciben las últimas noticias sobre todos los locales y asociaciones de tu entorno!.</p>");
                body.AppendLine("<p>Inicia sesión para apuntarte a la Newsletter.</p>");
               // body.AppendLine("<a href='https://localhost:7175/WebPages/index.html'>");
               //     body.AppendLine("<button type='button'>Ir a la platforma</button>");

            //var token = Guid.NewGuid().ToString("N");  

            // URL de confirmación
            var linkConfirmacion = $"https://localhost:7175/ConfirmarNewsletter?token={emailToken}&email={email}";

            // Botón como enlace estilizado
            body.AppendLine($@"    <a href='{linkConfirmacion}' 
                                   style='display:inline-block;
                                          padding:10px 20px;
                                          font-size:16px;
                                          font-weight:bold;
                                          color:#ffffff;
                                          background-color:#007BFF;
                                          text-decoration:none;
                                          border-radius:5px;'>
                                    Activar Suscripción a la Newsletter
                                    </a> ");
            //body.AppendLine("</a>");
            body.AppendLine("</div>");
            body.AppendLine("</body>");
            body.AppendLine("</html>");

            return body;
        }
    }
}