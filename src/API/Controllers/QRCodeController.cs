using Application.Common;
using Application.DTOs.Filters;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces.Controllers;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Domain.Entities.InAppNotification;
using static Domain.Entities.QRCode;
using static Domain.Entities.TipoEnvioCorreo;
using static Domain.Entities.TipoTransaccion;
using static Utilities.ExporterHelper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/qr")]
    public class QRCodesController : BaseController<QRCode>
    {
        private readonly IQRCodeService _service;
        private readonly ExportConfiguration _exportConfig;
        protected IConfiguration _config;

        public QRCodesController(IQRCodeService service,
                                 ILogger<QRCodesController> logger,
                                 IOptions<ExportConfiguration> options,
                                 IConfiguration config) {
            _service = service;
            _logger = logger;
            _exportConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
            _config = config;
        }

        [AllowAnonymous]
        //[Authorize]
        [HttpGet("ObtenerQRs")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var productos = await _service.GetAllAsync();
                return (productos != null && productos.Any()) ? Ok(productos) : NoContent();
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }

        [HttpPost ("CrearQR")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateQRCodeRequest request)
        {
            var qr = await _service.CreateAsync(
                request.Payload,
                request.Ttl.HasValue ? TimeSpan.FromSeconds(request.Ttl.Value) : null
            );

            return Ok(new QRCodeResponse(qr));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            var qr = await _service.GetAsync(id);
            if (qr == null) return NotFound();
            return Ok(new QRCodeResponse(qr));
        }

        [HttpGet("{id}/image")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var qr = await _service.GetAsync(id);
            if (qr == null) return NotFound();
            return File(qr.imagen!, "image/png");
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            await _service.ActivateAsync(id);
            return Ok("QR activado correctamente");
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            await _service.DeactivateAsync(id);
            return Ok("QR desactivado correctamente");
        }

        [HttpPost("{id}/consume")]
        [AllowAnonymous]
        public async Task<IActionResult> Consume([FromServices] ITransaccionService transaccionService,
                                                 [FromServices] ITipoTransaccionService tipoTransaccionService,
                                                 [FromServices] IUsuarioService usuarioService,
                                                 [FromServices] ICorreoService correoService,
                                                 [FromServices] IInAppNotificationService inAppNotificationService,
                                                 [FromServices] IProductoService productoService,
                                                 [FromServices] IPerfilService perfilService,
                                                 Guid id,
                                                 [FromQuery] string email)
        {
            var qrResult = await _service.ConsumeAsync(id);

            if (!qrResult) return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Se produjo un error al consumir el QR." });

            var filter = new UsuarioFilters();
            filter.Correo = email;
            var usuarios = await usuarioService.GetByFiltersAsync(filter);
            var usuario = usuarios.FirstOrDefault();

            // 
            var tiposTransaccion = await tipoTransaccionService.GetAllAsync();
            var tipoTransaccion = tiposTransaccion
                                  .Where(u => u.nombre == TiposTransaccion.QrProduto)
                                  .SingleOrDefault();

            // obtener datos del producto del QR para saber los puntos a sumar al usuario
            var qr = await _service.GetAsync(id);

            if (qr == null || 
                qr.origen.ToLower() != QRCode.Origen.Producto.ToLower() ||
                qr.idProducto == null) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "El QRCode tiene problemas de configuración." });
            }

            var idProducto = qr.idProducto.Value;

            var producto = await productoService.GetByIdAsync(idProducto);
            var nombreProducto = producto.nombre;  
            int puntosProducto = producto.puntos; 
              
            var transaccion = new Transaccion
            {
                idTipoTransaccion = tipoTransaccion.id,
                fecha = DateTime.UtcNow,
                puntos = puntosProducto,
                idUsuario = usuario.id.Value,
                nombre = tipoTransaccion.nombre,
                idProducto = producto.id
            };
            await transaccionService.AddAsync(transaccion);

            // Crear InAppNotificacion para que el usuario lo vea en la home privada
            var inApp = new InAppNotification
            {
                idUsuario = usuario.id.Value,
                titulo = "Has escaneado un producto!",
                mensaje = "Has ganado [" + puntosProducto + "] puntos por escanear [" + nombreProducto + "]",
                activo = true,
                fechaCreacion = DateTime.UtcNow,
                tipoEnvioInApp = TipoEnvioInApp.NuevaRecompensa
            };
           await inAppNotificationService.AddAsync(inApp);

            // Enviar mail notificando la recompensa al recomendador
            var tiposEnvio = await correoService.ObtenerTiposEnvioCorreo();
            var tipoEnvio = tiposEnvio.Where(u => u.nombre == TipoEnvio.EscanearProducto)
                                            .SingleOrDefault();

            var correo = new Correo(tipoEnvio, usuario.correo, usuario.nombre, _config["AppConfiguration:LogoURL"]);
            correoService.EnviarCorreo(correo);

            // si el usuario tiene perfil 'Basic' o 'Friend' entonces lo cambiamos a perfil 'Lover' 
            var perfilUsuario = await perfilService.GetByIdAsync(usuario.idPerfil.Value);

            bool cambiarPerfil = (perfilUsuario.nombre == Perfil.Perfiles.Basic || perfilUsuario.nombre == Perfil.Perfiles.Friend);
                 
            if (cambiarPerfil)
            {
                var perfiles = await perfilService.GetAllAsync();
                var perfilLover = perfiles
                                  .Where(u => u.nombre == Perfil.Perfiles.Lover)
                                  .SingleOrDefault();

                usuario.idPerfil = perfilLover.id;
                var updateResult = await usuarioService.UpdateAsync(usuario);

                if(updateResult) 
                {  
                    // creamos inApp notificando el cambio de perfil
                    var inAppPerfil = new InAppNotification
                    {
                        idUsuario = usuario.id.Value,
                        titulo = "Tu perfil ha cambiado!",
                        mensaje = "Tu perfil ahora es 'Lover' porque has escaneado un propducto de [" + qr.idEntidad + "].",
                        activo = true,
                        fechaCreacion = DateTime.UtcNow,
                        tipoEnvioInApp = TipoEnvioInApp.PrimeraCompra
                    };
                    await inAppNotificationService.AddAsync(inAppPerfil);

                } 
            } 

            // enviamos nuevo correo notificando el cambio de perfil?

            return Ok(true);
        }

        [Authorize]
        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try
            {
                var result = await _service.Remove(id);
                if (result == false) return NotFound();
                return Ok("QR consumido correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                 new { message = MessageProvider.GetMessage("QR:Eliminar", "Error"), id });
            }
        }

        /// <summary>
        /// Exportar listado a Excel o pdf
        /// </summary> 
        /// <returns> File to download </returns>
        [HttpGet("Exportar")]
        //[Authorize(Policy = "RequireAdmin")]
        //[Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> Exportar([FromServices] ICorreoService correoService,
                                                  [FromQuery] ExportFormat formato,
                                                  [FromQuery] bool envioEmail)
        {
            var entityName = nameof(QRCode);

            var file = await _service.ExportarAsync(formato);

            string fileExtension = string.Empty;
            string contentType = string.Empty;

            switch (formato)
            {
                case ExportFormat.Excel:
                    contentType = _exportConfig.ExcelContentType;
                    fileExtension = _exportConfig.ExcelExtension;
                    break;
                case ExportFormat.Pdf:
                    contentType = _exportConfig.PdfContentType;
                    fileExtension = _exportConfig.PdfExtension;
                    break;
            }

            var fileName = $"List_{entityName.ToString()}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";

            if (envioEmail)
            {
                var tiposEnvioCorreo = await correoService.ObtenerTiposEnvioCorreo();
                var tipoEnvioCorreo = tiposEnvioCorreo.Where(u => u.nombre == TipoEnvioCorreo.TipoEnvio.EnvioReport)
                                                      .SingleOrDefault();

                tipoEnvioCorreo.asunto = $"Report {entityName.ToString()} ({fileExtension})";
                tipoEnvioCorreo.cuerpo = $"Se adjunta el informe para la vista de datos {entityName.ToString()}";

                var correo = new Correo(tipoEnvioCorreo, _exportConfig.CorreoAdmin, "Admin", "");
                correo.FicheroAdjunto = new FicheroAdjunto()
                {
                    Archivo = file,
                    ContentType = contentType,
                    NombreArchivo = fileName
                };
                correoService.EnviarCorreo(correo);
            }
            return File(file, contentType, fileName);
        }
    }
}
