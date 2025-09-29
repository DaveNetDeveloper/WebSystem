using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Accion
    {
        public Guid id { get; set; }

        /// <summary>   </summary>
        public string nombre { get; set; }

        /// <summary>   </summary>
        public string? descripcion { get; set; }

        /// <summary>   </summary>
        public bool activo { get; set; }

        /// <summary>   </summary>
        public string tipoAccion { get; set; }

        [JsonIgnore]
        public ICollection<CampanaAcciones> CampanaAcciones { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static class NombreAccion
        {
            public const string EnvioEmail = "EnvioEmail";
            public const string EnvioSMS = "EnvioSMS";
            public const string EnvioPush = "EnvioPush";
            public const string EnvioInApp = "EnvioInApp";

            public const string Activacion = "Activación";

            public const string GenerarRecompensa = "GenerarRecompensa";
        }

        /// <summary>
        /// 
        /// </summary>
        public static class TipoAccion
        {
            public const string EnvioComunicacion = "EnvioComunicacion";
            public const string CrearContenido = "CrearContenido";
            public const string ManipulacionDatos = "ManipulacionDatos"; 
        }

        /// <summary>
        /// 
        /// </summary>
        public class AccionDetalle
        {
            public string titulo { get; set; }
            public string contenido { get; set; } 
            public string plantilla { get; set; }
            public Dictionary<string, string> parametros { get; set; }
        }
    }
}