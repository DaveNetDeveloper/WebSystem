using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IActividadReservaRepository : IRepository<ActividadReserva, Guid>
    {        Task<IEnumerable<ActividadReserva>> GetByFiltersAsync(IFilters<ActividadReserva> filters,
                                                                   IQueryOptions<ActividadReserva>? options = null); 
    }
}