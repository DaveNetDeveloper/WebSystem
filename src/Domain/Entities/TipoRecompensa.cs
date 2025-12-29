namespace Domain.Entities
{
    public class TipoRecompensa
    {
        public Guid id { get; set; }
        public string nombre { get; set; }
        public string? descripcion { get; set; }
        public bool activo { get; set; }

        public static class TiposRecompensa
        {
            public const string Bienvenida = "Bienvenida";
            public const string CompletarPerfil = "CompletarPerfil";
            public const string Recomendacion = "Recomendacion";
            public const string AsistenciaEvento = "AsistenciaEvento";
            public const string EscanearQR = "EscanearQR";
        }
    }
}