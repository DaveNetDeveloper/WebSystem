using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.Sms;

namespace Domain.Entities
{
    /// <summary> Entidad interna para un envio de SMS </summary>
    public class Sms
    {
        /// <summary> Telefono del destinatario <summary>
        public string? Destinatario { get; set; }

        /// <summary> Emisor del SMS  </summary>
        public string? Emisor { get; set; }

        /// <summary> Asunto del SMS  </summary>
        public string? Asunto { get; set; }

        /// <summary> Cuerpo del SMS </summary>
        public string? Mensaje { get; set; }

        /// <summary> Nombre del usuario </summary>
        public string? NombreUsuario { get; set; }

        /// <summary> Tipo de envío SMS </summary>
        public TipoEnvioSms TipoEnvio { get; set; }

        /// <summary> Constructor sin parámetros </summary>
        [JsonConstructor]
        public Sms() { }

        /// <summary> Constructor con parámetros </summary>
        /// <param name="tipoEnvio"> Tipo de envio del sms </param>
        /// <param name="destinatario">  </param>
        /// <param name="nombreUsuario">  </param>
        /// <param name="emisor"> </param>
        /// <param name="asunto"> </param>
        /// <param name="mensaje"> </param>
        public Sms(string tipoEnvio, 
                   string destinatario, 
                   string nombreUsuario, 
                   string emisor, 
                   string asunto, 
                   string mensaje)  {
            
            this.TipoEnvio = GetTipoEnvioSms(tipoEnvio);
            this.NombreUsuario = nombreUsuario;
            this.Destinatario = destinatario;
            this.Emisor = emisor; 
            this.Asunto = asunto;
            this.Mensaje = this.ApplyTagsForMessage(mensaje);
        }

        private string ApplyTagsForMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return message;

            string customizedMessage = message;

            customizedMessage = customizedMessage.Replace(SmsKeys.SMS_KEY_EMISOR, this.Emisor);
            customizedMessage = customizedMessage.Replace(SmsKeys.SMS_KEY_NAME, this.NombreUsuario); 

            return customizedMessage;
        }

        public enum TipoEnvioSms
        {
            Undefined = 0,
            Tipo1 = 1,
            Tipo2 = 2
        }

        private static class SmsKeys
        {
            public const string SMS_KEY_NAME =  "[_NAME_]"; 
            public const string SMS_KEY_EMISOR = "[_EMISOR_]"; 
        }

        private TipoEnvioSms GetTipoEnvioSms(string nombreTipoEnvioSms)
        {
            TipoEnvioSms tipoEnvio = TipoEnvioSms.Undefined;
            switch (nombreTipoEnvioSms)
            {
                case "Tipo1":
                    tipoEnvio = TipoEnvioSms.Tipo1;
                    break;
                case "Tipo2":
                    tipoEnvio = TipoEnvioSms.Tipo2;
                    break;
            }
            return tipoEnvio;
        }
    }
}