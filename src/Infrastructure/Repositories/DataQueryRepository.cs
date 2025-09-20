using Application.DTOs.DataQuery;
using Application.Interfaces.Repositories; 
using Domain.Entities.DataQuery;
using Infrastructure.Persistence;

using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq; 

namespace Infrastructure.Repositories
{
    public class DataQueryRepository : IDataQueryRepository
    {
        private readonly ApplicationDbContext _context;

        public DataQueryRepository(ApplicationDbContext context) {
            _context = context;
        }
        
        public async Task<IReadOnlyList<vUsuariosInactivosDTO>> ObtenerUsuariosInactivos(int page = 1, int pageSize = 100)
        { 
            if (page < 1) page = 1;
            var skip = (page - 1) * pageSize;

            return await _context.v_UsuariosInactivos
                .AsNoTracking()
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new vUsuariosInactivosDTO(
                    x.idUsuario,
                    x.apellidos,
                    x.correo,
                    x.nombre,
                    x.rol,
                    x.ultimaConexion,
                    x.diasInactivo
                )).ToListAsync(); 
        }

        public async Task<IReadOnlyList<vActividadUsuariosDTO>> ObtenerActividadUsuarios(int page = 1, int pageSize = 100)
        {
            if (page < 1) page = 1;
            var skip = (page - 1) * pageSize;

            return await _context.v_ActividadUsuarios
                .AsNoTracking()
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new vActividadUsuariosDTO(
                    x.idUsuario,
                    x.apellidos,
                    x.correo,
                    x.nombre,
                    x.region,
                    x.pais,
                    x.ultimaConexion,
                    x.totalLogins
                )).ToListAsync();
        }

        public async Task<IReadOnlyList<vUsuariosDispositivosDTO>> ObtenerUsuariosDispositivos(int page = 1, int pageSize = 100)
        {
            if (page < 1) page = 1;
            var skip = (page - 1) * pageSize;

            return await _context.v_UsuariosDispositivos
                .AsNoTracking()
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new vUsuariosDispositivosDTO(
                    x.idUsuario,
                    x.apellidos,
                    x.correo,
                    x.nombre,
                    x.browser,
                    x.plataforma,
                    x.sistemaOperativo,
                    x.tipoDispositivo,
                    x.modeloDispositivo
                )).ToListAsync();
        }

        public async Task<IReadOnlyList<vUsuariosIdiomasDTO>> ObtenerUsuariosIdiomas(int page = 1, int pageSize = 100)
        {
            if (page < 1) page = 1;
            var skip = (page - 1) * pageSize;

            return await _context.v_UsuariosIdiomas
                .AsNoTracking()
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new vUsuariosIdiomasDTO(
                    x.idUsuario,
                    x.apellidos,
                    x.correo,
                    x.nombre,
                    x.idioma
                )).ToListAsync();
        }

        public async Task<IReadOnlyList<vRolesUsuariosDTO>> ObtenerRolesUsuarios(int page = 1, int pageSize = 100)
        {
            if (page < 1) page = 1;
            var skip = (page - 1) * pageSize;

            return await _context.v_RolesUsuarios
                .AsNoTracking()
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new vRolesUsuariosDTO(
                    x.rol,
                    x.rolUsage,
                    x.usuariosCount
                )).ToListAsync();
        }

        public async Task<IReadOnlyList<vVisitasTipoDispositivoDTO>> ObtenerVisitasTipoDispositivo(int page = 1, int pageSize = 100)
        {
            if (page < 1) page = 1;
            var skip = (page - 1) * pageSize;

            return await _context.v_VisitasTipoDispositivo
                .AsNoTracking()
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new vVisitasTipoDispositivoDTO(
                    x.semana,
                    x.tipoDispositivo,
                    x.visitas
                )).ToListAsync();
        }

        public async Task<IReadOnlyList<vTotalUltimasTransaccionesDTO>> ObtenerTotalUltimasTransacciones(int page = 1, int pageSize = 100)
        {
            if (page < 1) page = 1;
            var skip = (page - 1) * pageSize;

            return await _context.v_TotalUltimasTransacciones
                .AsNoTracking()
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new vTotalUltimasTransaccionesDTO(
                    x.dia,
                    x.totalBienvenida,
                    x.totalCompletarPerfil,
                    x.totalQR,
                    x.totalEvento,
                    x.totalTransacciones
                )).ToListAsync();
        }
    }
}