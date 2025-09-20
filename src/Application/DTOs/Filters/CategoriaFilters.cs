using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class CategoriaFilters : IFilters<Categoria>
    {
        public Guid? Id { get; set; }
        public Guid? IdTipoEntidad { get; set; }
        public string? Nombre { get; set; }     
    }
}