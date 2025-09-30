using Application.DTOs.DataQuery;
using Application.Interfaces.Repositories; 
using Domain.DataQuery;
using Domain.Entities;
using Infrastructure.Persistence;

using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<IReadOnlyList<vCampanasUsuariosDTO>> ObtenerCampanasUsuarios(int page = 1, int pageSize = 100)
        {
            if (page < 1) page = 1;
            var skip = (page - 1) * pageSize;

            return await _context.v_CampanasUsuarios
                .AsNoTracking()
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new vCampanasUsuariosDTO(
                    x.idCampana,
                    x.nombreCampana,
                    x.frecuenciaCampana,
                    x.fechaEjecucion,
                    x.estadoEjecucion,
                    x.tipoCampana,
                    x.nombreSegmento,
                    x.tipoSegmento,
                    x.nombreUsuario,
                    x.correoUsuario
                )).ToListAsync();
        }

        public async Task<IReadOnlyList<vAllUserDataDTO>> ObtenerAllUserData(int page = 1, int pageSize = 100)
        {
            if (page < 1) page = 1;
            var skip = (page - 1) * pageSize;

            return await _context.v_AllUserData
                .AsNoTracking()
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new vAllUserDataDTO(
                    x.idUsuario,
                    x.nombreUsuario,
                    x.correo,
                    x.activo,
                    x.fechaCreacion,
                    x.fechaNacimiento,
                    x.suscrito,
                    x.puntos,
                    x.genero,
                    x.telefono,
                    x.edad,
                    x.codigoRecomendacion,
                    x.codigoRecomendacionRef,
                    x.totalRoles,
                    x.roles,
                    x.totalLogins,
                    x .ultimaConexion,
                    x.ultimoDispositivo,
                    x.ciudad,
                    x.provincia,
                    x.comunidadAutonoma,
                    x.pais,
                    x.totalRecompensas,
                    x.totalEntidades,
                    x.entidades,
                    x.totalSegmentos,
                    x.segmentos,
                    x.totalTransacciones,
                    x.totalTransaccionesProducto,
                    x.totalTransaccionesEvento
                )).ToListAsync();
        }

        public async Task<IReadOnlyList<vAllCampanasDataDTO>> ObtenerAllCampanasData(int page = 1, int pageSize = 100)
        {
            if (page < 1) page = 1;
            var skip = (page - 1) * pageSize;

            return await _context.v_AllCampanasData
                .AsNoTracking()
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new vAllCampanasDataDTO(
                        x.idCampana,
                        x.nombreCampana,
                        x.estadoCampana,
                        x.frecuenciaCampana,
                        x.fechaInicioCampana,
                        x.fechaFinCampana,
                        x.tipoCampana,
                        x.tipoAccion,
                        x.accion,
                        x.accionDetalle,
                        x.idSegmento,
                        x.segmento,
                        x.tipoSegmento
                )).ToListAsync();
        }
    }
}