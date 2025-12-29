using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class CampanaAccionesRepository : BaseRepository<CampanaAccionesRepository>, 
                                             ICampanaAccionesRepository
    {
        private readonly ApplicationDbContext _context;

        public CampanaAccionesRepository(ApplicationDbContext context) {
            _context = context;
        } 
        public async Task<IEnumerable<CampanaAcciones>> GetCampanaAccionesAllAsync() =>
            await _context.CampanaAcciones.ToListAsync();
        public CampanaAcciones GetAccionesByIdCampanaAsync(CampanaAcciones campanaAcciones) 
            =>  _context.CampanaAcciones.SingleOrDefault(u => u.idCampana == campanaAcciones.idCampana && 
                                                              u.idAccion == campanaAcciones.idAccion);
        public bool AddCampanaAccionAsync(CampanaAcciones campanaAcciones) {
             
            var nuevo = new CampanaAcciones
            {
                idCampana = campanaAcciones.idCampana,
                idAccion = campanaAcciones.idAccion,
                fecha = DateTime.UtcNow,
                accionDetalle = campanaAcciones.accionDetalle
            };

            _context.CampanaAcciones.Add(nuevo);
            _context.SaveChangesAsync();
            return true;
        } 
        public bool UpdateCampanaAccionAsync(CampanaAcciones campanaAcciones)
        {
            var updated = _context.CampanaAcciones.Where(r => r.idCampana == campanaAcciones.idCampana && 
                                                              r.idAccion == campanaAcciones.idAccion).SingleOrDefault();
            if (updated == null)
                return false;

            updated.idCampana = campanaAcciones.idCampana;
            updated.idAccion = campanaAcciones.idAccion;
            updated.fecha = DateTime.UtcNow;
            updated.accionDetalle = campanaAcciones.accionDetalle;

            _context.SaveChanges(); 
            return true;
        }
        public bool RemoveCampanaAccion(CampanaAcciones campanaAcciones)
        { 
            var toDelete = _context.CampanaAcciones.SingleOrDefault(u => u.idCampana == campanaAcciones.idCampana && 
                                                                         u.idAccion == campanaAcciones.idAccion);
            if (toDelete == null)
                return false;

            _context.CampanaAcciones.Remove(toDelete);
            _context.SaveChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCampana"></param>
        /// <returns> IEnumerable<Accion> </returns>
        public async Task<IEnumerable<Accion>> GetAccionesByCampana(int idCampana)
        {
            var campanaAccion = _context.CampanaAcciones.Include(a => a.Accion).Where(ca => ca.idCampana == idCampana);
            if (campanaAccion != null && campanaAccion.Any())
            { 
                var accionesResult = new List<Accion>();
                foreach (var accionCampana in campanaAccion) 
                {
                    accionesResult.Add(accionCampana.Accion);
                }
                return accionesResult.OrderBy(ip => ip.tipoAccion); 
            } 
            return null;
        } 
    }
}