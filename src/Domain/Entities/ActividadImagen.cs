namespace Domain.Entities
{
    public class ActividadImagen
    {
        public int id { get; set; } // PK
        public string imagen { get; set; }
        public int idactividad { get; set; }


        public Actividad Actividad { get; set; }
    }
}