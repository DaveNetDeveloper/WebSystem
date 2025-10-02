namespace Domain.Entities
{
    public class TipoEnvioCorreo
    { 
        public Guid id { get; set; } //PK
        public string nombre { get; set; }  
        public string? descripcion { get; set; }
        public bool? activo { get; set; }
        public string? asunto { get; set; }
        public string? cuerpo { get; set; }

        public static class TipoEnvio
        {
            public const string ContrasenaCambiada = "ContrasenaCambiada";
            public const string RecordatorioSuscripcion = "RecordatorioSuscripcion";
            public const string SuscripcionActivada = "SuscripcionActivada";
            public const string ReservaProducto = "ReservaProducto";
            public const string Bienvenida = "Bienvenida";
            public const string InscripcionActividad = "InscripcionActividad";
            public const string EnvioComunicacion = "EnvioComunicacion";
            public const string ValidacionCuenta = "ValidacionCuenta";
            public const string CambiarContrasena = "CambiarContrasena";
            public const string Recompensa = "Recompensa";
        }

        public enum TipoEnvioCorreos
        {
            ValidacionCuenta = 1,
            Bienvenida = 2,
            SuscripcionActivada = 3,
            ResetContrasena = 4,
            ContrasenaCambiada = 5,
            ReservaProducto = 6,
            InscripcionActividad = 7,
            RememberSubscribe = 8,
            Recompensa = 9,
            Undefined = 10
        }
    }
}