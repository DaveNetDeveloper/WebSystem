using Application.Interfaces.Services;
using Domain.Entities;
using WorkerService.Configuration;

namespace WorkerService.Jobs
{
    public class BaseBackgroundService<Job> : BackgroundService
    {
        protected IServiceScopeFactory _scopeFactory; 
        protected ILogger<Job> _logger;

        protected JobSettings _jobSettings;
        protected JobsConfiguration _jobsConfiguration;
        
        protected JobSettings GetCurrentJobSettings(string jobName)
        {
            return _jobsConfiguration.Jobs.FirstOrDefault(j => j.JobName == jobName)
            ?? throw new InvalidOperationException($"No se encontró configuración para el job {jobName}");
        }

        protected (IServiceScope Scope, T Service) GetServiceProvider<T>() where T : notnull
        {
            var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetService<T>()
                ?? throw new InvalidOperationException($"{typeof(T).Name} no está registrado.");

            return (scope, service);
        }

        protected async Task<bool> AddWorkerServiceExecution(WorkerServiceExecution workerServiceExecution)
        {
            //using var scope = _scopeFactory.CreateScope();
            //var workerService = scope.ServiceProvider.GetRequiredService<IWorkerServiceExecutionService>();

            var (scopeWS, workerService) = GetServiceProvider<IWorkerServiceExecutionService>();
            using (scopeWS) {
                return await workerService.AddAsync(workerServiceExecution);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
        }
    }
}