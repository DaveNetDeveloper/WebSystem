using Application.DTOs.Filters;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class MotivoConsultaService : IMotivoConsultaService
    {
        private readonly IMotivoConsultaRepository _repo;

        public MotivoConsultaService(IMotivoConsultaRepository repo) {
            _repo = repo;
        }

        public async Task<IEnumerable<MotivoConsulta>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<MotivoConsulta?> GetByIdAsync(Guid id)
            => await _repo.GetByIdAsync(id);

        public Task<IEnumerable<MotivoConsulta>> GetByFiltersAsync(MotivoConsultaFilters filters,
                                                            IQueryOptions<MotivoConsulta>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public async Task<bool> AddAsync(MotivoConsulta motivoConsulta)
            => await _repo.AddAsync(motivoConsulta);

        public async Task<bool> UpdateAsync(MotivoConsulta motivoConsulta)
            => await _repo.UpdateAsync(motivoConsulta);

        public async Task<bool> Remove(Guid id)
              => await _repo.Remove(id);
    }
}