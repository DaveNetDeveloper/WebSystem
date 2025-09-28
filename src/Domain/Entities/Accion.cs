using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Accion
    {
        public Guid id { get; set; }
        public string nombre { get; set; }
        public string? descripcion { get; set; }
        public bool activo { get; set; }
        public string tipoAccion { get; set; }

        [JsonIgnore]
        public ICollection<CampanaAcciones> CampanaAcciones { get; set; }

        public static class NombreAccion
        {
            public const string EnvioEmail = "EnvioEmail";
            public const string EnvioSMS = "EnvioSMS";
            public const string EnvioPush = "EnvioPush";
            public const string EnvioInApp = "EnvioInApp";

            public const string Activacion = "Activación";

            public const string GenerarRecompensa = "GenerarRecompensa";
        }

        public static class TipoAccion
        {
            public const string EnvioComunicacion = "EnvioComunicacion";
            public const string CrearContenido = "CrearContenido";
            public const string ManipulacionDatos = "ManipulacionDatos"; 
        }

        public class AccionDetalle
        {
            public string titulo { get; set; }
            public string contenido { get; set; } 
            public string plantilla { get; set; }
            public Dictionary<string, string> parametros { get; set; }
        }
    }
}