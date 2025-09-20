using Application.DTOs.Filters;
using Application.DTOs.DataQuery;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IDataQueryService
    {
        Task<IReadOnlyList<vUsuariosInactivosDTO>> ObtenerUsuariosInactivos(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vActividadUsuariosDTO>> ObtenerActividadUsuarios(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vUsuariosIdiomasDTO>> ObtenerUsuariosIdiomas(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vUsuariosDispositivosDTO>> ObtenerUsuariosDispositivos(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vRolesUsuariosDTO>> ObtenerRolesUsuarios(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vVisitasTipoDispositivoDTO>> ObtenerVisitasTipoDispositivo(int page = 1, int pageSize = 100);
        Task<IReadOnlyList<vTotalUltimasTransaccionesDTO>> ObtenerTotalUltimasTransacciones(int page = 1, int pageSize = 100);
        

    }
}