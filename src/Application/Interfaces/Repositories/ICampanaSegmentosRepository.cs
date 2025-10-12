using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ICampanaSegmentosRepository
    {
        Task<IEnumerable<CampanaSegmentos>> GetCampanaSegmentosAllAsync();
        CampanaSegmentos GetSegmentosByIdCampanaAsync(CampanaSegmentos campanaSegmentos);
        bool AddCampanaSegmentosAsync(CampanaSegmentos campanaSegmentos);
        bool UpdateCampanaSegmentosAsync(CampanaSegmentos campanaSegmentos);
        bool RemoveCampanaSegmentos(CampanaSegmentos campanaSegmentos);

        Task<IEnumerable<Segmento>> GetSegmentosByCampana(int idCampana);
    }
}