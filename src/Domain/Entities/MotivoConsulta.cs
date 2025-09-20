namespace Domain.Entities
{
    public class MotivoConsulta
    {
        public Guid id { get; set; } // PK
        public string nombre { get; set; }
        public string? descripcion { get; set; }
        public Guid idtipoentidad { get; set; }  
    }
}