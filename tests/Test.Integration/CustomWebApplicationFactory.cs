using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Threading.RateLimiting; 
using Microsoft.AspNetCore.RateLimiting;

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;

using Infrastructure.Persistence;
 
namespace Test.Integration
{ 
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _dbName = Guid.NewGuid().ToString();
        public string? TestToken { get; private set; }

        protected override IHost CreateHost(IHostBuilder builder)
        { 
            builder.UseEnvironment("Test");  
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureLogging(logging => {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.None);
            });
             
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                //TODO revisar porque hace que pete el test unitario de security.RateLimiter
                services.AddRateLimiter(options => {
                    options.AddFixedWindowLimiter("UsuariosLimiter", opt => {
                        opt.PermitLimit = 5; // muy bajo para test
                        opt.Window = TimeSpan.FromSeconds(30);
                        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                        opt.QueueLimit = 1;
                    });  
                });

                services.AddDbContext<ApplicationDbContext>(options => {
                    options.UseInMemoryDatabase(_dbName);
                });
                 
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                db.Database.EnsureCreated();

                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build(); 

                TestToken = config["TestUserToken"];  
            }); 
        } 
    } 
}