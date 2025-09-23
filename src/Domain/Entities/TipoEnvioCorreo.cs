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

        public enum TipoEnvioCorreos
        {
            ValidaciónCuenta,
            Bienvenida,
            SuscripciónActivada,
            ResetContrasena,
            ContrasenaCambiada,
            ReservaProducto,
            InscripcionActividad,
            RememberSubscribe,
            Undefined
        }
    }
}