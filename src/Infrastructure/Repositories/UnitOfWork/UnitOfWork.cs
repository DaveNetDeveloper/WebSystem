using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

using Application.Interfaces.Repositories;  
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Repositories.UnitOfWork;

namespace Infrastructure.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        public IUsuarioRepository UsuariosRepository { get; }
        public ITransaccionRepository TransaccionesRepository { get; }
        public IActividadRepository ActividadRepository { get; }
        public ICategoriaRepository CategoriaRepository { get; }
        public IEntidadRepository EntidadRepository { get; }
        public IFAQRepository FAQRepository { get; }
        public IProductoRepository ProductoRepository { get; }
        public IQRCodeRepository QRRepository { get; }
        public IRolRepository RolRepository { get; }
        public ITestimonioRepository TestimonioRepository { get; }
        public ITipoEntidadRepository TipoEntidadRepository { get; } 
        public UnitOfWork(  ApplicationDbContext context,
                            IUsuarioRepository usuariosRepository,
                            ITransaccionRepository transaccionesRepository,
                            IActividadRepository actividadRepository,
                            ICategoriaRepository categoriaRepository,
                            IEntidadRepository entidadRepository,
                            IFAQRepository faqRepository,
                            IProductoRepository productoRepository,
                            IQRCodeRepository qrRepository,
                            IRolRepository rolRepository,
                            ITestimonioRepository testimonioRepository,
                            ITipoEntidadRepository tipoEntidadRepository) {

            _context = context;

            UsuariosRepository = usuariosRepository;
            TransaccionesRepository = transaccionesRepository;
            ActividadRepository = actividadRepository;
            CategoriaRepository = categoriaRepository;
            EntidadRepository = entidadRepository;
            FAQRepository = faqRepository;
            ProductoRepository = productoRepository;
            QRRepository = qrRepository;
            RolRepository = rolRepository;
            TestimonioRepository = testimonioRepository;
            TipoEntidadRepository = tipoEntidadRepository; 
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
         => await _context.SaveChangesAsync(cancellationToken);

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
            => _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try { 
                await _context.SaveChangesAsync(cancellationToken); // Aplica todos los cambios pendientes
                await _currentTransaction?.CommitAsync(cancellationToken);  // Confirma la transacción
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null; 
            }
            catch {  
                await _currentTransaction?.RollbackAsync(cancellationToken);
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;  
                throw; 
            }
        } 

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            //if (_currentTransaction is not null) {
                await _currentTransaction?.RollbackAsync(cancellationToken);
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            //}
        }

        public void Dispose() => _context.Dispose();
    }
}