//using Application.DTOs.Filters;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface ITipoCampanaService : IService<TipoCampana, Guid>
    {
        Task<IEnumerable<TipoCampana>> GetByFiltersAsync(TipoCampanaFilters filters,
                                                         IQueryOptions<TipoCampana>? queryOptions = null);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}