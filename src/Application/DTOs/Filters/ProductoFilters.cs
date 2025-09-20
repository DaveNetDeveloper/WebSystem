using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class ProductoFilters : IFilters<Producto>
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public int? IdEntidad { get; set; }
        public bool? Activo { get; set; }  
    }
}