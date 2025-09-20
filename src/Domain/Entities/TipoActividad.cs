namespace Domain.Entities
{
    public class TipoActividad
    {
        public Guid id { get; set; }
        public string nombre { get; set; }
        public string? descripcion { get; set; }
    }
}