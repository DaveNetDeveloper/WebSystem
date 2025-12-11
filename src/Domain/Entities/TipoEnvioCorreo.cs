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
            public const string ValidacionCuenta = "ValidacionCuenta";
            public const string Bienvenida = "Bienvenida";
            public const string RecordatorioSuscripcion = "RecordatorioSuscripcion";
            public const string SuscripcionActivada = "SuscripcionActivada";
            public const string ReservaProducto = "ReservaProducto";
            public const string ReservaActividad = "ReservaActividad";
            public const string ReservaActividad_Manager = "ReservaActividad_Manager";
            public const string EnvioComunicacion = "EnvioComunicacion";
            public const string CambiarContrasena = "CambiarContrasena";
            public const string ContrasenaCambiada = "ContrasenaCambiada";
            public const string Recompensa = "Recompensa";
            public const string EnvioReport = "EnvioReport";
            public const string Undefined = "Undefined";
            public const string ConsultaUsuario_Manager = "ConsultaUsuario_Manager";
            
        }

        public enum TipoEnvioCorreos
        {
            ValidacionCuenta,
            Bienvenida,
            SuscripcionActivada,
            RecordatorioSuscripcion,
            ResetContrasena,
            CambiarContrasena,
            ContrasenaCambiada,
            ReservaProducto,
            ReservaActividad ,
            RememberSubscribe,
            Recompensa,
            EnvioReport,
            ReservaActividad_Manager,
            ConsultaUsuario_Manager,
            EnvioComunicacion,
            Undefined
        }
    }
}