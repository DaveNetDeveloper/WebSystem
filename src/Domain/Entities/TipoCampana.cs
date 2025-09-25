namespace Domain.Entities
{
    public class TipoCampana
    {
        public Guid id { get; set; }
        public string nombre { get; set; }
        public string? descripcion { get; set; }
        public string? objetivo { get; set; }
        public bool activo { get; set; }
    }
}