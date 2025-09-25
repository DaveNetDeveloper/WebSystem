using Application.DTOs.DataQuery;
using Application.Interfaces.Repositories; 
using Application.Interfaces.Services; 

namespace Application.Services
{
    public class DataQueryService : IDataQueryService
    {
        private readonly IDataQueryRepository _repo;

        public DataQueryService(IDataQueryRepository repo) {
            _repo = repo;
        }

        public async Task<IReadOnlyList<vUsuariosInactivosDTO>> ObtenerUsuariosInactivos(int page = 1, int pageSize = 100)
            => await _repo.ObtenerUsuariosInactivos(page, pageSize);

        public async Task<IReadOnlyList<vActividadUsuariosDTO>> ObtenerActividadUsuarios(int page = 1, int pageSize = 100)
            => await _repo.ObtenerActividadUsuarios(page, pageSize);

        public async Task<IReadOnlyList<vUsuariosIdiomasDTO>> ObtenerUsuariosIdiomas(int page = 1, int pageSize = 100)
            => await _repo.ObtenerUsuariosIdiomas(page, pageSize);

        public async Task<IReadOnlyList<vUsuariosDispositivosDTO>> ObtenerUsuariosDispositivos(int page = 1, int pageSize = 100)
            => await _repo.ObtenerUsuariosDispositivos(page, pageSize);

        public async Task<IReadOnlyList<vRolesUsuariosDTO>> ObtenerRolesUsuarios(int page = 1, int pageSize = 100)
           => await _repo.ObtenerRolesUsuarios(page, pageSize);

        public async Task<IReadOnlyList<vVisitasTipoDispositivoDTO>> ObtenerVisitasTipoDispositivo(int page = 1, int pageSize = 100)
           => await _repo.ObtenerVisitasTipoDispositivo(page, pageSize);

        public async Task<IReadOnlyList<vTotalUltimasTransaccionesDTO>> ObtenerTotalUltimasTransacciones(int page = 1, int pageSize = 100)
          => await _repo.ObtenerTotalUltimasTransacciones(page, pageSize);

        public async Task<IReadOnlyList<vCampanasUsuariosDTO>> ObtenerCampanasUsuarios(int page = 1, int pageSize = 100)
          => await _repo.ObtenerCampanasUsuarios(page, pageSize);

    }
}