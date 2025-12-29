using Application.DTOs.DataQuery;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface ISegmentoService : IService<Segmento, int>
    {
        Task<IEnumerable<Segmento>> GetByFiltersAsync(SegmentoFilters filters,
                                                      IQueryOptions<Segmento>? queryOptions = null);
        void ApplySegmentsForUser(vAllUserDataDTO usuario);
        Task<IEnumerable<Usuario>> GetUsuariosBySegmento(int idSegmento);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}