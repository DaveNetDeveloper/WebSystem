using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICampanaService : IService<Campana, int>
    {
        Task<IEnumerable<Campana>> GetByFiltersAsync(CampanaFilters filters,
                                                     IQueryOptions<Campana>? queryOptions = null);

        Task<IEnumerable<Segmento>> GetSegmentoByCampana(int idCampana);
        Task<IEnumerable<Accion>> GetAccionesByCampana(int idCampana);
        //Task<bool> EjecutarAccionesForUser(IEnumerable<Accion> acciones, int idUsuario);
    }
}