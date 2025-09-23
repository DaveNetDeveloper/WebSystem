using Application.DTOs.Filters; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _repo;

        public ConsultaService(IConsultaRepository repo) {
            _repo = repo;
        }

        public async Task<IEnumerable<Consulta>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<Consulta?> GetByIdAsync(Guid id)
            => await _repo.GetByIdAsync(id);

        public Task<IEnumerable<Consulta>> GetByFiltersAsync(ConsultaFilters filters,
                                                             IQueryOptions<Consulta>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public async Task<bool> AddAsync(Consulta consulta)
            => await _repo.AddAsync(consulta);

        public async Task<bool> UpdateAsync(Consulta consulta)
            => await _repo.UpdateAsync(consulta);

        public async Task<bool> Remove(Guid id)
              => await _repo.Remove(id);
    }
}