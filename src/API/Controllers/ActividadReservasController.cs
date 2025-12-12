using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Responses;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;

using static Domain.Entities.TipoEnvioCorreo;
using static Domain.Entities.TipoTransaccion;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Utilities;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActividadReservasController : BaseController<ActividadReserva>,
                                               IController<IActionResult, ActividadReserva, Guid>
    {
        private readonly IActividadReservaService _actividadReservaService;
        private readonly IActividadService _actividadService;
        private readonly IUsuarioService _usuarioService;
        private readonly ICorreoService _correoService;
        private readonly ITipoTransaccionService _tipoTransaccionService;
        private readonly ITransaccionService _transaccionService;
        private readonly IQRCodeService _qrCodeService;
        private readonly IEntidadService _entidadService;

        public ActividadReservasController(ILogger<ActividadReservasController> logger,
                                           IActividadReservaService actividadReservaService,
                                           IActividadService actividadService,
                                           IUsuarioService usuarioService,
                                           ICorreoService correoService,
                                           ITipoTransaccionService tipoTransaccionService,
                                           ITransaccionService transaccionService,
                                           IQRCodeService qrCodeService,
                                           IEntidadService entidadService) {
            _logger = logger;
            _actividadReservaService = actividadReservaService ?? throw new ArgumentNullException(nameof(actividadReservaService));
            _actividadService = actividadService ?? throw new ArgumentNullException(nameof(actividadService));
            _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
            _correoService = correoService ?? throw new ArgumentNullException(nameof(correoService));
            _tipoTransaccionService = tipoTransaccionService ?? throw new ArgumentNullException(nameof(tipoTransaccionService));
            _transaccionService = transaccionService ?? throw new ArgumentNullException(nameof(transaccionService));
            _qrCodeService = qrCodeService ?? throw new ArgumentNullException(nameof(qrCodeService));
            _entidadService = entidadService ?? throw new ArgumentNullException(nameof(entidadService));
        }

        [Authorize]
        [HttpGet("ObtenerActividadReservas")]
        public async Task<IActionResult> GetAllAsync()
        {
            var reservas = await _actividadReservaService.GetAllAsync();
            return (reservas != null && reservas.Any()) ? Ok(reservas) : NoContent();
        }

        [Authorize]
        [HttpGet("FiltrarActividadReservas")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<ActividadReserva> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false)
        {
            var _filters = filters as ActividadReservaFilters;
            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es ActividadReservaFilters'.");

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _actividadReservaService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        [Authorize]
        [HttpGet("ObtenerActividadReserva/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var reserva = await _actividadReservaService.GetByIdAsync(id);
            return reserva != null ? Ok(reserva) : NoContent();
        }

        [Authorize]
        [HttpPost("CrearActividadReserva")]
        public async Task<IActionResult> AddAsync([FromBody] ActividadReserva actividadReserva)
        {
            var nuevaReserva = new ActividadReserva
            {
                idActividad = actividadReserva.idActividad,
                idUsuario = actividadReserva.idUsuario,
                codigoReserva = actividadReserva.codigoReserva,
                fechaReserva = actividadReserva.fechaReserva,
                fechaActividad = actividadReserva.fechaActividad,
                fechaValidacion = actividadReserva.fechaValidacion,
                estado = actividadReserva.estado
            };

            var result = await _actividadReservaService.AddAsync(nuevaReserva);
            if (result == false) return NotFound();
            else
            {
                _logger.LogInformation(MessageProvider.GetMessage("ActividadReserva:Crear", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpPut("ActualizarActividadReserva")]
        public async Task<IActionResult> UpdateAsync([FromBody] ActividadReserva actividadReserva)
        {
            var result = await _actividadReservaService.UpdateAsync(actividadReserva);
            if (result == false) return NotFound();
            else
            {
                _logger.LogInformation(MessageProvider.GetMessage("ActividadReserva:Actualizar", "Success"));
                return Ok(result);
            }
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try
            {
                var result = await _actividadReservaService.Remove(id);
                if (result == false) return NotFound();
                else
                {
                    _logger.LogInformation(MessageProvider.GetMessage("ActividadReserva:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando la reserva, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("ActividadReserva:Eliminar", "Error"), id });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="codigoReserva"></param>
        /// <returns> bool </returns>
        [HttpPost("ValidarReserva")]
        //[Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> ValidarReserva([FromQuery] string email,
                                                        [FromQuery] string codigoReserva) {
            
            var usuarioFilter = new UsuarioFilters {
                Correo = email 
            };
            var usuarios = await _usuarioService.GetByFiltersAsync(usuarioFilter);
            var usuario = usuarios.SingleOrDefault();
            if (null == usuario)
                return NotFound();

            int? idUsuario = usuario.id;

            var reservaFilter = new ActividadReservaFilters { 
                CodigoReserva = codigoReserva,
                IdUsuario = idUsuario 
            };
            var reservas = await _actividadReservaService.GetByFiltersAsync(reservaFilter);
            var reserva = reservas.SingleOrDefault();

            if (reserva == null || reserva.estado != ActividadReserva.EstadoReserva.Reservada)
                return NotFound(false);

            reserva.fechaValidacion = DateTime.UtcNow;
            reserva.estado = ActividadReserva.EstadoReserva.Validada;
            var updateResult = await _actividadReservaService.UpdateAsync(reserva);

            // Crear transaccion de tipo [Puntos por validar la asistencia a la actividad] 
            var tipoTransacciones = await _tipoTransaccionService.GetAllAsync();
            var tipoTransaccion = tipoTransacciones.Where(u => u.nombre == TiposTransaccion.ReservarActividad)
                                                   .SingleOrDefault();
            var transaccion = new Transaccion {
                idTipoTransaccion = tipoTransaccion.id,
                fecha = DateTime.UtcNow,
                puntos = tipoTransaccion.puntos,
                idUsuario = idUsuario.Value,
                nombre = tipoTransaccion.nombre,
                idActividad = reserva.idActividad
            };
            var addResult = await _transaccionService.AddAsync(transaccion);
            return Ok(updateResult && addResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="codigoReserva"></param>
        /// <returns> ReservaActividadDTO </returns>
        [AllowAnonymous]
        [HttpPost("ReservarActividad")]
        //[Authorize]
        public async Task<IActionResult> ReservarActividad([FromQuery] int idUsuario, 
                                                           [FromQuery] int idActividad)
                                                           
        {  
            var _idReserva = Guid.NewGuid();
            var _codigoReserva = CodesHelper.GenerarCodigoReservaActividad(idActividad);
            var _estadoReserva = ActividadReserva.EstadoReserva.Reservada;
            var _fechaReserva = DateTime.UtcNow;

            // Obtener entidad Actividad 
            var actividad = await _actividadService.GetByIdAsync(idActividad);

            //Comprobar que el usuario no tiene ya una reserva para la actividad
            var reservaFilter = new ActividadReservaFilters
            {
                IdActividad = idActividad,
                IdUsuario = idUsuario
            };
            var reservas = await _actividadReservaService.GetByFiltersAsync(reservaFilter);
            bool existeReserva = reservas.Any(r => r.estado != ActividadReserva.EstadoReserva.Cancelada);

            if (existeReserva) 
                return StatusCode(StatusCodes.Status500InternalServerError, "Ya existe una reereva activa para el usuario");

            // Crear la reserva en [ActividadReservas] en estado "Reservada"
            var actividadReserva = new ActividadReserva
            {
                idReserva = _idReserva,
                codigoReserva = _codigoReserva,
                idActividad = idActividad,
                idUsuario = idUsuario,
                fechaReserva = _fechaReserva,
                fechaActividad = actividad.fechaInicio.Value.ToUniversalTime(),
                estado = _estadoReserva 
            };
            var reservaResult = await _actividadReservaService.AddAsync(actividadReserva);

            // Crear QRCode 
            Guid idQRCode = Guid.NewGuid();
            string payload = $"https://localhost:7175/WebPages/loginQR.html?id={idQRCode}";

            //calcular fecha de expiracion el QR basado en la fecha fin de la actividad
            TimeSpan? ttl = null;
            if (actividad.fechaFin.HasValue) 
            {
                ttl = actividad.fechaFin.Value - DateTime.UtcNow;    // Tiempo restante hasta la fecha de fin
                if (ttl <= TimeSpan.Zero) // Evitar valores negativos por si la fecha ya ha venciudo
                    ttl = TimeSpan.FromSeconds(1);
            }
            var qrCode = await _qrCodeService.CreateAsync(payload, ttl, idQRCode);
            
            // Enviar correo al usuario informando de la reserva de la actividad
            var tiposEnvioUser = await _correoService.ObtenerTiposEnvioCorreo();
            var tipoEnvioUser = tiposEnvioUser .Where(u => u.nombre == TipoEnvio.ReservaActividad)
                                               .SingleOrDefault();

            var usuario = await _usuarioService.GetByIdAsync(idUsuario);
            var correo = new Correo(tipoEnvioUser, usuario.correo, usuario.nombre, ""); // _config["AppConfiguration:LogoURL"]
            correo.FicheroAdjunto = new FicheroAdjunto {
                                            NombreArchivo = "QR_ReservaActividad.png",
                                            ContentType = "image/png",
                                            Archivo = qrCode.imagen }; 

            _correoService.EnviarCorreo(correo);

            // Enviar correo al manager de la entidad informando de la reserva
            var tiposEnvioManager = await _correoService.ObtenerTiposEnvioCorreo();
            var tipoEnvioManager = tiposEnvioManager.Where(u => u.nombre == TipoEnvio.ReservaActividad_Manager)
                                                    .SingleOrDefault();


            //obtener el usuario manager de la entidad

            var entidadActividad  = await _entidadService.GetByIdAsync(actividad.idEntidad);
            var emailManager = entidadActividad.manager;
            var matchUsers = await _usuarioService.GetByFiltersAsync(new UsuarioFilters { Correo = emailManager });
            var manager = matchUsers.SingleOrDefault();

            var correoManager = new Correo(tipoEnvioManager, manager.correo, manager.nombre, ""); // _config["AppConfiguration:LogoURL"]
            _correoService.EnviarCorreo(correoManager);

            // Devolver los datos de la reserva desde el controller(en DTO Response)
            var reservaResponse = new ReservaActividadDTO() {
                IdReserva = _idReserva,
                IdActividad = idActividad,
                CodigoReserva = _codigoReserva,
                EstadoReserva = _estadoReserva,
                FechaReserva = _fechaReserva,
                FechaActividad = actividad.fechaInicio.Value,
                IdUsuario = idUsuario
            };
            return Ok(reservaResponse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="codigoReserva"></param>
        /// <returns> bool </returns>
        [HttpPut("CancelarReserva")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> CancelarReserva([FromQuery] string email,
                                                         [FromQuery] string codigoReserva) {

            var usuarioFilter = new UsuarioFilters {
                Correo = email
            };
            
            var usuarios = await _usuarioService.GetByFiltersAsync(usuarioFilter);
            var usuario = usuarios.SingleOrDefault();
            if (null == usuario)
                return NotFound();

            int? idUsuario = usuario.id;

            var reservaFilter = new ActividadReservaFilters {
                CodigoReserva = codigoReserva,
                IdUsuario = idUsuario
            };

            var reservas = await _actividadReservaService.GetByFiltersAsync(reservaFilter);
            var reserva = reservas.SingleOrDefault();

            if (reserva == null || 
                reserva.estado == ActividadReserva.EstadoReserva.Cancelada ||
                reserva.estado == ActividadReserva.EstadoReserva.Validada) { 
                    
                return NotFound(false);
            }
            
            reserva.estado = ActividadReserva.EstadoReserva.Cancelada;
            var updateResult = await _actividadReservaService.UpdateAsync(reserva);
             
            return Ok(updateResult);
        } 
    }
}