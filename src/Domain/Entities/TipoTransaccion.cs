namespace Domain.Entities
{
    public class TipoTransaccion
    {
        public Guid id { get; set; }
        public string nombre { get; set; }
        public string? descripcion { get; set; }
    }
}