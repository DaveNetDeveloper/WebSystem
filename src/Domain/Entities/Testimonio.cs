namespace Domain.Entities
{
    public class Testimonio
    {
        public int id { get; set; }
        public string texto { get; set; }
        public string nombreUsuario { get; set; }
        public DateTime fecha { get; set; }
        public string imagen { get; set; }
    }
}