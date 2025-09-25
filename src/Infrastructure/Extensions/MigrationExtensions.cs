using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class MigrationExtensions
    {
        public static IServiceCollection AddDatabaseMigrations(this IServiceCollection services, 
                                                               string connectionString) {
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(MigrationExtensions).Assembly).For.Migrations()
                    .WithVersionTable(new CustomVersionTableMetaData()))
                .AddLogging(lb => lb.AddFluentMigratorConsole());
            return services;
        }
    }
}