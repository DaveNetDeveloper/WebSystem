namespace Domain.Entities
{
    public class Actividad
    {
        public int id { get; set; }
        public int idEntidad { get; set; }
        public string nombre { get; set; }
        public string? imagen { get; set; }
        public string descripcion { get; set; }
        public string linkEvento { get; set; }
        public bool activo { get; set; }
        public Guid idTipoActividad { get; set; }
        public string ubicación { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public int? popularidad { get; set; }
        public string? descripcionCorta { get; set; }
        public bool? gratis { get; set; }
        public string? informacioExtra { get; set; }
        public string? linkInstagram { get; set; }
        public string? linkFacebook { get; set; }
        public string? linkYoutube { get; set; } 
    }
}