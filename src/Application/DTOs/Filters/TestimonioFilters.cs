using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class TestimonioFilters : IFilters<Testimonio>
    {
        public int? Id { get; set; }
        public string? NombreUsuario { get; set; }
    }
}