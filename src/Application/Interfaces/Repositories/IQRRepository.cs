using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IQRRepository : IRepository<QR, Guid>
    {
        Task<IEnumerable<QR>> GetByFiltersAsync(IFilters<QR> filters, IQueryOptions<QR>? options = null);
    }
}