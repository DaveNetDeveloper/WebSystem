using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ITipoEnvioCorreoRepository : IRepository<TipoEnvioCorreo, Guid>
    {
        Task<IEnumerable<TipoEnvioCorreo>> GetByFiltersAsync(IFilters<TipoEnvioCorreo> filters, 
                                                             IQueryOptions<TipoEnvioCorreo>? options = null);
    }
}