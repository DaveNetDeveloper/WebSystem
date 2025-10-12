using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ICampanaAccionesRepository
    {
        Task<IEnumerable<CampanaAcciones>> GetCampanaAccionesAllAsync();
        CampanaAcciones GetAccionesByIdCampanaAsync(CampanaAcciones campanaAcciones);
        bool AddCampanaAccionAsync(CampanaAcciones campanaAcciones);
        bool UpdateCampanaAccionAsync(CampanaAcciones campanaAcciones);
        bool RemoveCampanaAccion(CampanaAcciones campanaAcciones);

        Task<IEnumerable<Accion>> GetAccionesByCampana(int idCampana);
    }
}