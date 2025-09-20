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
    public class RolRepository : BaseRepository<Rol>, IRolRepository
    {
        private readonly ApplicationDbContext _context;

        public RolRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> GetByFiltersAsync(IFilters<Rol> filters,
                                                                  IQueryOptions<Rol>? options = null)
        {
            RolFilters _filters = ((RolFilters)filters);

            var predicate = PredicateBuilder.New<Rol>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            var query = _context.Roles
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<Rol?> GetByIdAsync(Guid id) =>
            await _context.Roles.FindAsync(id);

        public async Task<IEnumerable<Rol>> GetAllAsync() =>
            await _context.Roles.ToListAsync();

        public async Task<bool> AddAsync(Rol rol) {
             
            var nuevoRol = new Rol {
                id = new Guid(),
                nombre = rol.nombre,
                descripcion = rol.descripcion
            };

            await _context.Roles.AddAsync(nuevoRol);
            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> UpdateAsync(Rol rol)
        {
            var updatedRol = await _context.Roles.Where(r => r.id == rol.id).SingleOrDefaultAsync();
            if (updatedRol == null)
                return false;

            updatedRol.nombre = rol.nombre;
            updatedRol.descripcion = rol.descripcion; 

            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> Remove(Guid id)
        { 
            var rolToDelete = await _context.Roles.FindAsync(id);

            if (rolToDelete == null)
                return false;

            _context.Roles.Remove(rolToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
