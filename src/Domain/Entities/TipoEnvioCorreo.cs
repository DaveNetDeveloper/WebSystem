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
            public const string ReservaActividad = "ReservaActividad";
            public const string EnvioComunicacion = "EnvioComunicacion";
            public const string ValidacionCuenta = "ValidacionCuenta";
            public const string CambiarContrasena = "CambiarContrasena";
            public const string Recompensa = "Recompensa";
            public const string EnvioReport = "EnvioReport";
            public const string Undefined = "Undefined";
            
        }

        public enum TipoEnvioCorreos
        {
            ValidacionCuenta,
            Bienvenida,
            SuscripcionActivada,
            ResetContrasena ,
            ContrasenaCambiada,
            ReservaProducto,
            ReservaActividad ,
            RememberSubscribe,
            Recompensa,
            EnvioReport,
            Undefined
        }
    }
}