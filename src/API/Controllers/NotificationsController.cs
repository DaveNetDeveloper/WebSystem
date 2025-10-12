using Application.DTOs.Requests; 
using Domain.Entities;
using Application.Messaging.Handler;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationRequestHandler _handler;
        private readonly MQServiceConfiguration _config = null;

        public NotificationsController(NotificationRequestHandler handler,
                                       IOptions<MQServiceConfiguration> options) {
             
            _config = options.Value ?? throw new ArgumentNullException(nameof(options));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost("EnviarNotificacion")]
        public async Task<IActionResult> EnviarNotificacion([FromBody] NotificationRequest request)
        {
            await _handler.HandleAsync(request, _config.QueueName);
            return Ok("Notificación publicada en RabbitMQ");
        }
    }
}