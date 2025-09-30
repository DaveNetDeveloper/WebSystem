using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Controllers; 
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class InAppNotificationController : BaseController<InAppNotification>, 
                                               IController<IActionResult, InAppNotification, Guid>
    { 
        private readonly IInAppNotificationService _inAppNotificationService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inAppNotificationService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public InAppNotificationController(ILogger<InAppNotificationController> logger, 
                                           IInAppNotificationService inAppNotificationService)
        {
            _logger = logger;
            _inAppNotificationService = inAppNotificationService ?? throw new ArgumentNullException(nameof(inAppNotificationService));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("ObtenerInAppNotifications")]
        public async Task<IActionResult> GetAllAsync()
        {
            var inAppNotifications = await _inAppNotificationService.GetAllAsync();
            return (inAppNotifications != null && inAppNotifications.Any()) ? Ok(inAppNotifications) : NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="descending"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [Authorize]
        [HttpGet("FiltrarInAppNotifications")]
        public async Task<IActionResult> GetByFiltersAsync([FromQuery] IFilters<InAppNotification> filters,
                                                           [FromQuery] int? page,
                                                           [FromQuery] int? pageSize,
                                                           [FromQuery] string? orderBy,
                                                           [FromQuery] bool descending = false) {
            var _filters = filters as InAppNotificationFilters;

            if (_filters is null)
                throw new InvalidOperationException("El filtro recibido no es 'InAppNotificationFilters'.");
            //else
            //_filters = ((InAppNotificationFilters)(IFilters<InAppNotification>)filters);

            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filtered = await _inAppNotificationService.GetByFiltersAsync(_filters, queryOptions);
            return Ok(filtered);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inAppNotification"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("CrearInAppNotification")]
        public async Task<IActionResult> AddAsync([FromBody] InAppNotification inAppNotification)
        {
            var nuevaInAppNotification = new InAppNotification
            {
                id = inAppNotification.id,
                idUsuario = inAppNotification.idUsuario,
                inAppNotificationType = inAppNotification.inAppNotificationType,
                fechaCreacion = DateTime.UtcNow,
                activo = inAppNotification.activo,
                titulo = inAppNotification.titulo,
                mensaje = inAppNotification.mensaje
            }; 

            var result = await _inAppNotificationService.AddAsync(nuevaInAppNotification);
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("InAppNotification:Crear", "Success"));
                return Ok(result);
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inAppNotification"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("ActualizarInAppNotification")]
        public async Task<IActionResult> UpdateAsync([FromBody] InAppNotification inAppNotification)
        {
            var result = await _inAppNotificationService.UpdateAsync(inAppNotification); 
            if (result == false) return NotFound();
            else {
                _logger.LogInformation(MessageProvider.GetMessage("InAppNotification:Actualizar", "Success"));
                return Ok(result);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        { 
            try {
                var result = await _inAppNotificationService.Remove(id);
                if (result == false) return NotFound();
                else {
                    _logger.LogInformation(MessageProvider.GetMessage("InAppNotification:Eliminar", "Success"));
                    return Ok(result);
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error eliminando la InAppNotification, {id}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("InAppNotification:Eliminar", "Error"), id });
            }  
        }
    }
}