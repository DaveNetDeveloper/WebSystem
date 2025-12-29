using Application.DTOs.Filters; 
using Application.DTOs.Responses;
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface IActividadReservaService : IService<ActividadReserva, Guid>
    {
        Task<IEnumerable<ActividadReserva>> GetByFiltersAsync(ActividadReservaFilters filters,
                                                              IQueryOptions<ActividadReserva>? queryOptions = null);
        ReservaActividadDTO ReservarActividad(int idUsuario, int idActividad);
        bool ValidarReserva(int idUsuario, string codigoReserva);
        Task<byte[]> ExportarAsync(ExportFormat formato);

    }
}