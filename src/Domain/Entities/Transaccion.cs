namespace Domain.Entities
{
    public class Transaccion
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string? nombre { get; set; }
        public int? idProducto { get; set; }
        public int? idActividad { get; set; }
        public DateTime? fecha { get; set; }
        public int? puntos { get; set; }
        public Guid idTipoTransaccion { get; set; } 
    }
}