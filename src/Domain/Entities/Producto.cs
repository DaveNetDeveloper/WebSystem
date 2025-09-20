namespace Domain.Entities
{
    public class Producto 
    {
        public int id { get; set; }
        public int idEntidad { get; set; }
        public string nombre { get; set; }
        public string? imagen { get; set; }
        public string descripcion { get; set; }
        public int puntos { get; set; }
        public bool activo { get; set; }
        public float? precio { get; set; }
        public int? descuento { get; set; }
        public int? popularidad { get; set; }
        public string? descripcionCorta { get; set; }
        public bool? disponible { get; set; }
        public string? informacioExtra { get; set; }
        public string? linkInstagram { get; set; }
        public string? linkFacebook { get; set; }
        public string? linkYoutube { get; set; } 
    }
}