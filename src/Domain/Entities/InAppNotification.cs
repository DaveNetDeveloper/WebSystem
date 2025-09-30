namespace Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class InAppNotification
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int idUsuario { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string tipoEnvioInApp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string titulo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string mensaje { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool activo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime fechaCreacion { get; set; }

        /// <summary>
        /// Enumeración para los tipos de notificación InApp
        /// </summary>
        public static class TipoEnvioInApp
        { 
            public const string Novedades = "Novedades";
            public const string Bienvenida = "Bienvenida";
            public const string NuevaRecompensa = "NuevaRecompensa";
            public const string Recomendacion = "Recomendacion";
            public const string PrimeraCompra = "PrimeraCompra";
            public const string VolverAVerte = "VolverAVerte";
            public const string TerminosyCondiciones = "TerminosyCondiciones"; 
        }
    }
}