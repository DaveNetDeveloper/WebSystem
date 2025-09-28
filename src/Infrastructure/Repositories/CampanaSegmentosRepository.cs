using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class CampanaSegmentosRepository : BaseRepository<CampanaSegmentosRepository>, ICampanaSegmentosRepository
    {
        private readonly ApplicationDbContext _context;

        public CampanaSegmentosRepository(ApplicationDbContext context) {
            _context = context;
        } 
        public async Task<IEnumerable<CampanaSegmentos>> GetCampanaSegmentosAllAsync() =>
            await _context.CampanaSegmentos.ToListAsync();
        public CampanaSegmentos GetSegmentosByIdCampanaAsync(CampanaSegmentos campanaSegmento) 
            =>  _context.CampanaSegmentos.SingleOrDefault(u => u.idCampana == campanaSegmento.idCampana && 
                                                               u.idSegmento == campanaSegmento.idSegmento);
        public bool AddCampanaSegmentosAsync(CampanaSegmentos campanaSegmento) {
             
            var nuevo = new CampanaSegmentos
            {
                idCampana = campanaSegmento.idCampana,
                idSegmento = campanaSegmento.idSegmento,
                fecha = DateTime.UtcNow
            };

            _context.CampanaSegmentos.Add(nuevo);
            _context.SaveChangesAsync();
            return true;
        } 
        public bool UpdateCampanaSegmentosAsync(CampanaSegmentos campanaSegmento)
        {
            var updated = _context.CampanaSegmentos.Where(r => r.idCampana == campanaSegmento.idCampana && 
                                                               r.idSegmento == campanaSegmento.idSegmento).SingleOrDefault();
            if (updated == null)
                return false;

            updated.idCampana = campanaSegmento.idCampana;
            updated.idSegmento = campanaSegmento.idSegmento;
            updated.fecha = DateTime.UtcNow; 

            _context.SaveChanges(); 
            return true;
        }
        public bool RemoveCampanaSegmentos(CampanaSegmentos campanaSegmento)
        { 
            var toDelete = _context.CampanaSegmentos.SingleOrDefault(u => u.idCampana == campanaSegmento.idCampana && 
                                                                          u.idSegmento == campanaSegmento.idSegmento);
            if (toDelete == null)
                return false;

            _context.CampanaSegmentos.Remove(toDelete);
            _context.SaveChanges();

            return true;
        }
        public async Task<IEnumerable<Segmento>> GetSegmentosByCampana(int idCampana)
        {
            var campanaSegmento = _context.CampanaSegmentos.Include(s => s.Segmento)
                                                           .Include(c => c.Campana)
                                                           .Where(cs => cs.idCampana == idCampana);
            
            if (campanaSegmento != null && campanaSegmento.Any())  {
                var segmentosResult = new List<Segmento>();
                foreach (var segmentoCampana in campanaSegmento) {
                    segmentosResult.Add(segmentoCampana.Segmento);
                }
                return segmentosResult.OrderBy(ip => ip.id);
            }
            return null;
        }
    }
}