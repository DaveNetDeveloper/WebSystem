using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
namespace Application.DTOs.Filters
{
    public class QRFilters : IFilters<QR>
    {
        public Guid? Id { get; set; }
        public bool? Activo { get; set; }
        public bool? Multicliente { get; set; }
        public bool? Consumido { get; set; }
        public int? IdProducto { get; set; }
        public DateTime? FechaExpiracion { get; set; }
    }
}