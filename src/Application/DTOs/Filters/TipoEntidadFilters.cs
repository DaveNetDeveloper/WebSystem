using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class TipoEntidadFilters : IFilters<TipoEntidad>
    {
        public Guid? Id { get; set; }
        public string? Nombre { get; set; }  
    }
}