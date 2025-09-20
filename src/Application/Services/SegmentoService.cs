using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

using System.Text.Json;

namespace Application.Services
{
    public class SegmentoService : ISegmentoService
    {
        private readonly ISegmentoRepository _repo;
        private readonly IUsuarioSegmentosRepository _repoUsuarioSegmentos;

        public SegmentoService(ISegmentoRepository repo, 
                               IUsuarioSegmentosRepository repoUsuarioSegmentos) {
            _repo = repo;
            _repoUsuarioSegmentos = repoUsuarioSegmentos;
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

        //
        // Lógica de negocio para aplicar las segmentaciones a un usuario
        //
        public void ApplySegmentsForUser(Usuario usuario)
        {
            var allSegments = _repo.GetAllAsync().Result; 
            foreach (var segment in allSegments) {
                try {
                    // Creamos el objeto ReglaSegmento con los datos de la regla del segmento
                    var regla = new ReglaSegmento { Campo = segment.campoRegla, 
                                                    Operador = segment.operadorRegla, 
                                                    Valor = segment.valorRegla };

                    // Comprobamos si la relacion ya existe
                    var userSegment = _repoUsuarioSegmentos.GetSegmentoByIdUsuario(usuario.id.Value, segment.id);

                    // Comparar datos de usuario segun los datos de la regla
                    if (regla.AplicaReglaAUsuario(usuario, regla)) {
                         
                        // creamos objeto para la nueva relacion [UsuarioSegmentos]
                        var usuarioSegmento = new UsuarioSegmentos {
                            idUsuario = usuario.id.Value,
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
                            var deleteResult = _repoUsuarioSegmentos.RemoveUsuarioSegmento(usuario.id.Value, segment.id);
                        }
                    }
                }
                catch {
                    throw;
                }
            }
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
    }
}