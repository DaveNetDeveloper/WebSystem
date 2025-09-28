using Domain.Entities;
using Application.Interfaces.Services;
using WorkerService.Common;
using WorkerService.Configuration;
using WorkerService.Interfaces;
using Utilities;

using Microsoft.Extensions.Options;
using System.Text;

namespace WorkerService.Jobs
{
    public class ExecuteCampaigns : BaseBackgroundService<ExecuteCampaigns>, IBackgroundService
    {
        private readonly MailConfiguration _mailSettings;

        public ExecuteCampaigns(ILogger<ExecuteCampaigns> logger, 
                               IOptions<JobsConfiguration> options,
                               IServiceScopeFactory scopeFactory,
                               IOptions<MailConfiguration> mailOptions) { 
            _logger = logger; 
            _jobsConfiguration = options.Value; 
            _scopeFactory = scopeFactory; 
            _jobSettings = GetCurrentJobSettings(Common.WorkerService.ExecuteCampaigns);
            _mailSettings = mailOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RunAsync(stoppingToken);
        }

        public async Task RunAsync(CancellationToken stoppingToken)
        {
            if (!_jobSettings.Enabled) {
                _logger.LogInformation($"Job {_jobSettings.JobName} deshabilitado.");
                return;
            }

            while (!stoppingToken.IsCancellationRequested) {
                try { 
                    var (scopeUser, campanaService) = GetServiceProvider<ICampanaService>();
                    using (scopeUser)
                    {
                        // Obetenmos y recorremos todas las campañas activas
                        var activeCampaigns = campanaService.GetAllAsync()
                                                            .Result
                                                            .Where(u => u.activo == true);
                        var sb = new StringBuilder();

                        foreach (var campaign in activeCampaigns)
                        {
                            // Obtenemos la ultima ejecucion de la campaña
                            CampanaExecution? lastCampaignExecution; 
                            var (scopeExecution, executionService) = GetServiceProvider<ICampanaExecutionService>();
                            using (scopeUser) {
                                lastCampaignExecution = executionService.GetAllAsync()
                                                                        .Result
                                                                        .Where(e => e.idCampana == campaign.id)
                                                                        .OrderByDescending(e => e.fechaEjecucion)
                                                                        .FirstOrDefault();
                                
                                // Comprobamos si hay que ejecutar la campaña
                                if (campaign.IsTimeToRun(lastCampaignExecution))  {

                                    // Obtener acciones de cada campaña 
                                    var acciones = campanaService.GetAccionesByCampana(campaign.id);
                                    if (!acciones.Result.Any()) continue;

                                    var listaAcciones = acciones.Result;

                                    // Obtener segmentos de cada campaña
                                    var segmentos = campanaService.GetSegmentoByCampana(campaign.id);
                                    if (!segmentos.Result.Any()) continue;

                                    foreach (var segmento in segmentos.Result) {

                                        // Obtener usuarios de cada segmento
                                        var (scopeSeg, segmentService) = GetServiceProvider<ISegmentoService>();
                                        using (scopeSeg) {

                                            var usuarios = segmentService.GetUsuariosBySegmento(segmento.id);
                                            if (!usuarios.Result.Any()) continue;

                                            foreach (var usuario in usuarios.Result) {

                                                if (usuario?.id == null) continue;

                                                var (scopeAction, actionService) = GetServiceProvider<IAccionService>();
                                                using (scopeSeg) 
                                                {
                                                    var resultActionsExec = await actionService.EjecutarAccionesForUser(listaAcciones,
                                                                                                                        usuario.id.Value,
                                                                                                                        campaign.id);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        // Añadir ejecución "Passed"
                        var workerServiceExecution = new WorkerServiceExecution { 
                            workerService = _jobSettings.JobName,
                            result = WorkerServiceResult.Passed,
                            resultDetailed = sb.ToString(),
                            executionTime = DateTime.UtcNow
                        }; 
                        var wsExecutionresult = await AddWorkerServiceExecution(workerServiceExecution);
                        _logger.LogInformation($"Job {_jobSettings.JobName} done at {DateTime.Now}");
                    }
                }
                catch (TaskCanceledException) {
                    var aux = "";  // si el error es de tipo TaskCanceledException no hacemos nada
                }
                catch (Exception ex) 
                {
                    var workerServiceExecution = new WorkerServiceExecution {
                        workerService = _jobSettings.JobName,
                        result = WorkerServiceResult.Failed,
                        resultDetailed = $"WorkerService has failed with error: {ex.Message.Truncate(500)}",
                        executionTime = DateTime.UtcNow
                    };
                    var wsExecutionresult = await AddWorkerServiceExecution(workerServiceExecution);
                    _logger.LogError(ex, $"WorkerService has failed  with error: {ex.Message}");
                }
                
                // Set job frequency
                if (_jobSettings.IntervalMinutes.HasValue) {
                    await Task.Delay(TimeSpan.FromMinutes(_jobSettings.IntervalMinutes.Value), stoppingToken);
                }
                else if (_jobSettings.IntervalDays.HasValue) {
                    await Task.Delay(TimeSpan.FromDays(_jobSettings.IntervalDays.Value), stoppingToken);
                }
                else if (_jobSettings.IntervalHours.HasValue)
                {
                    await Task.Delay(TimeSpan.FromHours(_jobSettings.IntervalHours.Value), stoppingToken);
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