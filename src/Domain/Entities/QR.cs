namespace Domain.Entities
{
    public class QR
    {
        public Guid id { get; set; }
        public int idProducto { get; set; }
        public bool activo { get; set; }
        public bool? multicliente { get; set; }
        public string qrCode { get; set; }
        public bool consumido { get; set; }
        public DateTime? fechaExpiracion { get; set; }
    }
}