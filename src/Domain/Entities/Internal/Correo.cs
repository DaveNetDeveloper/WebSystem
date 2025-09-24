using System.Text;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public class Correo
    {
        public string? Destinatario { get; set; }
        public string? Asunto { get; set; }
        public string? Cuerpo { get; set; } 
        public string? NombreUsuario { get; set; }
        public Guid? EmailToken { get; set; }
        public TipoEnvioCorreo.TipoEnvioCorreos TipoEnvio { get; set; }
        public string? LogoUrl { get; set; }
        
        public Correo(TipoEnvioCorreo tipoEnvio, string destinatario, string nombreUsuario, string logoUrl)
        {
            this.TipoEnvio = GetTipoEnvioCorreo(tipoEnvio.nombre);
            this.NombreUsuario = nombreUsuario;
            this.Destinatario = destinatario;
            this.EmailToken = Guid.NewGuid();
            this.LogoUrl = logoUrl; // "https://www.getautismactive.com/wp-content/uploads/2021/01/Test-Logo-Circle-black-transparent.png"; 
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
            customizedBody = customizedBody.Replace(EmailKeys.EMAIL_KEY_TOKEN, this.EmailToken.ToString());

            return customizedBody;
        }
        private static class EmailKeys
        {
            public const string EMAIL_KEY_NAME =  "[_NAME_]";
            public const string EMAIL_KEY_LOGO =  "[_LOGO_]";
            public const string EMAIL_KEY_TOKEN = "[_TOKEN_]";
            public const string EMAIL_KEY_EMAIL = "[_EMAIL_]"; 
        }

        private TipoEnvioCorreo.TipoEnvioCorreos GetTipoEnvioCorreo(string nombreTipoEnvio)
        {
            TipoEnvioCorreo.TipoEnvioCorreos tipoEnvio = TipoEnvioCorreos.Undefined;
            switch (nombreTipoEnvio)
            {
                case "ValidaciónCuenta":
                    tipoEnvio = TipoEnvioCorreos.ValidaciónCuenta;
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
                case "SuscripciónActivada":
                    tipoEnvio = TipoEnvioCorreos.SuscripciónActivada;
                    break;
                case "ReservaProducto":
                    tipoEnvio = TipoEnvioCorreos.ReservaProducto;
                    break;
                case "InscripcionActividad":
                    tipoEnvio = TipoEnvioCorreos.InscripcionActividad;
                    break;
            }
            return tipoEnvio;
        }

    }
}