namespace Domain.Entities
{
    public class FAQ
    {
        public Guid id { get; set; }
        public int orden { get; set; }
        public string pregunta { get; set; }
        public string? respuesta { get; set; }
        public int idEntidad { get; set; } //FK 
    }
}