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
    /// <summary>
    /// 
    /// </summary>
    public class ExecuteCampaigns : BaseBackgroundService<ExecuteCampaigns>, IBackgroundService
    {
        private readonly MailConfiguration _mailSettings;

        /// <summary>
        /// 
        /// </summary> 
        public ExecuteCampaigns(ILogger<ExecuteCampaigns> logger, 
                               IOptions<JobsConfiguration> options,
                               IServiceScopeFactory scopeFactory,
                               IOptions<MailConfiguration> mailOptions) { 
            _logger = logger; 
            _jobsConfiguration = options.Value; 
            _scopeFactory = scopeFactory; 
            _jobSettings = GetCurrentJobSettings(WorkerService.ExecuteCampaigns);
            _mailSettings = mailOptions.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param> 
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RunAsync(stoppingToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param> 
        public async Task RunAsync(CancellationToken stoppingToken)
        {
            if (!IsJobEnabled()) return;
            if (!IsJobScheduled()) return;

            while (!stoppingToken.IsCancellationRequested) {
                try { 
                    var (scopeCampana, campanaService) = GetServiceProvider<ICampanaService>();
                    using (scopeCampana)
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
                            using (scopeExecution)
                            {
                                lastCampaignExecution = executionService.GetAllAsync()
                                                                        .Result
                                                                        .Where(e => e.idCampana == campaign.id)
                                                                        .OrderByDescending(e => e.fechaEjecucion)
                                                                        .FirstOrDefault();

                                string jsonEmpty = @"{ ""idUsuario"": [ ] }";
                                string jsonResult = "";
                                if (lastCampaignExecution == null)
                                    jsonResult = jsonEmpty;
                                else
                                    jsonResult = lastCampaignExecution.idsUsuarios;

                                // Comprobamos si hay que ejecutar la campaña
                                if (campaign.IsTimeToRun(lastCampaignExecution))
                                {
                                    // Obtener acciones de cada campaña 
                                    var acciones = campanaService.GetAccionesByCampana(campaign.id).Result;
                                    if (acciones == null || !acciones.Any()) continue; 

                                    // Obtener segmentos de cada campaña
                                    var segmentos = campanaService.GetSegmentoByCampana(campaign.id).Result;
                                    if (segmentos == null || !segmentos.Any()) continue;

                                    bool resultActionsExec = false;
                                    foreach (var segmento in segmentos)
                                    {
                                        // Obtener usuarios de cada segmento
                                        var (scopeSegment, segmentService) = GetServiceProvider<ISegmentoService>();
                                        using (scopeSegment) 
                                        {
                                            // Obtener usuarios de cada segmento
                                            var usuarios = segmentService.GetUsuariosBySegmento(segmento.id).Result; 
                                            if (usuarios == null || !usuarios.Any()) continue;

                                            foreach (var usuario in usuarios)  {

                                                if (usuario?.id == null) continue; 
                                                var (scopeAction, actionService) = GetServiceProvider<IAccionService>();
                                                using (scopeAction) {
                                                    resultActionsExec = await actionService.EjecutarAccionesForUser(acciones,
                                                                                                                    usuario.id.Value,
                                                                                                                    campaign.id);
                                                    if (resultActionsExec)
                                                        jsonResult = JsonHelper.AddValue(jsonResult, "idUsuario", usuario.id.ToString());
                                                }
                                            }
                                        }
                                    }
                                    // Regista la ejecucion de la campaña
                                    var currentExecution = new CampanaExecution
                                    {
                                        idCampana = campaign.id,
                                        fechaEjecucion = DateTime.UtcNow,
                                        estado = resultActionsExec ? CampanaExecution.EstadoEjecucion.Passed : CampanaExecution.EstadoEjecucion.Error,
                                        idsUsuarios = jsonResult
                                    };
                                    await executionService.AddAsync(currentExecution);
                                }
                            }
                        }
                        // Añadir ejecución "Passed"
                        var workerServiceExecution = new WorkerServiceExecution { 
                            workerService = _jobSettings.JobName,
                            result = WorkerServiceExecution.WorkerServiceResult.Passed,
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
                        result = WorkerServiceExecution.WorkerServiceResult.Failed,
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