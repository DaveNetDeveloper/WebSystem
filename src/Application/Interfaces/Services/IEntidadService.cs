using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface IEntidadService : IService<Entidad, int>
    {
        Task<IEnumerable<Entidad>> GetByFiltersAsync(EntidadFilters filters,
                                                     IQueryOptions<Entidad>? queryOptions = null);
        Task<IEnumerable<Categoria>> GetCategoriasByEntidad(int id);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}