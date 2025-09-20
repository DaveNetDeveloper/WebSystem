using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ITransaccionService : IService<Transaccion, int>
    {
        Task<IEnumerable<Transaccion>> GetByFiltersAsync(TransaccionFilters filters,
                                                     IQueryOptions<Transaccion>? queryOptions = null); 
    }
}