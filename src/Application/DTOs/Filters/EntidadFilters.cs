using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class EntidadFilters : IFilters<Entidad>
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public bool? Activo { get; set; }
        public Guid? IdTipoEntidad { get; set; }
        public string? Manager { get; set; }
        
    }
}