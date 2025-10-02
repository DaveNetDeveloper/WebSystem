using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class TransaccionFilters : IFilters<Transaccion>
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdProducto { get; set; }
        public int? IdActividad { get; set; } 
        public Guid? IdTipoTransaccion { get; set; }
    }
}