using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IAccionService : IService<Accion, Guid>
    {
        Task<IEnumerable<Accion>> GetByFiltersAsync(AccionFilters filters,
                                                    IQueryOptions<Accion>? queryOptions = null);
    }
}