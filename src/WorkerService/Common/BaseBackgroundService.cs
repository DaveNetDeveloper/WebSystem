using Application.Interfaces.Services;
using Domain.Entities;
using WorkerService.Configuration;

namespace WorkerService.Common
{
    public class BaseBackgroundService<Job> : BackgroundService
    {
        protected IServiceScopeFactory _scopeFactory; 
        protected ILogger<Job> _logger;

        protected JobSettings _jobSettings;
        protected JobsConfiguration _jobsConfiguration;

        protected bool IsJobEnabled()
        {
            if (!_jobSettings.Enabled)
            {
                _logger.LogInformation($"Job {_jobSettings.JobName} deshabilitado.");
                return false;
            }
            return true;
        }

        protected bool IsJobScheduled()
        {
            if (_jobSettings.ScheduledTime.HasValue && _jobSettings.ScheduledTime.Value > DateTime.UtcNow)
            {
                _logger.LogInformation($"El job {_jobSettings.JobName} no se iniciará hasta {_jobSettings.ScheduledTime}.");
                return false;
            }
            return true;
        }

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
            var (scopeWS, workerService) = GetServiceProvider<IWorkerServiceExecutionService>();
            using (scopeWS) {
                return await workerService.AddAsync(workerServiceExecution);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
        } 

        public static class WorkerService
        {
            public const string ReminderForUnsubscribers = "ReminderForUnsubscribers";
            public const string UpdateUserSegments = "UpdateUserSegments";
            public const string ExecuteCampaigns = "ExecuteCampaigns";
        }
    }
}