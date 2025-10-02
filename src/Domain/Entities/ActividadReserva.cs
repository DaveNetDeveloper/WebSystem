using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class ActividadReserva
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid idReserva { get; set; } // PK

        /// <summary>
        /// 
        /// </summary>
        public string codigoReserva { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int idActividad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int idUsuario { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime fechaReserva { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? fechaActividad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string estado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? fechaValidacion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public ICollection<Usuario> Usuario { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public ICollection<Usuario> Actividad { get; set; }

        public static class EstadoReeserva
        {
            public const string Reservada = "Reservada";
            public const string Validada = "Validada";
            public const string Pendiente = "Pendiente";
            public const string Error = "Error";
        }

    }
}