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
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> GetByFiltersAsync(IFilters<Categoria> filters,
                                                                  IQueryOptions<Categoria>? options = null)
        {
            CategoriaFilters _filters = ((CategoriaFilters)filters);

            var predicate = PredicateBuilder.New<Categoria>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            if (_filters.IdTipoEntidad.HasValue)
                predicate = predicate.And(u => u.idTipoEntidad == _filters.IdTipoEntidad.Value);

            var query = _context.Categorias
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<Categoria?> GetByIdAsync(Guid id) =>
            await _context.Categorias.FindAsync(id);

        public async Task<IEnumerable<Categoria>> GetAllAsync() =>
            await _context.Categorias.ToListAsync();
          
        public async Task<bool> AddAsync(Categoria categoria)
        { 
            var nuevaCategoria = new Categoria {
                id = Guid.NewGuid(),
                idTipoEntidad = categoria.idTipoEntidad,
                nombre = categoria.nombre,
                descripcion = categoria.descripcion
            };

            if (categoria.id != null)
                nuevaCategoria.id = categoria.id;

            await _context.Categorias.AddAsync(nuevaCategoria);
            await _context.SaveChangesAsync();
            return true;  
        }
         
        public async Task<bool> UpdateAsync(Categoria categoria)
        {
            var updatedCategory = await _context.Categorias.Where(c => c.id == categoria.id).SingleOrDefaultAsync();
            if (updatedCategory == null)
                return false;

            updatedCategory.nombre = categoria.nombre;
            updatedCategory.descripcion = categoria.descripcion;
            updatedCategory.idTipoEntidad = categoria.idTipoEntidad;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(Guid id) {

            var categoriaToDelete = await _context.Categorias.FindAsync(id);

            if (categoriaToDelete == null) 
                return false;  

            _context.Categorias.Remove(categoriaToDelete);
            await _context.SaveChangesAsync();
            return true;  
        }
    }
}
