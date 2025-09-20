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
    public class ProductoRepository : BaseRepository<Producto>, IProductoRepository
    {
        private readonly ApplicationDbContext _context;
         
        public ProductoRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetByFiltersAsync(IFilters<Producto> filters,
                                                                  IQueryOptions<Producto>? options = null)
        {
            ProductoFilters _filters = ((ProductoFilters)filters);

            var predicate = PredicateBuilder.New<Producto>(true);

            if (_filters.Id.HasValue)
                predicate = predicate.And(u => u.id == _filters.Id.Value);

            if (_filters.IdEntidad.HasValue)
                predicate = predicate.And(u => u.idEntidad == _filters.IdEntidad.Value);

            if (_filters.Activo.HasValue)
                predicate = predicate.And(u => u.activo == _filters.Activo.Value);

            if (!string.IsNullOrEmpty(_filters.Nombre))
                predicate = predicate.And(u => u.nombre.ToLower() == _filters.Nombre.ToLower());

            var query = _context.Productos
                            .AsExpandable()
                            .Where(predicate);

            query = ApplyOrdening(query, options);
            query = ApplyPagination(query, options);
            return await query.ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id) =>
            await _context.Productos.FindAsync(id);
 
        public async Task<IEnumerable<Producto>> GetAllAsync() =>
            await _context.Productos.ToListAsync();
        
        public async Task<bool> AddAsync(Producto producto)
        {
            var nuevoProducto = new Producto {
                id = (_context.Productos.Count() + 1),
                nombre = producto.nombre,
                descripcion = producto.descripcion,
                descripcionCorta = producto.descripcionCorta,
                idEntidad = producto.idEntidad,
                imagen = producto.imagen,
                puntos = producto.puntos,
                activo = producto.activo,
                precio = producto.precio,
                descuento = producto.descuento,
                popularidad = producto.popularidad,
                disponible = producto.disponible,
                informacioExtra = producto.informacioExtra,
                linkInstagram = producto.linkInstagram,
                linkFacebook = producto.linkFacebook,
                linkYoutube = producto.linkYoutube
            };

            await _context.Productos.AddAsync(nuevoProducto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Producto producto)
        {
            var productoToUpdate = await _context.Productos.Where(c => c.id == producto.id).SingleOrDefaultAsync();
            if (productoToUpdate == null)
                return false;

            productoToUpdate.nombre = producto.nombre;
            productoToUpdate.descripcion = producto.descripcion;
            productoToUpdate.descripcionCorta = producto.descripcionCorta;
            productoToUpdate.idEntidad = producto.idEntidad;
            productoToUpdate.imagen = producto.imagen;
            productoToUpdate.puntos = producto.puntos;
            productoToUpdate.activo = producto.activo;
            productoToUpdate.precio = producto.precio;
            productoToUpdate.descuento = producto.descuento;
            productoToUpdate.popularidad = producto.popularidad;
            productoToUpdate.disponible = producto.disponible;
            productoToUpdate.informacioExtra = producto.informacioExtra;
            productoToUpdate.linkInstagram = producto.linkInstagram;
            productoToUpdate.linkFacebook = producto.linkFacebook;
            productoToUpdate.linkYoutube = producto.linkYoutube; 

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Remove(int id)
        { 
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
         
        public async Task<IEnumerable<ProductoImagen>> GetImagenesByProducto(int id)
        { 
            var imagenesProductoDB = await _context.ProductoImagenes
                                        .Include(p => p.Producto)
                                          //.ThenInclude(ec => ec.Categoria)
                                        .Where(pi => pi.idproducto == id).ToListAsync();

            if (imagenesProductoDB != null && imagenesProductoDB.Any()) {

                return imagenesProductoDB.OrderBy(ip => ip.id);
            } 
            return null;
        }
    }
}