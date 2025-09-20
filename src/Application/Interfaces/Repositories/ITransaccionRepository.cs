using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Common;

namespace Application.Interfaces.Repositories
{
    public interface ITransaccionRepository : IRepository<Transaccion, int>
    {
        Task<IEnumerable<Transaccion>> GetByFiltersAsync(IFilters<Transaccion> filters, IQueryOptions<Transaccion>? options = null);

        //Task<Transaccion?> GetByNameAsync(string name);
        //Task<IEnumerable<Transaccion>> ObtenerPorUsuarioAsync(int idUsuario);
        //Task<IEnumerable<Transaccion>> ObtenerPorProductoAsync(int idProducto);
    }
}