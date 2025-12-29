using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ICampanaRepository : IRepository<Campana, int>
    {
        Task<IEnumerable<Campana>> GetByFiltersAsync(IFilters<Campana> filters, 
                                                     IQueryOptions<Campana>? options = null);
        //Task<IEnumerable<ProductoImagen>> GetImagenesByProducto(int id);
    }
}