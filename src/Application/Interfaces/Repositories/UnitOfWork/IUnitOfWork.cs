using Domain.Entities;
using Application.Interfaces.Repositories;

namespace Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {  
        IUsuarioRepository UsuariosRepository { get; }
        ITransaccionRepository TransaccionesRepository { get; }
        IActividadRepository ActividadRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        IEntidadRepository EntidadRepository { get; }
        IFAQRepository FAQRepository { get; }
        IProductoRepository ProductoRepository { get; }
        IQRRepository QRRepository { get; }
        IRolRepository RolRepository { get; }
        ITestimonioRepository TestimonioRepository { get; }
        ITipoEntidadRepository TipoEntidadRepository { get; } 
        ITokenRepository TokenRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}