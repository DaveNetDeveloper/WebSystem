using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ITestimonioRepository : IRepository<Testimonio, int>
    {
        //Task<IEnumerable<Testimonio>> GetAllAsync();
        //Task<Testimonio?> GetByIdAsync(int id);
        // Task<bool> AddAsync(Testimonio testimonio);
        //Task<bool> UpdateAsync(Testimonio testimonio);
        // Task<bool> Remove(int id);

        Task<IEnumerable<Testimonio>> GetByFiltersAsync(IFilters<Testimonio> filters, IQueryOptions<Testimonio>? options = null);

    }
}