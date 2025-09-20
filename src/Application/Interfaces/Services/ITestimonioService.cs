using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ITestimonioService : IService<Testimonio, int>
    {
        Task<IEnumerable<Testimonio>> GetByFiltersAsync(TestimonioFilters filters,
                                                     IQueryOptions<Testimonio>? queryOptions = null);
    }
}