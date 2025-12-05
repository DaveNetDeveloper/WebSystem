using Application.DTOs.Filters;
using Application.DTOs.Requests;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System.Collections;
using Utilities;
using static Application.Services.DataQueryService;
using static Utilities.ExporterHelper;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService 
    {
        private readonly IUsuarioRepository _repo;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;

        public UsuarioService(IUsuarioRepository repo,
                              IExcelExporter excelExporter,
                              IExporter pdfExporter)
        {
            _repo = repo;
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

        public Task<IEnumerable<Usuario>> GetByFiltersAsync(UsuarioFilters filters,
                                                            IQueryOptions<Usuario>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions); 

        public Task<IEnumerable<Usuario>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<Usuario?> GetByIdAsync(int id) 
            => _repo.GetByIdAsync(id);

        public Task<bool> ActivarSuscripcion(string email)
           => _repo.ActivarSuscripcion(email);
        
        public async Task<bool> CambiarContrasena(string email, string nuevaContrasena) {
            nuevaContrasena = PasswordHelper.HashPassword(nuevaContrasena);
            return await _repo.CambiarContrasena(email, nuevaContrasena); 
        }   

        public async Task<bool> AddAsync(Usuario usuario) {
            usuario.contrasena = PasswordHelper.HashPassword(usuario.contrasena);
            return await _repo.AddAsync(usuario);
        } 

        public Task<bool> UpdateAsync(Usuario usuario)
            => _repo.UpdateAsync(usuario);

        public Task<bool> Remove(int id)
              => _repo.Remove(id);

        public Task<List<Rol>> GetRolesByUsuarioId(int idUsuario)
             => _repo.GetRolesByUsuarioId(idUsuario);

        public Task<List<Direccion>> GetDireccionesByUsuario(int idUsuario)
            => _repo.GetDireccionesByUsuario(idUsuario);

        public Task<bool> AddRoleAsync(int idUsuario, Guid idRol)
            => _repo.AddRoleAsync(idUsuario, idRol);

        public async Task BajaLogicaAsync(int idUsuario)
            => _repo.BajaLogicaAsync(idUsuario);

        public Task<bool> CompletarPerfil(CompleteProfleRequest completeProfileDTO)
            => _repo.CompletarPerfil(completeProfileDTO);

        //
        // JOBS
        //  

        /// <summary>
        /// 
        /// </summary>
        /// <returns> IEnumerable<Usuario> </returns>
        public Task<IEnumerable<Usuario>> CheckUnsubscribedUsers()
           => _repo.CheckUnsubscribedUsers();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataQueryType"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportarAsync(ExportFormat formato) // TODO por implementar en todas las entidades exportables
        {
            Type entityType = typeof(Usuario);
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