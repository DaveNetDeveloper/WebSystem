using Application.DTOs.DataQuery;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System.Collections;
using System.Data.SqlTypes;
using System.Text.Json;
using static Application.Services.DataQueryService;
using static Utilities.ExporterHelper; 

namespace Application.Services
{
    public class SegmentoService : ISegmentoService
    {
        private readonly ISegmentoRepository _repo;
        private readonly IUsuarioSegmentosRepository _repoUsuarioSegmentos;
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;

        public SegmentoService(ISegmentoRepository repo, 
                               IUsuarioSegmentosRepository repoUsuarioSegmentos,
                               IExcelExporter excelExporter,
                               IExporter pdfExporter)
        {
            _repo = repo;
            _repoUsuarioSegmentos = repoUsuarioSegmentos;
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

        public Task<IEnumerable<Segmento>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<IEnumerable<Segmento>> GetByFiltersAsync(SegmentoFilters filters,
                                                             IQueryOptions<Segmento>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<Segmento?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public Task<bool> AddAsync(Segmento segmento)
            => _repo.AddAsync(segmento);

        public Task<bool> UpdateAsync(Segmento segmento)
             => _repo.UpdateAsync(segmento);

        public Task<bool> Remove(int id)
              => _repo.Remove(id);

        public Task<IEnumerable<Usuario>> GetUsuariosBySegmento(int idSegmento) 
            => _repoUsuarioSegmentos.GetUsuariosBySegmento(idSegmento);

        //
        // Lógica de negocio para aplicar las segmentaciones a un usuario
        //
        public void ApplySegmentsForUser(vAllUserDataDTO usuario)
        {
            if (null== usuario.IdUsuario) return;

            var allSegments = _repo.GetAllAsync().Result; 
            foreach (var segment in allSegments) {
                try {
                    // Creamos el objeto ReglaSegmento con los datos de la regla del segmento
                    var regla = new ReglaSegmento { Campo = segment.campoRegla, 
                                                    Operador = segment.operadorRegla, 
                                                    Valor = segment.valorRegla };

                    // Comprobamos si la relacion ya existe
                    var userSegment = _repoUsuarioSegmentos.GetSegmentoByIdUsuario(usuario.IdUsuario.Value, segment.id);

                    // Comparar datos de usuario segun los datos de la regla
                    if (AplicaReglaAUsuario(usuario, regla)) {
                         
                        // creamos objeto para la nueva relacion [UsuarioSegmentos]
                        var usuarioSegmento = new UsuarioSegmentos {
                            idUsuario = usuario.IdUsuario.Value,
                            idSegmento = segment.id,
                            fecha = DateTime.UtcNow
                        };
                         
                        if (null == userSegment) { // Si no existe, creamos la relacion
                            var addResult = _repoUsuarioSegmentos.AddUsuarioSegmento(usuarioSegmento);
                        }
                        else { // SI existe, actualizamos la fecha de la relacion
                            var updateResult = _repoUsuarioSegmentos.UpdateUsuarioSegmento(usuarioSegmento); 
                        }
                    }
                    else { // si no aplia y existia, eliminamos la relacion

                        if (null != userSegment) { 
                            var deleteResult = _repoUsuarioSegmentos.RemoveUsuarioSegmento(usuario.IdUsuario.Value, segment.id);
                        }
                    }
                }
                catch {
                    throw;
                }
            }
        }

        /// <summary>
        /// Método para evaluar si una regla se aplica a un usuario concreto
        /// </summary>
        /// <param name="usuario"> Usuario representado a través del DTO [vAllUserDataDTO] </param>
        /// <param name="regla"> Regla del segmento con logica a evaluar </param>
        /// <returns> bool </returns>
        public bool AplicaReglaAUsuario(vAllUserDataDTO usuario, ReglaSegmento regla)
        {
            // Obtener el valor del usuario según el campo de la regla
            var prop = typeof(vAllUserDataDTO).GetProperty(regla.Campo);
            if (prop == null) return false;

            var valorUsuario = prop.GetValue(usuario); 
            return regla.EvaluarRegla(valorUsuario);
        }

        //
        // CRUD para la tabla [UsuarioSegmentos]
        //
        public Task<IEnumerable<UsuarioSegmentos>> GetUsuariosSegmentosAllAsync()
            => _repoUsuarioSegmentos.GetUsuariosSegmentosAllAsync();

        public UsuarioSegmentos GetSegmentoByIdUsuarioAsync(int idUsuario, int idSegmento)
            => _repoUsuarioSegmentos.GetSegmentoByIdUsuario(idUsuario, idSegmento);

        public bool AddUsuarioSegmentoAsync(UsuarioSegmentos usuarioSegmentos)
            => _repoUsuarioSegmentos.AddUsuarioSegmento(usuarioSegmentos);

        public bool UpdateUsuarioSegmentoAsync(UsuarioSegmentos usuarioSegmentos)
             => _repoUsuarioSegmentos.UpdateUsuarioSegmento(usuarioSegmentos);

        public bool RemoveUsuarioSegmento(int idUsuario, int idSegmento)
              => _repoUsuarioSegmentos.RemoveUsuarioSegmento(idUsuario, idSegmento);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataQueryType"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportarAsync(ExportFormat formato) // TODO por implementar en todas las entidades exportables
        {
            Type entityType = typeof(Segmento);
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