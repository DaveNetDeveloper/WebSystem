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
        private readonly IUsuarioRecompensasRepository _repoUsuarioRecompensas;
        private readonly ITipoRecompensasRepository _repoTipoRecompensas;

        public RecompensaService(IRecompensaRepository repo,
                                 IUsuarioRecompensasRepository repoUsuarioRecompensas,
                                 ITipoRecompensasRepository repoTipoRecompensas) {
            _repo = repo;
            _repoUsuarioRecompensas = repoUsuarioRecompensas;
            _repoTipoRecompensas = repoTipoRecompensas;
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

        public async Task<int> GenerarRecompensa(int idUsuario, TipoRecompensa tipoRecompensa)
        {
            var returneId = -1;
            var recompensaFilter = new RecompensaFilters {
                IdTipoRecompensa = tipoRecompensa.id,
                IdEntidad = null // sin filtrar por entidad (recompensas genericas)
            };
            var recompensa = GetByFiltersAsync(recompensaFilter)?.Result.FirstOrDefault();

            var usuarioRecompensa = new UsuarioRecompensa {
                idrecompensa = recompensa.id,
                idusuario = idUsuario,
                fecha = DateTime.UtcNow
            };
            var usuarioRecompensaResult = AddUsuarioRecompensaAsync(usuarioRecompensa);

            if (usuarioRecompensaResult.Result)
                return recompensa.id;
            else
                return -1;
        }

        public Task<IEnumerable<Recompensa>> GetRecompensasByUsuario(int idUsuario)
            => _repo.GetRecompensasByUsuario(idUsuario);

        //
        // CRUD para la tabla [TipoRecompensas]
        //
        public Task<IEnumerable<TipoRecompensa>> GetAllTiposRecompensas()
            => _repoTipoRecompensas.GetAllAsync();
        public Task<TipoRecompensa> GetTipoRecompensa(Guid idTipoRecompensa)
            => _repoTipoRecompensas.GetByIdAsync(idTipoRecompensa);

        public Task<bool> AddTipoRecompensa(TipoRecompensa tipoRecompensa)
            => _repoTipoRecompensas.AddAsync(tipoRecompensa);

        public Task<bool> UpdateTipoRecompensa(TipoRecompensa tipoRecompensa)
             => _repoTipoRecompensas.UpdateAsync(tipoRecompensa);

        public Task<bool> RemoveTipoRecompensa(Guid idTipoRecompensa)
              => _repoTipoRecompensas.Remove(idTipoRecompensa);


        //
        // CRUD para la tabla [UsuarioRecompensas]
        //
        public Task<IEnumerable<UsuarioRecompensa>> GetUsuariosRecompensasAllAsync()
            => _repoUsuarioRecompensas.GetUsuariosRecompensasAllAsync();

        public Task<UsuarioRecompensa> GetUsuarioRecompensaAsync(int idUsuario, int idRecompensa)
            => _repoUsuarioRecompensas.GetUsuarioRecompensa(idUsuario, idRecompensa);

        public Task<bool> AddUsuarioRecompensaAsync(UsuarioRecompensa usuarioRecompensa)
            => _repoUsuarioRecompensas.AddUsuarioRecompensa(usuarioRecompensa);

        public Task<bool> UpdateUsuarioRecompensaAsync(UsuarioRecompensa usuarioRecompensa)
             => _repoUsuarioRecompensas.UpdateUsuarioRecompensa(usuarioRecompensa);

        public Task<bool> RemoveUsuarioRecompensa(int idUsuario, int idRecompensa)
              => _repoUsuarioRecompensas.RemoveUsuarioRecompensa(idUsuario, idRecompensa);
    }
}