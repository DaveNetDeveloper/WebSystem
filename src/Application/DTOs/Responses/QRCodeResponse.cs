using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests
{
    public record QRCodeResponse(QRCode qr)
    {
        public Guid Id { get; init; } = qr.id;
        public string Token { get; init; } = qr.token;
        public string Payload { get; init; } = qr.payload;
        public string Status { get; init; } = qr.status.ToString();
        public DateTime CreatedAt { get; init; } = qr.fechaCreacion;
        public DateTime? ExpiresAt { get; init; } = qr.fechaExpiracion;
        public bool IsExpired { get; init; } = qr.IsExpired;
        public string ImageBase64 { get; init; } = Convert.ToBase64String(qr.imagen ?? Array.Empty<byte>());

        public int IdEntidad { get; init; } = qr.idEntidad;
        public string Origen { get; init; } = qr.origen;
        public int? IdProducto { get; init; } = qr.idProducto;
        public int? IdActividad { get; init; } = qr.idActividad;
    }
}
