using Application.DTOs.Filters; 
using Application.Interfaces.Common;
using Domain.Entities;
using static Utilities.ExporterHelper;

namespace Application.Interfaces.Services
{
    public interface IRecompensaService : IService<Recompensa, int>
    {
        Task<IEnumerable<Recompensa>> GetByFiltersAsync(RecompensaFilters filters,
                                                        IQueryOptions<Recompensa>? queryOptions = null);


        Task<IEnumerable<Recompensa>> GetRecompensasByUsuario(int idUsuario);
        Task<int> GenerarRecompensa(int idUsuario, TipoRecompensa tipoRecompensa);



        Task<IEnumerable<UsuarioRecompensa>> GetUsuariosRecompensasAllAsync();
        Task<UsuarioRecompensa> GetUsuarioRecompensaAsync(int idUsuario, int idRecompensa);
        Task<bool> AddUsuarioRecompensaAsync(UsuarioRecompensa usuarioRecompensa);
        Task<bool> UpdateUsuarioRecompensaAsync(UsuarioRecompensa usuarioRecompensa);
        Task<bool> RemoveUsuarioRecompensa(int idUsuario, int idRecompensa);

        Task<IEnumerable<TipoRecompensa>> GetAllTiposRecompensas();
        Task<TipoRecompensa> GetTipoRecompensa(Guid idTipoRecompensa);
        Task<bool> AddTipoRecompensa(TipoRecompensa tipoRecompensa);
        Task<bool> UpdateTipoRecompensa(TipoRecompensa tipoRecompensa);
        Task<bool> RemoveTipoRecompensa(Guid idTipoRecompensa);
        Task<byte[]> ExportarAsync(ExportFormat formato);
    }
}