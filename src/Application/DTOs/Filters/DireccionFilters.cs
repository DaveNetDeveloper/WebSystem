using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class DireccionFilters : IFilters<Direccion>
    {
        public int? Id { get; set; }
        public string? TipoVia { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Ciudad { get; set; }
        public string? ComunidadAutonoma { get; set; }
    }
}