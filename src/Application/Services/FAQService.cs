using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class FAQService : IFAQService
    {
        private readonly IFAQRepository _repo;

        public FAQService(IFAQRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<FAQ>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<IEnumerable<FAQ>> GetByFiltersAsync(FAQFilters filters,
                                                            IQueryOptions<FAQ>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<FAQ?> GetByIdAsync(Guid id)
           => _repo.GetByIdAsync(id);

        public Task<bool> AddAsync(FAQ faq)
            => _repo.AddAsync(faq);

        public Task<bool> UpdateAsync(FAQ faq)
             => _repo.UpdateAsync(faq);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id); 
    }
}