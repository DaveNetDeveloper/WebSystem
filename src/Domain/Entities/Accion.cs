namespace Domain.Entities
{
    public class Accion
    {
        public Guid id { get; set; } 
        public string nombre { get; set; }
        public string? descripcion { get; set; }
        public bool activo { get; set; }
        public string tipoAccion { get; set; } 
    }
}