using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface IRolService : IService<Rol, Guid>
    {
        Task<IEnumerable<Rol>> GetByFiltersAsync(RolFilters filters,
                                                     IQueryOptions<Rol>? queryOptions = null);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}