using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class PerfilFilters : IFilters<Perfil>
    {
        public Guid? Id { get; set; }
        public string? Nombre { get; set; }
        public bool? Activo { get; set; }
        public Guid? IdRol { get; set; }
    }
}