using Application.DTOs.Filters; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;

namespace Application.Services
{
    public class TransaccionService : ITransaccionService
    {
        private readonly ITransaccionRepository _repo;
        private readonly IUsuarioRepository _repoUsuario;
        private readonly IUnitOfWork _unitOfWork;

        public TransaccionService(ITransaccionRepository repo, 
                                  IUsuarioRepository repoUsuario, 
                                  IUnitOfWork unitOfWork) {
            _repo = repo;

            _repoUsuario = repoUsuario;
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Transaccion>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<IEnumerable<Transaccion>> GetByFiltersAsync(TransaccionFilters filters,
                                                            IQueryOptions<Transaccion>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<Transaccion?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public async Task<bool> AddAsync(Transaccion transaccion)
        {
            await _unitOfWork.BeginTransactionAsync();
            try {

                await _unitOfWork.TransaccionesRepository.AddAsync(transaccion);
                var transactionUser = await _unitOfWork.UsuariosRepository.GetByIdAsync(transaccion.idUsuario);

                await _unitOfWork.UsuariosRepository.ActualizarBalance(transaccion.idUsuario, transaccion.puntos);
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception) {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public Task<bool> UpdateAsync(Transaccion transaccion)
        {
            //
            // TODO Pendiente aplicar UnitOfWork
            // 
            var result = _repo.UpdateAsync(transaccion);
            if (result.Result) {

                // TODO : solo si se ha actualizado el campo puntos
                //_repoUsuario.ActualizarBalance(transaccion.idUsuario, transaccion.puntos); 
            }
            return result;
        }

        public async Task<bool> Remove(int id)
        {
            var transaccion = await _unitOfWork.TransaccionesRepository.GetByIdAsync(id);
            if (transaccion == null) return false;

            var transactionUser = await _unitOfWork.UsuariosRepository.GetByIdAsync(transaccion.idUsuario);
            await _unitOfWork.TransaccionesRepository.Remove(id);
            await _unitOfWork.UsuariosRepository.ActualizarBalance(transactionUser.id.Value, -transaccion.puntos);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
    }
}