using Application;
using Application.DTOs.DataQuery;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;  

namespace Application.Interfaces.Repositories
{
    public interface IDataQueryRepository
    {
        Task<IReadOnlyList<vUsuariosInactivosDTO>> ObtenerUsuariosInactivos(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vActividadUsuariosDTO>> ObtenerActividadUsuarios(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vUsuariosIdiomasDTO>> ObtenerUsuariosIdiomas(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vUsuariosDispositivosDTO>> ObtenerUsuariosDispositivos(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vRolesUsuariosDTO>> ObtenerRolesUsuarios(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vVisitasTipoDispositivoDTO>> ObtenerVisitasTipoDispositivo(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vTotalUltimasTransaccionesDTO>> ObtenerTotalUltimasTransacciones(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vCampanasUsuariosDTO>> ObtenerCampanasUsuarios(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vAllUserDataDTO>> ObtenerAllUserData(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vAllCampanasDataDTO>> ObtenerAllCampanasData(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vAsistenciaActividadesDTO>> ObtenerAsistenciaActividades(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vTotalErroresDTO>> ObtenerTotalErrores(int page = 1, int pageSize = 100);
    }
}