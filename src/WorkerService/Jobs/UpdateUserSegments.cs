using Application.Interfaces.Services;
using Domain.Entities;
using Utilities;
using WorkerService.Common;
using WorkerService.Configuration;
using WorkerService.Interfaces;

using Microsoft.Extensions.Options;
using System.Text;

namespace WorkerService.Jobs
{
    public class UpdateUserSegments : BaseBackgroundService<UpdateUserSegments>, IUpdateUserSegments
    {
        public UpdateUserSegments(ILogger<UpdateUserSegments> logger, 
                          IOptions<JobsConfiguration> options,
                          IServiceScopeFactory scopeFactory) { 
            _logger = logger; 
            _jobsConfiguration = options.Value; 
            _scopeFactory = scopeFactory; 
            _jobSettings = GetCurrentJobSettings(Common.WorkerService.UpdateUserSegments);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RunUpdateUserSegmentsAsync(stoppingToken);
        }

        public async Task RunUpdateUserSegmentsAsync(CancellationToken stoppingToken)
        {
            if (!_jobSettings.Enabled) {
                _logger.LogInformation($"Job {_jobSettings.JobName} deshabilitado.");
                return;
            }

            while (!stoppingToken.IsCancellationRequested) {
                try { 
                    var (scopeUser, userService) = GetServiceProvider<IUsuarioService>();
                    using (scopeUser)
                    {
                        var sb = new StringBuilder();
                        var allActiveUsers = userService.GetAllAsync().Result.Where(u => u.activo == true);

                        foreach (var usuario in allActiveUsers)  {
                            var (scopeSeg, segmentService) = GetServiceProvider<ISegmentoService>();
                            using (scopeSeg)
                            {
                                segmentService.ApplySegmentsForUser(usuario);
                            }
                        }
                    }
                }
                catch (TaskCanceledException) {
                    var aux = "";  // si el error es de tipo TaskCanceledException no hacemos nada
                }
                catch (Exception ex) {
                    // Añadir ejecución "Failed"
                    var workerServiceExecution = new WorkerServiceExecution {
                        //id = Guid.NewGuid(),
                        workerService = Common.WorkerService.UpdateUserSegments,
                        result = WorkerServiceResult.Failed,
                        resultDetailed = $"WorkerService has failed with error: {ex.Message.Truncate(500)}",
                        executionTime = DateTime.UtcNow
                    };
                    var result = await AddWorkerServiceExecution(workerServiceExecution);
                    _logger.LogError(ex, $"WorkerService has failed  with error: {ex.Message}");
                }
                //
                // Set job frequency
                //
                if (_jobSettings.IntervalMinutes.HasValue) {
                    await Task.Delay(TimeSpan.FromMinutes(_jobSettings.IntervalMinutes.Value), stoppingToken);
                }
                else if (_jobSettings.IntervalDays.HasValue) {
                    await Task.Delay(TimeSpan.FromDays(_jobSettings.IntervalDays.Value), stoppingToken);
                }
                else if (_jobSettings.ScheduledTime.HasValue) {
                    var delay = _jobSettings.ScheduledTime.Value - DateTimeOffset.Now;
                    if (delay > TimeSpan.Zero)
                        await Task.Delay(delay, stoppingToken);
                    else {
                        break; // ya pasó la hora
                    }
                }
                else {
                    throw new InvalidOperationException($"El Job {_jobSettings.JobName} no tiene configuración definida.");
                }
            }
        }
    }
}