using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IQRService : IService<QR, Guid>
    {
        Task<IEnumerable<QR>> GetByFiltersAsync(QRFilters filters,
                                                IQueryOptions<QR>? queryOptions = null);
    }
}