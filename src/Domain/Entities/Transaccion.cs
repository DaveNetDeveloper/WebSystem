namespace Domain.Entities
{
    public class Transaccion
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string? nombre { get; set; }
        public int? idProducto { get; set; }
        public DateTime? fecha { get; set; }
        public int puntos { get; set; }
        public Guid idTipoTransaccion { get; set; }

        //public static class TipoTransaccion
        //{
        //    public const string PuntosBienvenida = "Puntos de bienvenida";
        //    public const string CompletarPerfil = "Puntos por completar el perfil";
        //    public const string QrProduto = "Puntos por escanear un QR";
        //    public const string ReservarEvento = "Puntos por asistencia a evento";
        //} 
    }
}