using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using static Domain.Entities.Accion;
using Utilities;

using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AccionService : IAccionService
    {
        private readonly IAccionRepository _repo;
        private readonly ICorreoService _correoService;
        private readonly IUsuarioService _usuarioService;
        private readonly MailConfiguration _appConfig; 
        private int _idUsuario;
        private int _idCampana;

        /// <summary>
        /// Contructor
        /// </summary>
        public AccionService(IAccionRepository repo,
                             ICorreoService correoService,
                             IUsuarioService usuarioService,
                             IOptions<MailConfiguration> configOptions) {
            _repo = repo;
            _correoService = correoService;
            _usuarioService = usuarioService;
            _appConfig = configOptions.Value;
        }

        public Task<IEnumerable<Accion>> GetAllAsync()
            => _repo.GetAllAsync();
        
        public Task<Accion?> GetByIdAsync(Guid id)
          => _repo.GetByIdAsync(id);

        public Task<IEnumerable<Accion>> GetByFiltersAsync(AccionFilters filters,
                                                           IQueryOptions<Accion>? queryOptions = null)
         => _repo.GetByFiltersAsync(filters, queryOptions);

        public Task<bool> AddAsync(Accion accion)
            => _repo.AddAsync(accion);

        public Task<bool> UpdateAsync(Accion accion)
             => _repo.UpdateAsync(accion);

        public Task<bool> Remove(Guid id)
              => _repo.Remove(id);


        /// <summary> 
        /// Clase estática que define las constantes para el tipo de frecuencia de una una campaña <summary>
        /// </summary> 
        /// <param name="acciones"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idCampana"></param>
        /// <returns> bool  </returns>
        public async Task<bool> EjecutarAccionesForUser(IEnumerable<Accion> acciones, 
                                                        int idUsuario, 
                                                        int idCampana)  {
            _idUsuario = idUsuario;
            _idCampana = idCampana;
            var result = false;

            foreach (var accion in acciones) {

                if (accion == null || !accion.activo) // Si cualquier accion es null o la accion no esta activa, no se ejecuta ninguna
                    return false; // salimos

                switch (accion.tipoAccion)
                {
                    case Accion.TipoAccion.EnvioComunicacion:
                        result = result == result & EjecutarEnvioComunicacion(accion);
                        break;

                    case Accion.TipoAccion.ManipulacionDatos:
                        result = result == result & EjecutarManipulacionDatos(accion);
                        break;

                    case Accion.TipoAccion.CrearContenido:
                        result = result == result & EjecutarCrearContenidoDinamico(accion);
                        break;

                    default:
                        result = false;
                        break;
                } 
            } 
            return result;
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accion"></param>
        private bool EjecutarEnvioComunicacion(Accion accion)
        {
            Accion.AccionDetalle detallesAccion = null; 

            accion.CampanaAcciones.Where(ca => ca.idAccion == accion.id)
                                  .ToList()
                                  .ForEach(ca   => detallesAccion = JsonSerializer.Deserialize<Accion.AccionDetalle>(ca.accionDetalle));

            var ca = accion.CampanaAcciones.SingleOrDefault(ca => ca.idAccion == accion.id && ca.idCampana == _idCampana);

            if (ca != null && accion.nombre != null) 
                detallesAccion = JsonSerializer.Deserialize<Accion.AccionDetalle>(ca.accionDetalle);

            switch(accion.nombre)
            {
                    case Accion.NombreAccion.EnvioEmail:

                        Console.WriteLine($"[Accion] Enviando email al usuario {_idUsuario}");
                        var tipoEnvioCorreo = _correoService.ObtenerTiposEnvioCorreo()
                                                            .Result
                                                            .Where(u => u.nombre == accion.tipoAccion).Single();
                        // obtener datos del usuario
                        var usuario = _usuarioService.GetByIdAsync(_idUsuario).Result;
                        var correo = new Correo(tipoEnvioCorreo, usuario.correo, usuario.nombre, _appConfig.LogoURL);
                        var emailToken = _correoService.EnviarCorreo(correo);

                        return true;
                    break;
                    case Accion.NombreAccion.EnvioSMS:

                        Console.WriteLine($"[Accion] Enviando SMS al usuario {_idUsuario}");
                        // Enviar sms
                        return true;
                    break;
                    case Accion.NombreAccion.EnvioPush:

                        Console.WriteLine($"[Accion] Enviando notificación push al usuario {_idUsuario}");
                        // Enviar push notification
                        return true;
                    break;
                    case Accion.NombreAccion.EnvioInApp:

                        Console.WriteLine($"[Accion] Enviando mensaje In-App al usuario {_idUsuario}");
                        // Guardar mensaje In-App en Base de datos
                        return true;
                    break;
                    default:
                        Console.WriteLine($"[Accion] Tipo de comunicación no reconocido");
                        return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accion"></param> 
        /// <returns> bool  </returns>
        private bool EjecutarManipulacionDatos(Accion accion)
        {
            // _idUsuario 
            switch (accion.nombre)
            {
                case Accion.NombreAccion.Activacion:

                    Console.WriteLine($"[Accion] Activando al usuario {_idUsuario}");
                    

                    return true;
                    break; 

                default:
                    Console.WriteLine($"[Accion] Tipo de manipulación no reconocido");
                    return false;
            } 
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accion"></param>
        /// <returns> bool  </returns>
        private bool EjecutarCrearContenidoDinamico(Accion accion)
        {
            // _idUsuario 
            switch (accion.nombre)
            {
                case Accion.NombreAccion.GenerarRecompensa:

                    Console.WriteLine($"[Accion] Generando recompensa al usuario {_idUsuario}");


                    return true;
                    break;

                default:
                    Console.WriteLine($"[Accion] Tipo de creación de contenido dinamica no reconocido");
                    return false;
            }
            return true;
        }
    }
}