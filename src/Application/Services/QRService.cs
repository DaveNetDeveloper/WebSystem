using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class QRService : IQRService
    {
        private readonly IQRRepository _repo;

        public QRService(IQRRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<QR>> GetAllAsync()
            => _repo.GetAllAsync();
        public Task<QR?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);

        public Task<IEnumerable<QR>> GetByFiltersAsync(QRFilters filters,
                                                            IQueryOptions<QR>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);
        public Task<bool> AddAsync(QR qr)
            => _repo.AddAsync(qr);

        public Task<bool> UpdateAsync(QR qr)
             => _repo.UpdateAsync(qr);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);
    }
}