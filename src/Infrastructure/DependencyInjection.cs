using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
 
using Application.Interfaces.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Repositories.UnitOfWork;
using Infrastructure.Persistence; 

namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        private static IConfiguration _configuration;

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)  
        {  
            //Get custom configuratiom file  
            string baseDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName; // "C/Users/David/Desktop/"
            var configPath = Path.Combine(baseDirectory, "Infrastructure", "bin", "Debug", "net8.0", "Persistence"); // "C/Users/David/Desktop/Infrastructure/bin/Debug/net8.0/Persistence"

            _configuration = new ConfigurationBuilder()
                  .SetBasePath(configPath)
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .Build();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection")));

            //register Repository Interfaces
            services.AddScoped<ITipoEnvioCorreoRepository, TipoEnvioCorreoRepository>(); // Solo repositorio, sin service
            services.AddScoped<IUnitOfWork, UnitOfWork>();  // Solo repositorio, sin service 
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IRolRepository, RolRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IActividadRepository, ActividadRepository>();
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
            services.AddScoped<ISegmentoRepository, SegmentoRepository>();
            services.AddScoped<IUsuarioSegmentosRepository, UsuarioSegmentosRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IDataQueryRepository, DataQueryRepository>();
            services.AddScoped<ITipoTransaccionRepository, TipoTransaccionRepository>();
            return services;
        }
    }
}