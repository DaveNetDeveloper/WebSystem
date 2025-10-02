namespace Domain.Entities
{
    public class TipoTransaccion
    {
        public Guid id { get; set; }
        public string nombre { get; set; }
        public string? descripcion { get; set; }
        public int? puntos { get; set; }

        public static class TiposTransaccion
        {
            public const string PuntosBienvenida = "Puntos de bienvenida";
            public const string CompletarPerfil = "Puntos por completar el perfil";
            public const string QrProduto = "Puntos por escanear un QR";
            public const string ReservarEvento = "Puntos por asistencia a evento";
            public const string PuntosRecomendacion= "Puntos por recomendacion";
        }
    }
}