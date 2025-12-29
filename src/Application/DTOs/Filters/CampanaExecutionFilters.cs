using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class CampanaExecutionFilters : IFilters<CampanaExecution>
    {
        public Guid? Id { get; set; } 
        public string? Estado { get; set; }
        public int? IdCampana { get; set; }
    }
}