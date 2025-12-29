using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class PerfilRepository : BaseRepository<Perfil>, IPerfilRepository
    {
        private readonly ApplicationDbContext _context;

        public PerfilRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Perfil>> GetByFiltersAsync(IFilters<Perfil> filters,
                                                                 IQueryOptions<Perfil>? options = null)
        {
            PerfilFilters _filters = ((PerfilFilters)filters);

            var predicate = PredicateBuilder.New<Perfil>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (_filters.IdRol.HasValue)
                predicate = predicate.And(u => u.idRol == _filters.IdRol.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            if (_filters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == _filters.Activo.Value);

            var query = _context.Perfiles
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }
        public async Task<Perfil?> GetByIdAsync(Guid id) =>
            await _context.Perfiles.FindAsync(id);

        public async Task<IEnumerable<Perfil>> GetAllAsync() =>
            await _context.Perfiles.ToListAsync();

        public async Task<bool> AddAsync(Perfil perfil) {

            var nuevoPerfil = new Perfil
            {
                id = Guid.NewGuid(),
                nombre = perfil.nombre,
                descripcion = perfil.descripcion,
                activo = true,
                idRol = perfil.idRol
            };

            if (perfil.id != null)
                nuevoPerfil.id  = perfil.id;

            await _context.Perfiles.AddAsync(nuevoPerfil);
            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> UpdateAsync(Perfil perfil)
        {
            var updatedPerfil = await _context.Perfiles.Where(r => r.id == perfil.id).SingleOrDefaultAsync();
            if (updatedPerfil == null)
                return false;

            updatedPerfil.nombre = perfil.nombre;
            updatedPerfil.descripcion = perfil.descripcion;
            updatedPerfil.activo = perfil.activo;
            updatedPerfil.idRol = perfil.idRol;

            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> Remove(Guid id)
        { 
            var perfilToDelete = await _context.Perfiles.FindAsync(id);

            if (perfilToDelete == null)
                return false;

            _context.Perfiles.Remove(perfilToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
