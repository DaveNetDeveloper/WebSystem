namespace Domain.Entities
{
    public class TipoEnvioCorreo
    { 
        public Guid id { get; set; } //PK
        public string nombre { get; set; }  
        public string? descripcion { get; set; }
        public bool? activo { get; set; } 
    }
}