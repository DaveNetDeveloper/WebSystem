namespace Domain.Entities
{
    public class Correo
    {
        public string? Destinatario { get; set; }
        public string? Asunto { get; set; }
        public string? Cuerpo { get; set; }
        public TipoEnvioCorreos TipoEnvio { get; set; }
    }

    public enum TipoEnvioCorreos
    {
        ValidaciónCuenta,
        Bienvenida, 
        SuscripciónActivada,
        ResetContraseña,
        ContraseñaCambiada,
        ReservaProducto,
        InscripcionActividad,
        RememberSubscribe
    }
}
