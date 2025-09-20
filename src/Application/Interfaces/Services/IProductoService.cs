using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces.Common;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IProductoService : IService<Producto, int>
    {
        Task<IEnumerable<Producto>> GetByFiltersAsync(ProductoFilters filters,
                                                        IQueryOptions<Producto>? queryOptions = null);
        Task<IEnumerable<ImagenesProductoDTO>> GetImagenesByProducto(int id);
    }
}