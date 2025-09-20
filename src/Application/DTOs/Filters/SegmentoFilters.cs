using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class SegmentoFilters : IFilters<Segmento>
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public Guid? idTipoSegmento { get; set; } 
    }
}