using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class RecompensaFilters : IFilters<Recompensa>
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; } 
        public int? IdEntidad { get; set; } 
    }
}