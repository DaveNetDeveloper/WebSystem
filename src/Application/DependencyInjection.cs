﻿using Application.DTOs.Filters;
using Application.Interfaces.DTOs.Filters;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<IWorkerServiceExecutionService, WorkerServiceExecutionService>();
            services.AddScoped<IEmailTokenService, EmailTokenService>();
            services.AddScoped<ITipoSegmentoService, TipoSegmentoService>();
            services.AddScoped<ISegmentoService, SegmentoService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IDataQueryService, DataQueryService>();
            services.AddScoped<ITipoTransaccionService, TipoTransaccionService>();

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
            services.AddScoped<IFilters<TipoSegmento>, TipoSegmentoFilters>();
            services.AddScoped<IFilters<Segmento>, SegmentoFilters>();
            services.AddScoped<IFilters<Login>, LoginFilters>();
            services.AddScoped<IFilters<TipoTransaccion>, TipoTransaccionFilters>();

            return services;
        }
    }
}