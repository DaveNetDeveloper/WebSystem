using Application.DTOs.DataQuery;
using Application.Interfaces.DTOs.DataQuery;
using Application.Interfaces.Repositories; 
using Application.Interfaces.Services;

using System.Collections; 
using System.Text.Json.Serialization;

namespace Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class DataQueryService : IDataQueryService, IExportService, IExporter
    {
        private readonly IExcelExporter _excelExporter;
        private readonly IExporter _pdfExporter;
        private readonly IDataQueryRepository _repo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="excelExporter"></param>
        public DataQueryService(IDataQueryRepository repo,
                                IExcelExporter excelExporter,
                                IExporter pdfExporter)  {
            _repo = repo; 
            _excelExporter = excelExporter;
            _pdfExporter = pdfExporter;
        }

         
        public enum ExportFormat { Excel, Pdf }

        public byte[] ExportDynamic(IEnumerable data, Type entityType) 
        {
            return null;
        }

        public byte[] Export<T>(IEnumerable<T> data, string sheetName) 
        {
            return null;
        }

        public enum DataQueryType
        { 
            UsuariosInactivos,
            ActividadUsuarios,
            UsuariosIdiomas,
            UsuariosDispositivos,
            RolesUsuarios,
            VisitasTipoDispositivo,
            TotalUltimasTransacciones,
            CampanasUsuarios,
            AllUserData,
            AllCampanasData,
            AsistenciaActividades
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataQueryType"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportarAsync(DataQueryType dataQueryType, ExportFormat formato)
        { 
            Type entityType = null;
            IEnumerable queryResult = null; 

            switch (dataQueryType)
            {
                case DataQueryType.UsuariosInactivos:

                    entityType = typeof(vUsuariosInactivosDTO);
                    queryResult = ObtenerUsuariosInactivos().Result;
                    break;
                case DataQueryType.ActividadUsuarios:

                    entityType = typeof(vActividadUsuariosDTO);
                    queryResult = ObtenerActividadUsuarios().Result;
                    break;
                case DataQueryType.UsuariosIdiomas:

                    entityType = typeof(vUsuariosIdiomasDTO);
                    queryResult = ObtenerUsuariosIdiomas().Result;
                    break;
                case DataQueryType.UsuariosDispositivos:

                    entityType = typeof(vUsuariosDispositivosDTO);
                    queryResult = ObtenerUsuariosDispositivos().Result;
                    break;
                case DataQueryType.RolesUsuarios:

                    entityType = typeof(vRolesUsuariosDTO);
                    queryResult = ObtenerRolesUsuarios().Result;
                    break;
                case DataQueryType.VisitasTipoDispositivo:

                    entityType = typeof(vVisitasTipoDispositivoDTO);
                    queryResult = ObtenerVisitasTipoDispositivo().Result;
                    break;
                case DataQueryType.TotalUltimasTransacciones:

                    entityType = typeof(vTotalUltimasTransaccionesDTO);
                    queryResult = ObtenerTotalUltimasTransacciones().Result;
                    break;
                case DataQueryType.CampanasUsuarios:

                    entityType = typeof(vCampanasUsuariosDTO);
                    queryResult = ObtenerCampanasUsuarios().Result;
                    break;
                case DataQueryType.AllUserData:

                    entityType = typeof(vAllUserDataDTO);
                    queryResult = ObtenerAllUserData().Result;
                    break;
                case DataQueryType.AllCampanasData:

                    entityType = typeof(vAllCampanasDataDTO);
                    queryResult = ObtenerAllCampanasData().Result;
                    break;
                case DataQueryType.AsistenciaActividades:

                    entityType = typeof(vAsistenciaActividadesDTO);
                    queryResult = ObtenerAsistenciaActividades().Result;
                    break;
                default:
                    throw new InvalidOperationException("Vista no reconocida");
            }

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<vUsuariosInactivosDTO>> ObtenerUsuariosInactivos(int page = 1, int pageSize = 100)
            => await _repo.ObtenerUsuariosInactivos(page, pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<vActividadUsuariosDTO>> ObtenerActividadUsuarios(int page = 1, int pageSize = 100)
            => await _repo.ObtenerActividadUsuarios(page, pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<vUsuariosIdiomasDTO>> ObtenerUsuariosIdiomas(int page = 1, int pageSize = 100)
            => await _repo.ObtenerUsuariosIdiomas(page, pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<vUsuariosDispositivosDTO>> ObtenerUsuariosDispositivos(int page = 1, int pageSize = 100)
            => await _repo.ObtenerUsuariosDispositivos(page, pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<vRolesUsuariosDTO>> ObtenerRolesUsuarios(int page = 1, int pageSize = 100)
           => await _repo.ObtenerRolesUsuarios(page, pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<vVisitasTipoDispositivoDTO>> ObtenerVisitasTipoDispositivo(int page = 1, int pageSize = 100)
           => await _repo.ObtenerVisitasTipoDispositivo(page, pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<vTotalUltimasTransaccionesDTO>> ObtenerTotalUltimasTransacciones(int page = 1, int pageSize = 100)
          => await _repo.ObtenerTotalUltimasTransacciones(page, pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<vCampanasUsuariosDTO>> ObtenerCampanasUsuarios(int page = 1, int pageSize = 100)
            => await _repo.ObtenerCampanasUsuarios(page, pageSize);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<vAllUserDataDTO>> ObtenerAllUserData(int page = 1, int pageSize = 100)
            => await _repo.ObtenerAllUserData(page, pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<vAllCampanasDataDTO>> ObtenerAllCampanasData(int page = 1, int pageSize = 100)
          => await _repo.ObtenerAllCampanasData(page, pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<vAsistenciaActividadesDTO>> ObtenerAsistenciaActividades(int page = 1, int pageSize = 100)
            => await _repo.ObtenerAsistenciaActividades(page, pageSize);
    }
}