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
    public class ActividadReservaService : IActividadReservaService
    {
        private readonly IActividadReservaRepository _repo; 

        public ActividadReservaService(IActividadReservaRepository repo) {
            _repo = repo;
        }

        public Task<IEnumerable<ActividadReserva>> GetAllAsync()
            => _repo.GetAllAsync();
        public Task<ActividadReserva?> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);
        public Task<IEnumerable<ActividadReserva>> GetByFiltersAsync(ActividadReservaFilters filters,
                                                                     IQueryOptions<ActividadReserva>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<bool> AddAsync(ActividadReserva actividadReserva)
            => _repo.AddAsync(actividadReserva);

        public Task<bool> UpdateAsync(ActividadReserva actividadReserva)
            => _repo.UpdateAsync(actividadReserva);

        public Task<bool> Remove(Guid id)
            => _repo.Remove(id);

        public bool ValidarReserva(int idUsuario, string codigoReserva)
        {

             

            return true;
        }

        public ReservaActividadDTO ReservarActividad(int idUsuario, int idActividad)
        { 
            //var returneId = -1;
            //var recompensaFilter = new RecompensaFilters {
            //    IdTipoRecompensa = tipoRecompensa.id,
            //    IdEntidad = null // sin filtrar por entidad (recompensas genericas)
            //};
            //var recompensa = GetByFiltersAsync(recompensaFilter)?.Result.FirstOrDefault();

            //var usuarioRecompensa = new UsuarioRecompensa {
            //    idrecompensa = recompensa.id,
            //    idusuario = idUsuario,
            //    fecha = DateTime.UtcNow
            //};
            //var usuarioRecompensaResult = AddUsuarioRecompensaAsync(usuarioRecompensa);

            //if (usuarioRecompensaResult.Result)
            //    return recompensa.id;
             
            return new ReservaActividadDTO();
        } 
    }
}