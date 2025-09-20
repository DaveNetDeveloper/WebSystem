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
    public class EntidadRepository : BaseRepository<Entidad>, IEntidadRepository
    {
        private readonly ApplicationDbContext _context;
         
        public EntidadRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Entidad>> GetAllAsync() =>
            await _context.Entidades.ToListAsync();

        public async Task<IEnumerable<Entidad>> GetByFiltersAsync(IFilters<Entidad> filters,
                                                                  IQueryOptions<Entidad>? options = null)
        {
            EntidadFilters _filters = ((EntidadFilters)filters); 

            var predicate = PredicateBuilder.New<Entidad>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());
       
            if (_filters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == _filters.Activo.Value);

            if (_filters.IdTipoEntidad.HasValue)
                predicate = predicate.And(u => u.idTipoEntidad == _filters.IdTipoEntidad.Value);

            var query = _context.Entidades
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<Entidad?> GetByIdAsync(int id) =>
            await _context.Entidades.FindAsync(id);

        public async Task<bool> AddAsync(Entidad entidad)
        {
            var nuevaEntidad = new Entidad {
                id = (_context.Entidades.Count() + 1),
                idTipoEntidad = entidad.idTipoEntidad,
                nombre = entidad.nombre,
                descripcion = entidad.descripcion,
                ubicacion = entidad.ubicacion,
                fechaAlta = DateTime.Now,
                popularidad = entidad.popularidad,
                activo = entidad.activo,
                imagen = entidad.imagen
            };  

            await _context.Entidades.AddAsync(nuevaEntidad);
            await _context.SaveChangesAsync();
            return true;
        } 

        public async Task<bool> UpdateAsync(Entidad entidad)
        {
            var updatedEntity = await _context.Entidades.Where(c => c.id == entidad.id).SingleOrDefaultAsync();
            if (updatedEntity == null)
                return false;

            updatedEntity.nombre = entidad.nombre;
            updatedEntity.descripcion = entidad.descripcion;
            updatedEntity.idTipoEntidad = entidad.idTipoEntidad; 
            updatedEntity.ubicacion = entidad.ubicacion;
            updatedEntity.fechaAlta = entidad.fechaAlta;
            updatedEntity.popularidad = entidad.popularidad;
            updatedEntity.activo = entidad.activo;
            updatedEntity.imagen = entidad.imagen;
             
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> Remove(int id) { 
            
            var entidadToDelete = await _context.Entidades.FindAsync(id);

            if (entidadToDelete == null) 
                return false;  

            _context.Entidades.Remove(entidadToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        //
        //Bindings - Table relations
        // 
        public async Task<IEnumerable<Categoria>> GetCategoriasByEntidad(int id)
        {
            var entidadCategorias = await _context.Entidades
                                        .Include(e => e.EntidadadCategorias)
                                            .ThenInclude(ec => ec.Categoria)
                                        //  .Include(u => u.UsuarioRoles)
                                        //  .ThenInclude(ur => ur.Entidad)
                                        .Where(ec => ec.id == id).ToListAsync();
             
            if (entidadCategorias != null && entidadCategorias.Any()) {

                var categorias = new List<Categoria>();
                foreach (var entidad in entidadCategorias) {

                    if (entidad != null && entidad.EntidadadCategorias != null) {   
                        
                        foreach (var ec in entidad.EntidadadCategorias) { 
                            var categoria = await _context.Categorias.FindAsync(ec.Categoria.id);   
                            categorias.Add(categoria); 
                        }
                    }
                }
                return categorias.OrderBy(c => c.nombre);
            }
            return null;
        } 
    }
}