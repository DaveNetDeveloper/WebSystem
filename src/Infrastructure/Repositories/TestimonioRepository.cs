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
    public class TestimonioRepository : BaseRepository<Testimonio>, ITestimonioRepository
    {
        private readonly ApplicationDbContext _context;
         
        public TestimonioRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Testimonio>> GetByFiltersAsync(IFilters<Testimonio> filters,
                                                                  IQueryOptions<Testimonio>? options = null)
        {
            TestimonioFilters _filters = ((TestimonioFilters)filters); 

            var predicate = PredicateBuilder.New<Testimonio>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (_filters.IdEntidad.HasValue)
                predicate = predicate.And(u => u.idEntidad == _filters.IdEntidad.Value);

            if (!string.IsNullOrEmpty(_filters.NombreUsuario))
                predicate = predicate.And(u => u.nombreUsuario.ToLower() == _filters.NombreUsuario.ToLower());

            var query = _context.Testimonios
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<Testimonio?> GetByIdAsync(int id) =>
            await _context.Testimonios.FindAsync(id);

        public async Task<IEnumerable<Testimonio>> GetAllAsync() =>
            await _context.Testimonios.ToListAsync();

        public async Task<bool> AddAsync(Testimonio testimonio)
        {
            var nuevoTestimonio = new Testimonio {
                id = (_context.Testimonios.Count() + 1),
                texto = testimonio.texto,
                nombreUsuario = testimonio.nombreUsuario,
                fecha = testimonio.fecha,
                imagen = testimonio.imagen,
                idEntidad = testimonio.idEntidad
            };

            await _context.Testimonios.AddAsync(nuevoTestimonio);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Testimonio testimonio)
        {
            var updatedTestimonio = await _context.Testimonios.Where(c => c.id == testimonio.id).SingleOrDefaultAsync();
            if (updatedTestimonio == null)
                return false;

            updatedTestimonio.texto = testimonio.texto;
            updatedTestimonio.nombreUsuario = testimonio.nombreUsuario;
            updatedTestimonio.fecha = testimonio.fecha;
            updatedTestimonio.imagen = testimonio.imagen;
            updatedTestimonio.idEntidad = testimonio.idEntidad;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(int id)
        { 
            var testimonioToDelete = await _context.Testimonios.FindAsync(id); 
            if (testimonioToDelete == null)
                return false;

            _context.Testimonios.Remove(testimonioToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}