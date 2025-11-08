using Application.DTOs.Filters;
using Application.DTOs.Requests;
using Application.Interfaces.DTOs.Filters;
using Application.Messaging.Handler;
using Application.Interfaces.Messaging;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Application.Messaging;
using Domain.Entities;

using Microsoft.Extensions.DependencyInjection;
using static Domain.Entities.TipoRecompensa;

namespace Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //
            // Register Services
            //
            services.AddScoped<ICorreoService, CorreoService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IActividadService, ActividadService>();
            services.AddScoped<ITipoActividadService, TipoActividadService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<ITipoEntidadService, TipoEntidadService>();
            services.AddScoped<IEntidadService, EntidadService>();
            services.AddScoped<ITestimonioService, TestimonioService>();
            services.AddScoped<ITransaccionService, TransaccionService>();
            services.AddScoped<IQRService, QRService>();
            services.AddScoped<IFAQService, FAQService>();
            services.AddScoped<IRecompensaService, RecompensaService>();
            services.AddScoped<IConsultaService, ConsultaService>();
            services.AddScoped<IMotivoConsultaService, MotivoConsultaService>();
            services.AddScoped<ITipoEnvioCorreoService, TipoEnvioCorreoService>();
            services.AddScoped<IWorkerServiceExecutionService, WorkerServiceExecutionService>();
            services.AddScoped<IEmailTokenService, EmailTokenService>();
            services.AddScoped<IAccionService, AccionService>();
            services.AddScoped<ICampanaService, CampanaService>();
            services.AddScoped<ITipoCampanaService, TipoCampanaService>();
            services.AddScoped<ICampanaExecutionService, CampanaExecutionService>();
            services.AddScoped<ITipoSegmentoService, TipoSegmentoService>();
            services.AddScoped<ISegmentoService, SegmentoService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IDataQueryService, DataQueryService>();
            services.AddScoped<ITipoTransaccionService, TipoTransaccionService>();
            services.AddScoped<IInAppNotificationService, InAppNotificationService>();
            services.AddScoped<ISmsNotificationService, SmsNotificationService>();
            //services.AddScoped<ITipoRecompensaService, TipoRecompensaService>();
            //services.AddScoped<IUsuarioRecompensaService, UsuarioRecompensaService>();
            services.AddScoped<IActividadReservaService, ActividadReservaService>();
            services.AddScoped<ILogService, LogService>();

            services.AddScoped<IExcelExporter, ExcelExporter>();
            services.AddScoped<IExporter, PdfExporter>();
            services.AddScoped<IExportService, DataQueryService>();
            services.AddScoped<ISmsService, TwilioSmsService>();

            services.AddScoped<NotificationRequestHandler>();

            //services.AddSingleton<INotificationProcessor, NotificationProcessor>();
            services.AddScoped<INotificationProcessor, NotificationProcessor>();

            //
            // Register Filters
            //
            services.AddScoped<IFilters<Actividad>, ActividadFilters>();
            services.AddScoped<IFilters<Categoria>, CategoriaFilters>();
            services.AddScoped<IFilters<Direccion>, DireccionFilters>();
            services.AddScoped<IFilters<Entidad>, EntidadFilters>();
            services.AddScoped<IFilters<FAQ>, FAQFilters>();
            services.AddScoped<IFilters<Consulta>, ConsultaFilters>();
            services.AddScoped<IFilters<MotivoConsulta>, MotivoConsultaFilters>();
            services.AddScoped<IFilters<Producto>, ProductoFilters>();
            services.AddScoped<IFilters<QR>, QRFilters>();
            services.AddScoped<IFilters<Recompensa>, RecompensaFilters>();
            services.AddScoped<IFilters<Rol>, RolFilters>();
            services.AddScoped<IFilters<Testimonio>, TestimonioFilters>();
            services.AddScoped<IFilters<TipoActividad>, TipoActividadFilters>();
            services.AddScoped<IFilters<TipoEntidad>, TipoEntidadFilters>();
            services.AddScoped<IFilters<TipoEnvioCorreo>, TipoEnvioCorreoFilters>();
            services.AddScoped<IFilters<Transaccion>, TransaccionFilters>();
            services.AddScoped<IFilters<Usuario>, UsuarioFilters>();
            services.AddScoped<IFilters<Accion>, AccionFilters>();
            services.AddScoped<IFilters<Campana>, CampanaFilters>();
            services.AddScoped<IFilters<TipoCampana>, TipoCampanaFilters>();
            services.AddScoped<IFilters<CampanaExecution>, CampanaExecutionFilters>();
            services.AddScoped<IFilters<TipoSegmento>, TipoSegmentoFilters>();
            services.AddScoped<IFilters<Segmento>, SegmentoFilters>();
            services.AddScoped<IFilters<Login>, LoginFilters>();
            services.AddScoped<IFilters<TipoTransaccion>, TipoTransaccionFilters>();
            services.AddScoped<IFilters<InAppNotification>, InAppNotificationFilters>();
            services.AddScoped<IFilters<SmsNotification>, SmsNotificationFilters>();
            services.AddScoped<IFilters<TipoRecompensa>, TipoRecompensaFilters>();
            services.AddScoped<IFilters<ActividadReserva>, ActividadReservaFilters>();
            services.AddScoped<IFilters<Log>, LogFilters>();

            return services;
        }
    }
}