using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface IFAQService : IService<FAQ, Guid>
    {
        Task<IEnumerable<FAQ>> GetByFiltersAsync(FAQFilters filters,
                                                 IQueryOptions<FAQ>? queryOptions = null);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}