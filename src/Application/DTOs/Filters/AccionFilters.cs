using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class AccionFilters : IFilters<Accion>
    {
        public Guid? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? TipoAccion { get; set; }
        public bool? Activo { get; set; }  
    }
}