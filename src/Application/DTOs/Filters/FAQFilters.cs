using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class FAQFilters : IFilters<FAQ>
    {
        public Guid? Id { get; set; }
        public int? IdEntidad { get; set; }
    }
}