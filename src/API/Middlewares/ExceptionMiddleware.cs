using Domain.Entities;
using Application.Interfaces.Services;

using System.Net;
using System.Text.Json;

namespace API.Middlewares
{ 
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, 
                                   ILogger<ExceptionMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context,
                                      ILogService logService) {
            try {
                //logService.AddAsync(new Log { 
                //        tipoLog = Log.TipoLog.Info,
                //        proceso = Log.Proceso.API_Middleware,
                //        titulo = "Inicio de petición",
                //        detalle = $"Iniciando petición {context.Request.Method} {context.Request.Path}",
                //        fecha = DateTime.UtcNow
                //});
                await _next(context);
            }
            catch (Exception ex) {
                logService.AddAsync(new Log {
                        tipoLog = Log.TipoLog.Error,
                        proceso = Log.Proceso.API_Middleware,
                        titulo = "Error en petición",
                        detalle = $"{context.Request.Method} {context.Request.Path}: {ex.Message}",
                        fecha = DateTime.UtcNow
                });

                _logger.LogError(ex, "Ocurrió un error en la petición {Path}", context.Request.Path);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new { error = "Ocurrió un error inesperado" };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}