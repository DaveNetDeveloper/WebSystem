using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    /// <summary> Entidad interna con datos del correo electrónico </summary>
    public class Correo
    {
        /// <summary> Email del destinatario <summary>
        public string? Destinatario { get; set; }

        /// <summary> Asunto del correo  </summary>
        public string? Asunto { get; set; }

        /// <summary> Cuerpo del correo </summary>
        public string? Cuerpo { get; set; }

        /// <summary> Nombre del usuario </summary>
        public string? NombreUsuario { get; set; }

        /// <summary> EmailToken asocadio al correo </summary>
        public Guid? EmailToken { get; set; }

        /// <summary> Tipo de envío del correo </summary>
        public TipoEnvioCorreos TipoEnvio { get; set; }

        /// <summary> Url de la imagen del logo mostrado en el correo </summary>
        public string? LogoUrl { get; set; }

        public FicheroAdjunto? FicheroAdjunto { get; set; }

        /// <summary> Constructor sin parámetros </summary>
        [JsonConstructor]
        public Correo() { }

        /// <summary> Constructor con parámetros </summary>
        /// <param name="tipoEnvio"> Tipo de envio del correo </param>
        /// <param name="destinatario"> Email del destinatario del correo </param>
        /// <param name="nombreUsuario"> Nombre del usuario del destinatario del correo </param>
        /// <param name="logoUrl"> Url del logo del correo </param>
        public Correo(TipoEnvioCorreo tipoEnvio, string destinatario, string nombreUsuario, string logoUrl, Guid? emailToken = null)
        {
            this.TipoEnvio = GetTipoEnvioCorreo(tipoEnvio.nombre);
            this.NombreUsuario = nombreUsuario;
            this.Destinatario = destinatario;
            this.EmailToken = emailToken;
            this.LogoUrl = logoUrl;
            this.Asunto = tipoEnvio.asunto;
            this.Cuerpo = this.ApplyTagsForBody(tipoEnvio.cuerpo);
        }

        private string ApplyTagsForBody(string cuerpo)
        {
            if (string.IsNullOrEmpty(cuerpo))
                return cuerpo;

            string customizedBody = cuerpo;

            customizedBody = customizedBody.Replace(EmailKeys.EMAIL_KEY_LOGO, this.LogoUrl);
            customizedBody = customizedBody.Replace(EmailKeys.EMAIL_KEY_NAME, this.NombreUsuario);
            customizedBody = customizedBody.Replace(EmailKeys.EMAIL_KEY_EMAIL, this.Destinatario);

            if(this.EmailToken.HasValue) customizedBody = customizedBody.Replace(EmailKeys.EMAIL_KEY_TOKEN, this.EmailToken.Value.ToString());

            return customizedBody;
        }

        private static class EmailKeys
        {
            public const string EMAIL_KEY_NAME =  "[_NAME_]";
            public const string EMAIL_KEY_LOGO =  "[_LOGO_]";
            public const string EMAIL_KEY_TOKEN = "[_TOKEN_]";
            public const string EMAIL_KEY_EMAIL = "[_EMAIL_]";
        }

        private TipoEnvioCorreos GetTipoEnvioCorreo(string nombreTipoEnvio)
        {
            TipoEnvioCorreos tipoEnvio = TipoEnvioCorreos.Undefined;
            switch (nombreTipoEnvio)
            {
                case "ValidacionCuenta":
                    tipoEnvio = TipoEnvioCorreos.ValidacionCuenta;
                    break;
                case "Bienvenida":
                    tipoEnvio = TipoEnvioCorreos.Bienvenida;
                    break;
                case "ContrasenaCambiada":
                    tipoEnvio = TipoEnvioCorreos.ContrasenaCambiada;
                    break;
                case "RememberSubscribe":
                    tipoEnvio = TipoEnvioCorreos.RememberSubscribe;
                    break;
                case "ResetContrasena":
                    tipoEnvio = TipoEnvioCorreos.ResetContrasena;
                    break;
                case "SuscripcionActivada":
                    tipoEnvio = TipoEnvioCorreos.SuscripcionActivada;
                    break;
                case "EscanearProducto":
                    tipoEnvio = TipoEnvioCorreos.EscanearProducto;
                    break;
                case "ReservaActividad":
                    tipoEnvio = TipoEnvioCorreos.ReservaActividad;
                    break;
                case "EnvioReport":
                    tipoEnvio = TipoEnvioCorreos.EnvioReport;
                    break;
            }
            return tipoEnvio;
        }
    }
}