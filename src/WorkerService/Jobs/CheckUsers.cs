using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.Text;
using Utilities;
using WorkerService.Common;
using WorkerService.Configuration;
using WorkerService.Interfaces;

namespace WorkerService.Jobs
{
    public class CheckUsers : BaseBackgroundService<CheckUsers>, IBackgroundService
    {
        private readonly MailConfiguration _mailSettings;

        public CheckUsers(ILogger<CheckUsers> logger, 
                          IOptions<JobsConfiguration> options,
                          IOptions<MailConfiguration> mailOptions,
                          IServiceScopeFactory scopeFactory) {
            _logger = logger; 
            _jobsConfiguration = options.Value;
            _mailSettings = mailOptions.Value;
            _scopeFactory = scopeFactory;
            _jobSettings = GetCurrentJobSettings(Common.WorkerService.CheckUsers); 
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
                    var (scopeUser, userService) = GetServiceProvider<IUsuarioService>();
                    using (scopeUser) {

                        // todos los usuarios que no esten suscritos y que ademas esten activos y tengan el campo 'correo' informado
                        var unsubscribedUsers = userService.CheckUnsubscribedUsers().Result.Where(u => u.activo == true && string.IsNullOrEmpty(u.correo));

                        // Enviar un correo recordatorio a los usuario en [unsubscribedUsers] 
                        var sb = new StringBuilder();
                        foreach (var usuario in unsubscribedUsers) {

                            // Enviar correo recordatorio al email {usuario.correo}  
                            var (scopeMail, correoService) = GetServiceProvider<ICorreoService>();
                            using (scopeMail)
                            {
                                var tipoEnvioCorreo = correoService.ObtenerTiposEnvioCorreo().Result.Where(u => u.nombre == "SuscripciónActivada").Single();
                                
                                var correo = new Correo(tipoEnvioCorreo, usuario.correo, usuario.nombre, _mailSettings.LogoURL);
                                var emailToken = correoService.EnviarCorreo(correo,
                                                                            EncodeDecodeHelper.GetDecodeValue(_mailSettings.ServidorSmtp),
                                                                            EncodeDecodeHelper.GetDecodeValue(_mailSettings.PuertoSmtp),
                                                                            EncodeDecodeHelper.GetDecodeValue(_mailSettings.UsuarioSmtp),
                                                                            EncodeDecodeHelper.GetDecodeValue(_mailSettings.ContrasenaSmtp));

                                sb.AppendLine($"Se ha enviado un correo de tipo [RememberSubscribe] al usuario [{usuario.correo}]");

                                var (scopeEmailToken, emailTokenService) = GetServiceProvider<IEmailTokenService>();
                                using (scopeEmailToken)
                                {
                                    // creamos un objeto de la entidad EmailToken y lo guardamos en BD para el seguimiento del tolken enviado en el email
                                    var emailTokenEntity = new EmailToken {
                                        id = Guid.NewGuid(),
                                        userId = usuario.id.Value,
                                        token = emailToken,
                                        fechaCreacion = DateTime.UtcNow,
                                        fechaExpiracion = emailTokenService.GetExpirationDate(DateTime.UtcNow),
                                        emailAction = EmailToken.EmailTokenActions.SubscribeNewsletter.ToString(),
                                        consumido = false,
                                        fechaConsumido = null,
                                        userAgent = null,
                                        ip = null
                                    };
                                    var results = await emailTokenService.AddAsync(emailTokenEntity);
                                }
                            }
                        }

                        // Añadir ejecución "Passed"
                        var workerServiceExecution = new WorkerServiceExecution {
                            //id =Guid.NewGuid(),
                            workerService = Common.WorkerService.CheckUsers,
                            result = WorkerServiceResult.Passed,
                            resultDetailed = sb.ToString(),
                            executionTime = DateTime.UtcNow
                        };

                        var result = await AddWorkerServiceExecution(workerServiceExecution);
                        //if (result == false) 

                        _logger.LogInformation($"Check done at {DateTime.Now}");
                    }

                }
                catch (TaskCanceledException) { 
                    var aux = "";
                    // si el error es de tipo TaskCanceledException no hacemos nada
                }
                catch (Exception ex) { 
                    var workerServiceExecution = new WorkerServiceExecution {
                        //id =Guid.NewGuid(),
                        workerService = Common.WorkerService.CheckUsers,
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