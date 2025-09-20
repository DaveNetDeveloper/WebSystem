using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class RolFilters : IFilters<Rol>
    {
        public Guid? Id { get; set; }
        public string? Nombre { get; set; }  
    }
}