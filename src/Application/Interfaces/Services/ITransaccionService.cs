using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface ITransaccionService : IService<Transaccion, int>
    {
        Task<IEnumerable<Transaccion>> GetByFiltersAsync(TransaccionFilters filters,
                                                     IQueryOptions<Transaccion>? queryOptions = null);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}