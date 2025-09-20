using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RecompensaService : IRecompensaService
    {
        private readonly IRecompensaRepository _repo;

        public RecompensaService(IRecompensaRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<Recompensa>> GetAllAsync()
            => _repo.GetAllAsync();
        public Task<Recompensa?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);
        public Task<IEnumerable<Recompensa>> GetByFiltersAsync(RecompensaFilters filters,
                                                            IQueryOptions<Recompensa>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<bool> AddAsync(Recompensa recompensa)
            => _repo.AddAsync(recompensa);

        public Task<bool> UpdateAsync(Recompensa recompensa)
            => _repo.UpdateAsync(recompensa);

        public Task<bool> Remove(int id)
            => _repo.Remove(id);

        public Task<IEnumerable<Recompensa>> GetRecompensasByUsuario(int idUsuario)
            => _repo.GetRecompensasByUsuario(idUsuario); 

    }
}