using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;
using System.Threading.Tasks;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface IMotivoConsultaService : IService<MotivoConsulta, Guid>
    {
        Task<IEnumerable<MotivoConsulta>> GetByFiltersAsync(MotivoConsultaFilters filters,
                                                        IQueryOptions<MotivoConsulta>? queryOptions = null);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}