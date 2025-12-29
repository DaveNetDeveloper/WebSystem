using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IConsultaRepository : IRepository<Consulta, Guid>
    {
        Task<IEnumerable<Consulta>> GetByFiltersAsync(IFilters<Consulta> filters, 
                                                      IQueryOptions<Consulta>? options = null);
    }
}