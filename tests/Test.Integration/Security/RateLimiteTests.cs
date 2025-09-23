using Domain.Entities;

using Infrastructure.Persistence; 
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Test;
using Test.Integration;

namespace Test.Integration.Security
{
    /// <summary>
    /// Pruebas de integración para los endpoints del controlador de usuarios.
    /// </summary>
    [Category("Integration")]
    [TestFixture]
    public class RateLimiteTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test"); 
        }

        /// <summary>
        /// Inicializa el entorno para cada prueba, creando un cliente y poblando la base de datos en memoria.
        /// </summary>
        [SetUp]
        public void SetUp()
        {  
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
             
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); 

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _factory.TestToken);

            context.Usuarios.AddRange(
                 new Usuario { id = 1, nombre = "Juan", apellidos = "apellidos", correo = "juan@test.com", contrasena = "abc", token = "VBx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE=", genero = "Hombre" },
                 new Usuario { id = 2, nombre = "Ana",  apellidos = "apellidos", correo = "ana@test.com", contrasena = "abc", token = "Zly7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URCA=", genero = "Otro" },
                 new Usuario { id = 999, nombre = "testAdmin", apellidos = "apellidos", correo = "mail@test.com", contrasena = "abc", token= "XYx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE=", genero = "Mujer" }
            );
            context.SaveChanges();  
        }
  
        [Test]
        public async Task ObtenerUsuarios_ShouldReturn503Error_WhenRateLimitExceeded()  
        {
            try {   
                var limitRequest = 5;
                for (int i = 0; i < limitRequest; i++) {   

                    var response = await _client.GetAsync("/Usuarios/ObtenerUsuarios");  
                    Assert.That(response.StatusCode, Is.Not.EqualTo(HttpStatusCode.ServiceUnavailable), $"Request {(i + 1).ToString()} shouldn't be rate-limited yet");
                    TestContext.Out.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - Request {(i + 1).ToString()} - StatusCode: {response.StatusCode}");
                }  
                var extraResponse = await _client.GetAsync("/Usuarios/ObtenerUsuarios");

                TestContext.Out.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - Request {(limitRequest + 1).ToString() } - StatusCode: {extraResponse.StatusCode}"); 
                Assert.That(extraResponse.StatusCode, Is.EqualTo(HttpStatusCode.ServiceUnavailable));
            }
            catch (HttpRequestException ex) { 
               switch (ex.StatusCode) { 
                    case HttpStatusCode.ServiceUnavailable:
                    case HttpStatusCode.TooManyRequests:
                        Assert.Pass(ex.StatusCode.ToString());
                        break;
                    default:
                        Assert.Fail("Todo ha ido bien");
                        break;
                } 
            } 
            catch (Exception ex) {
                TestContext.Out.WriteLine("Se capturó una excepción generica durante el test:");
                TestContext.Out.WriteLine($"Tipo: {ex.GetType().Name}");
                TestContext.Out.WriteLine($"Mensaje: {ex.Message}");
                TestContext.Out.WriteLine($"StackTrace: {ex.StackTrace}");
            }  
        }

        /// <summary>
        /// Limpia los recursos después de cada prueba.
        /// </summary>
        [TearDown]
        public void Cleanup() {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}