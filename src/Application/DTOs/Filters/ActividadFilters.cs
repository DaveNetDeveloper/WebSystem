using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class ActividadFilters : IFilters<Actividad>
    {
        public int? Id { get; set; }
        public int? IdEntidad { get; set; }
        public string? Nombre { get; set; }
        public bool? Activo { get; set; }
        public Guid? IdTipoActividad { get; set; }
        public bool? Gratis { get; set; } 
    }
}