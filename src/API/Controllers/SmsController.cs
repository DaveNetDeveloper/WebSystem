using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Application.DTOs.Requests;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SmsController : ControllerBase 
    {
        private readonly ISmsService _smsService;

        public SmsController(ISmsService smsService)
                             //ILogger<SmsController> logger)
        { 
            //_logger = logger;
            _smsService = smsService ?? throw new ArgumentNullException(nameof(smsService));
        }

        /// <summary>
        /// Envia un sms 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        // [Authorize]
        [HttpPost("EnviarSMS")]
        public async Task<IActionResult> EnviarSMS([FromBody] SmsRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.To) || string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Número o mensaje vacío.");

            await _smsService.SendAsync(request.To, request.Message);
            return Ok("SMS enviado correctamente");
        } 
    } 
}