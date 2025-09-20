using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface IProductoRepository : IRepository<Producto, int>
    {
        Task<IEnumerable<Producto>> GetByFiltersAsync(IFilters<Producto> filters, IQueryOptions<Producto>? options = null);
        Task<IEnumerable<ProductoImagen>> GetImagenesByProducto(int id);
    }
}