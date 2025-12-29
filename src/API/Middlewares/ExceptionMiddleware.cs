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
            catch (Exception ex)
            {
                logService.AddAsync(new Log
                {
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
            //catch (Exception ex)
            //{
            //    // 1. Loggear la excepción (lo haces correctamente)
            //    logService.AddAsync(new Log
            //    {
            //        tipoLog = Log.TipoLog.Error,
            //        proceso = Log.Proceso.API_Middleware,
            //        titulo = "Error en petición",
            //        detalle = $"{context.Request.Method} {context.Request.Path}: {ex.Message}",
            //        fecha = DateTime.UtcNow
            //    });

            //    _logger.LogError(ex, "Ocurrió un error en la petición {Path}", context.Request.Path);

            //    // 2. Establecer el código de estado (Interno, solo para la respuesta actual)
            //    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //    // 3. Realizar la Redirección
            //    // Si quieres que el NAVEGADOR cambie de página (es decir, salir de la llamada API y cargar HTML):

            //    if (!context.Response.HasStarted) // Importante: solo redirige si no se ha enviado nada
            //    {
            //        // Usar 307 Temporary Redirect para POST/PUT/DELETE si es posible, o 302 Found
            //        // Si quieres redirigir a una página HTML de error:
            //        context.Response.Redirect("/error.html", permanent: false);

            //        // Finaliza el pipeline aquí para evitar más procesamiento
            //        return;
            //    }

            //    // Si la respuesta YA comenzó (lo cual es raro en tu caso, pero posible), 
            //    // no puedes redirigir. En ese caso, puedes intentar escribir un mensaje simple.
            //    if (!context.Response.HasStarted)
            //    {
            //        context.Response.ContentType = "text/plain";
            //        await context.Response.WriteAsync("Error inesperado del servidor.");
            //    }
            //}
            //catch (Exception ex) {
            //    logService.AddAsync(new Log {
            //            tipoLog = Log.TipoLog.Error,
            //            proceso = Log.Proceso.API_Middleware,
            //            titulo = "Error en petición",
            //            detalle = $"{context.Request.Method} {context.Request.Path}: {ex.Message}",
            //            fecha = DateTime.UtcNow
            //    });

            //    _logger.LogError(ex, "Ocurrió un error en la petición {Path}", context.Request.Path);
            //    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //    context.Response.ContentType = "application/json";

            //    var response = new { error = "Ocurrió un error inesperado" };
            //    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            //}
        }
    }
}