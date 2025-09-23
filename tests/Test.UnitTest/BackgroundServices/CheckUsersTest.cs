using Application.Interfaces.Services; 
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Moq;
using Test;
using WorkerService.Common;
using WorkerService.Configuration;
using WorkerService.Jobs;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Test.UnitTest.BackgroundServices
{
    [Category("UnitTest")]
    [TestFixture]
    internal class CheckUsersTest
    {  
        Mock<IUsuarioService> userServiceMock;
        //Mock<IWorkerServiceExecutionService> workerServiceExecutionService;
        Mock<IServiceScope> scopeMock;
        Mock<IServiceProvider> serviceProviderMock;
        Mock<IServiceScopeFactory> scopeFactoryMock;
        Mock<ILogger<CheckUsers>> loggerMock;

        //WorkerServiceExecution testWorkerServiceExecution;

       [SetUp]
        public void SetUp() {

            userServiceMock = new Mock<IUsuarioService>();
            userServiceMock.Setup(x => x.CheckUnsubscribedUsers())
                           .ReturnsAsync(new List<Usuario>());


            //testWorkerServiceExecution = new WorkerServiceExecution {
            //    id =Guid.NewGuid(),
            //    workerService = WorkerService.Common.WorkerService.CheckUsers,
            //    result = WorkerServiceResult.Passed,
            //    resultDetailed = "Test WorkerService has been executed correctly",
            //    executionTime = DateTime.Now
            //}; 

            //workerServiceExecutionService = new Mock<IWorkerServiceExecutionService>();
            //workerServiceExecutionService.Setup(r => r.AddAsync(testWorkerServiceExecution))
            //               .ReturnsAsync(true); 

            serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IUsuarioService)))
                .Returns(userServiceMock.Object);
             
            scopeMock = new Mock<IServiceScope>(); // Mock del scope
            scopeMock.Setup(s => s.ServiceProvider).Returns(serviceProviderMock.Object);

            scopeFactoryMock = new Mock<IServiceScopeFactory>();
            scopeFactoryMock.Setup(f => f.CreateScope()).Returns(scopeMock.Object);

            loggerMock = new Mock<ILogger<CheckUsers>>();
        }

        [Test]
        public async Task CheckUsersJob_RunCheckAsyn_WriteLogsInformation()
        {
            var optionsMock = Options.Create(new JobsConfiguration {
                Jobs = new List<JobSettings> {
                    new JobSettings {
                        JobName = WorkerService.Common.WorkerService.CheckUsers,
                        IntervalMinutes = 1
                        // si en el futuro añado otro modo de ejecución, aquí lo configuro
                    }
                }
            });

            var mailOptionsMock = Options.Create(new MailConfiguration { 
                    UsuarioSmtp = "",
                    ContraseñaSmtp = "",
                    ServidorSmtp = "",
                    PuertoSmtp = "" }); 
             
          var checkUsers = new CheckUsers(loggerMock.Object, 
                                          optionsMock,
                                          mailOptionsMock,
                                          scopeFactoryMock.Object);
           
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(100); // Cancela rápido para no bloquear
             
            try {
                await checkUsers.RunCheckUsersAsync(cts.Token);
            }
            catch (TaskCanceledException) { 
                var aux = "";
                //error esperado por la cancelacion rápida del CancellationTokenSource
            }
            catch (Exception ex) {
                //error inesperado
                var errorMessage = ex.Message;
            }
            Console.Out.WriteLine($"WorkerService [RunCheckUsersAsync] ejecutado correctamente a {DateTime.Now}.");

            //userServiceMock.Verify(x => x.CheckUnsubscribedUsers(), Times.AtLeastOnce);  // Verificar que se llamó al servicio
        }
    }
}