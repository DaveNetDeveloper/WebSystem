using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Messaging;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using static Domain.Entities.TipoEnvioCorreo;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Application.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public class NotificationProcessor : INotificationProcessor
    {
        private readonly ILogger<NotificationProcessor> _logger;
        private readonly ICorreoService _correoService;
        private readonly ISmsService _smsService;
        private readonly IUsuarioService _usuarioservice;

        public NotificationProcessor(ILogger<NotificationProcessor> logger,
                                     ICorreoService correoService,
                                     ISmsService smsService,
                                     IUsuarioService usuarioservice) {
            _correoService = correoService;
            _smsService = smsService;
            _usuarioservice = usuarioservice;
            _logger = logger;
        }

        public async Task ProcessAsync(NotificationRequest message)
        {
            try
            { 
                _logger.LogInformation("Procesando mensaje de notificación: {Message}", JsonSerializer.Serialize(message));

                int idUsuario;
                if (string.IsNullOrEmpty(message.Recipient) || !Int32.TryParse(message.Recipient, out idUsuario)) {
                    throw new InvalidDataException(); 
                }

                string email = string.Empty;
                string telefono = string.Empty;
                string nombre = string.Empty;

                var usuario = await _usuarioservice.GetByIdAsync(idUsuario);
                if (usuario != null)
                {
                    email = usuario.correo;
                    telefono = usuario.telefono;
                    nombre = usuario.nombre;
                }

                // lo tratamos segun el tipo de notificacion
                switch (message.Type.ToLowerInvariant())
                {
                    case NotificationTypes.Email:

                        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(nombre)) {
                            throw new InvalidDataException();
                        }
                        
                        // Envío de correo electrónico 
                        var tipoEnvio = await _correoService.ObtenerTipoEnvioCorreo(TipoEnvioCorreos.Undefined);

                        var contextEnvio = new EnvioUndefinedEmailContext(email: email,
                                                                          nombre: nombre);
                        var correo = new CorreoN {
                            Destinatario = contextEnvio.Email,
                            Asunto = tipoEnvio.asunto,
                            Cuerpo = tipoEnvio.cuerpo
                        };

                        correo.ApplyTags(contextEnvio.GetTags());
                        _correoService.EnviarCorreo_Nuevo(correo);

                        _logger.LogInformation("Correo electrónico enviado a {Recipient}", message.Recipient);
                        break;

                    case NotificationTypes.SMS:

                        if (string.IsNullOrEmpty(telefono)) { // aplicar validacion de [FormatValidationHelper.IsValidPhone]
                            throw new InvalidDataException(); // probar con OperationCanceledException() para el reintento del mensaje
                        }

                        //var sms = new SmsDTO();
                        var request = new SmsRequest() {
                            To = telefono,
                            Message = message.Message
                        }; 
                        _smsService.SendAsync(request.To, request.Message);

                        _logger.LogInformation("SMS enviado a {Recipient}", message.Recipient);
                        break;

                    case NotificationTypes.Push:

                        throw new NotImplementedException(); // TODO

                        _logger.LogInformation("Push notification envianda a {Recipient}", message.Recipient);
                        break;

                    default:
                        _logger.LogWarning("Tipo de notificación desconocido: {Type}", message.Type);
                        break;
                }
                await Task.CompletedTask;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Procesamiento cancelado para {Recipient}", message.Recipient);
                throw; // dejar que el consumidor decida ack/nack/requeue
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando notificación para {Recipient}", message.Recipient);
                throw; // rethrow para que el consumidor pueda nack/requeue o mover a DLQ
            }
        }
    }
}