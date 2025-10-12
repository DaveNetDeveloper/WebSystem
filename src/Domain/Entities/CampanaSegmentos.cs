namespace Domain.Entities
{
    public class CampanaSegmentos
    {
        public int idCampana { get; set; }  // PK
        public int idSegmento { get; set; } //PK
        public DateTime fecha { get; set; }

        public Campana Campana { get; set; }
        public Segmento Segmento { get; set; }  
    }
}