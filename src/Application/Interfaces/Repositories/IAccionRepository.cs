using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IAccionRepository : IRepository<Accion, Guid>
    {
        Task<IEnumerable<Accion>> GetByFiltersAsync(IFilters<Accion> filters, 
                                                    IQueryOptions<Accion>? options = null); 
    }
}