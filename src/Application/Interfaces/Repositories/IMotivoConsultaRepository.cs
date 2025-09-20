using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IMotivoConsultaRepository : IRepository<MotivoConsulta, Guid>
    {

        Task<IEnumerable<MotivoConsulta>> GetByFiltersAsync(IFilters<MotivoConsulta> filters, IQueryOptions<MotivoConsulta>? options = null);

    }
}