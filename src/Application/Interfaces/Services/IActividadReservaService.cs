using Application.DTOs.Filters; 
using Domain.Entities;
using Application.Interfaces.Common;

namespace Application.Interfaces.Services
{
    public interface IActividadReservaService : IService<ActividadReserva, Guid>
    {
        Task<IEnumerable<ActividadReserva>> GetByFiltersAsync(ActividadReservaFilters filters,
                                                              IQueryOptions<ActividadReserva>? queryOptions = null);
        string ReservarActividad(int idUsuario, int idActividad);

        bool ValidarReserva(int idUsuario, string codigoReserva);
    }
}