namespace Domain.Entities
{
    public class Campana
    {
        public int id { get; set; }
        public Guid idTipoCampana { get; set; }
        public string nombre { get; set; }
        public bool activo { get; set; }
        public string? descripcion { get; set; }
        public string frecuencia { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
    }
}