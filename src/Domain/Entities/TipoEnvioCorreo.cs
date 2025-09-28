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
            ValidacionCuenta = 1,
            Bienvenida = 2,
            SuscripcionActivada = 3,
            ResetContrasena = 4,
            ContrasenaCambiada = 5,
            ReservaProducto = 6,
            InscripcionActividad = 7,
            RememberSubscribe = 8,
            Undefined = 9
        }
    }
}