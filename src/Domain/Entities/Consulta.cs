namespace Domain.Entities
{
    public class Consulta
    {
        public Guid id { get; set; } // PK
        public string nombreCompleto { get; set; }
        public string email { get; set; }
        public string? telefono { get; set; }
        public string? asunto { get; set; }
        public string mensaje { get; set; }
        public DateTime fecha { get; set; }
        public Guid? idMotivoConsulta { get; set; } //FK 
        public int? idEntidad { get; set; } //FK 
    }
}