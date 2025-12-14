namespace Domain.Entities
{
    public enum QrStatus
    {
        Inactive = 0,
        Active = 1,
        Consumed = 2
    }

    public class QRCode
    {
        public Guid id { get; set; } = Guid.NewGuid(); // PK
        public string token { get; set; } = Guid.NewGuid().ToString("N");
        public string payload { get; set; } = default!;
        public QrStatus status { get; set; } = QrStatus.Active;
        public DateTime fechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? fechaExpiracion { get; set; }
        public byte[]? imagen { get; set; }
        public int idEntidad { get; set; } // FK
        public string origen { get; set; }
        public int? idProducto { get; set; } // FK
        public int? idActividad { get; set; }  // FK

        public bool IsExpired => fechaExpiracion.HasValue && DateTime.UtcNow > fechaExpiracion.Value;

        public QRCode() { } 

        public QRCode(string _payload, TimeSpan? _ttl, byte[] _imagen, Guid? id)
        {
            payload = _payload;
            imagen = _imagen;
            if (_ttl.HasValue) { 
                fechaExpiracion = DateTime.UtcNow.Add(_ttl.Value);
            }

            if (id.HasValue) {
                this.id = id.Value;
            }
        }

        public void Activate()
        {
            if (IsExpired)
                throw new InvalidOperationException("QR expirado, no puede activarse.");
            if (status == QrStatus.Consumed)
                throw new InvalidOperationException("QR ya consumido, no puede reactivarse.");
            status = QrStatus.Active;
        }

        public void Deactivate()
        {
            if (IsExpired)
                throw new InvalidOperationException("QR expirado, no puede desactivarse.");
            if (status == QrStatus.Consumed)
                throw new InvalidOperationException("QR ya consumido, no puede modificarse.");
            status = QrStatus.Inactive;
        }

        public void Consume()
        {
            if (IsExpired)
                throw new InvalidOperationException("QR expirado, no puede consumirse.");
            if (status != QrStatus.Active)
                throw new InvalidOperationException("Solo los QR activos pueden consumirse.");
            status = QrStatus.Consumed;
        }

        public static class Origen
        {
            public const string Producto = "producto";
            public const string Actividad = "actividad"; 
        }  

    }
}