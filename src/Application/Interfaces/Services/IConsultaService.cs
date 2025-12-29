using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using System.Threading.Tasks;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface IConsultaService : IService<Consulta, Guid>
    {
        Task<IEnumerable<Consulta>> GetByFiltersAsync(ConsultaFilters filters,
                                                      IQueryOptions<Consulta>? queryOptions = null);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}