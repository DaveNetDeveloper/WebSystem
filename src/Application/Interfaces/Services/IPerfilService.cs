using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface IPerfilService : IService<Perfil, Guid>
    {
        Task<IEnumerable<Perfil>> GetByFiltersAsync(PerfilFilters filters,
                                                     IQueryOptions<Perfil>? queryOptions = null);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}