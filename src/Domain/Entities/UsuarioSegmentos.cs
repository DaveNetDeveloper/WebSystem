namespace Domain.Entities
{
    public class UsuarioSegmentos
    {
        public int idUsuario { get; set; } //PK
        public int idSegmento { get; set; }  // PK
        public DateTime fecha { get; set; }

        public Usuario Usuario { get; set; }
        public Segmento Segmento { get; set; }  
    }
}