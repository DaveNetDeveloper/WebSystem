using Application.DTOs.Filters; 
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System.Collections;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;
using static Application.Services.DataQueryService;
using static Utilities.ExporterHelper;

namespace Application.Services
{
    public class TransaccionService : ITransaccionService
    {
        private readonly ITransaccionRepository _repo;
        private readonly IUsuarioRepository _repoUsuario;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;

        public TransaccionService(ITransaccionRepository repo, 
                                  IUsuarioRepository repoUsuario, 
                                  IUnitOfWork unitOfWork,
                                  IExcelExporter excelExporter,
                              IExporter pdfExporter)
        {
            _repo = repo;

            _repoUsuario = repoUsuario;
            _unitOfWork = unitOfWork;
            _excelExporter = excelExporter;
            _pdfExporter = pdfExporter;
        }

        public byte[] ExportDynamic(IEnumerable data, Type entityType)
        {
            return null;
        }

        public byte[] Export<T>(IEnumerable<T> data, string sheetName)
        {
            return null;
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
                //var transactionUser = await _unitOfWork.UsuariosRepository.GetByIdAsync(transaccion.idUsuario);

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataQueryType"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportarAsync(ExportFormat formato) // TODO por implementar en todas las entidades exportables
        {
            Type entityType = typeof(Transaccion);
            IEnumerable queryResult = await GetAllAsync();

            byte[] excelBytes = null;
            switch (formato)
            {
                case ExportFormat.Excel:
                    excelBytes = _excelExporter.ExportToExcelDynamic(queryResult, entityType);
                    break;
                case ExportFormat.Pdf:
                    excelBytes = _pdfExporter.ExportDynamic(queryResult, entityType);
                    break;
            }
            return excelBytes;
        }

        public async Task<byte[]> ExportarAsync(DataQueryType dataQueryType, ExportFormat formato)
        {
            return null;
        }

    }
}