﻿using Application.Common;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence; 
using Infrastructure.Repositories;
using Infrastructure.Repositories.UnitOfWork;
using Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        private static IConfiguration _configuration;

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
                                                           string environmentName)  
        {
            string baseDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var configPath = Path.Combine(baseDirectory, "Infrastructure", "bin", "Debug", "net8.0", "Persistence");

            if (environmentName == Environments.Test) {
                configPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName, "src", "Infrastructure", "bin", "Debug", "net8.0", "Persistence");
            }
            else {
                string elseVar = "";
            }
            
            _configuration = new ConfigurationBuilder()
                  .SetBasePath(configPath)
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .Build();

            //
            // Sets ConnectionString
            //
            services.AddDatabaseMigrations(_configuration.GetConnectionString("DefaultConnection"));
            services.AddDbContext<ApplicationDbContext>(op => op.UseNpgsql(_configuration.GetConnectionString("DefaultConnection")));

            //
            //register Repository Interfaces
            //
            services.AddScoped<ITipoEnvioCorreoRepository, TipoEnvioCorreoRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();  // Solo repositorio, sin service 
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IRolRepository, RolRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IActividadRepository, ActividadRepository>();
            services.AddScoped<ITipoActividadRepository, TipoActividadRepository>();
            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<ITipoEntidadRepository, TipoEntidadRepository>();
            services.AddScoped<IEntidadRepository, EntidadRepository>();
            services.AddScoped<ITestimonioRepository, TestimonioRepository>();
            services.AddScoped<ITransaccionRepository, TransaccionRepository>();
            services.AddScoped<IQRRepository, QRRepository>();
            services.AddScoped<IFAQRepository, FAQRepository>();
            services.AddScoped<IRecompensaRepository, RecompensaRepository>();
            services.AddScoped<IMotivoConsultaRepository, MotivoConsultaRepository>();
            services.AddScoped<IConsultaRepository, ConsultaRepository>();
            services.AddScoped<IWorkerServiceExecutionRepository, WorkerServiceExecutionRepository>();
            services.AddScoped<IEmailTokenRepository, EmailTokenRepository>();
            services.AddScoped<ITipoSegmentoRepository, TipoSegmentoRepository>();
            services.AddScoped<IAccionRepository, AccionRepository>();
            services.AddScoped<ICampanaRepository, CampanaRepository>(); 
            services.AddScoped<ITipoCampanaRepository, TipoCampanaRepository>();
            services.AddScoped<ICampanaExecutionRepository, CampanaExecutionRepository>();
            services.AddScoped<ISegmentoRepository, SegmentoRepository>();
            services.AddScoped<IUsuarioSegmentosRepository, UsuarioSegmentosRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IDataQueryRepository, DataQueryRepository>();
            services.AddScoped<ITipoTransaccionRepository, TipoTransaccionRepository>();

            return services;
        }
    }
}