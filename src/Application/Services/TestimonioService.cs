using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class TestimonioService : ITestimonioService
    {
        private readonly ITestimonioRepository _repo;

        public TestimonioService(ITestimonioRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<Testimonio>> GetAllAsync()
            => _repo.GetAllAsync();
        public Task<Testimonio?> GetByIdAsync(int id)
           => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Testimonio>> GetByFiltersAsync(TestimonioFilters filters,
                                                            IQueryOptions<Testimonio>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<bool> AddAsync(Testimonio testimonio)
            => _repo.AddAsync(testimonio);

        public Task<bool> UpdateAsync(Testimonio testimonio)
             => _repo.UpdateAsync(testimonio);

        public Task<bool> Remove(int id)
              => _repo.Remove(id);
    }
}