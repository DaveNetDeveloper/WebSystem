using Domain.Entities;
using Infrastructure.Persistence; 

using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Test.Integration.Api
{
    /// <summary>
    /// Pruebas de integración para los endpoints del controlador de usuarios.
    /// </summary>
    [Category("Integration")]
    [TestFixture]
    public class UsuariosControllerTests
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
                 new Usuario { id = 1, nombre = "Juan", apellidos = "apellidos", correo = "juan@test.com", contraseña="abc", token = "VBx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE=" },
                 new Usuario { id = 2, nombre = "Ana",  apellidos = "apellidos", correo = "ana@test.com",  contraseña = "abc", token = "Zly7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URCA=" },
                 new Usuario { id = 999, nombre = "testAdmin", apellidos = "apellidos", correo = "mail@test.com", contraseña = "abc", token= "XYx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE=" }
            );
            context.SaveChanges();  
        }

        /// <summary>
        /// Verifica que el endpoint /Usuarios/ObtenerUsuarios devuelve todos los usuarios esperados.
        /// </summary>
        [Test]
        public async Task ObtenerUsuarios_Test()
        {   
            var response = await _client.GetAsync("/Usuarios/ObtenerUsuarios");
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)); 

            var usuarios = await response.Content.ReadFromJsonAsync<List<Usuario>>();

            // Verificar que la respuesta contiene los usuarios esperados
            Assert.IsNotNull(usuarios);
            Assert.That(3, Is.EqualTo(usuarios.Count));
            Assert.That(usuarios.Exists(u => u.nombre == "Juan"));
            Assert.That(usuarios.Exists(u => u.nombre == "Ana"));
            Assert.That(usuarios.Exists(u => u.nombre == "testAdmin")); 
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario por su ID desde el endpoint /Usuarios/ObtenerUsuario/{id}.
        /// </summary>
        [Test]
        public async Task ObtenerUsuarioById_Test()
        {
            int id = 1;
            var response = await _client.GetAsync("/Usuarios/ObtenerUsuario/" + id); 
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
             
            var content = await response.Content.ReadAsStringAsync();

            var usuario = await response.Content.ReadFromJsonAsync<Usuario>(); 
            Assert.IsNotNull(usuario);
            Assert.That(usuario.nombre.Equals("Juan"));
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/ObtenerUsuarioByEmail.
        /// </summary>
        [Test]
        public async Task ObtenerUsuarioByEmail_Test()
        {
            var response = await _client.GetAsync("/Usuarios/ObtenerUsuarioByEmail");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();

            var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            Assert.IsNotNull(usuario);
            Assert.That(usuario.nombre.Equals("Juan"));
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/ObtenerUsuarioByToken.
        /// </summary>
        [Test]
        public async Task ObtenerUsuarioByToken_Test()
        {
            var response = await _client.GetAsync("/Usuarios/ObtenerUsuarioByToken");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();

            var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            Assert.IsNotNull(usuario);
            Assert.That(usuario.nombre.Equals("Juan"));
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/CrearUsuario.
        /// </summary>
        [Test]
        public async Task CrearUsuario_Test()
        { 
            var response = await _client.GetAsync("/Usuarios/CrearUsuario");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();

            var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            Assert.IsNotNull(usuario);
            Assert.That(usuario.nombre.Equals("Juan"));
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/ActualizarUsuario.
        /// </summary>
        [Test]
        public async Task ActualizarUsuario_Test()
        { 
            var response = await _client.GetAsync("/Usuarios/ActualizarUsuario");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();

            var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            Assert.IsNotNull(usuario);
            Assert.That(usuario.nombre.Equals("Juan"));
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/Login.
        /// </summary>
        [Test]
        public async Task Login_Test()
        {
            var response = await _client.GetAsync("/Usuarios/Login");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();

            var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            Assert.IsNotNull(usuario);
            Assert.That(usuario.nombre.Equals("Juan"));
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/CambiarContraseña.
        /// </summary>
        [Test]
        public async Task CambiarContraseña_Test()
        { 
            var response = await _client.GetAsync("/Usuarios/CambiarContraseña");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();

            var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            Assert.IsNotNull(usuario);
            Assert.That(usuario.nombre.Equals("Juan"));
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/ValidarCuenta.
        /// </summary>
        [Test]
        public async Task ValidarCuenta_Test()
        { 
            var response = await _client.GetAsync("/Usuarios/ValidarCuenta");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();

            var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            Assert.IsNotNull(usuario);
            Assert.That(usuario.nombre.Equals("Juan"));
        } 

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/ActivacionSuscripcion.
        /// </summary>
        [Test]
        public async Task ActivacionSuscripcion_Test()
        { 
            var response = await _client.GetAsync("/Usuarios/ActivacionSuscripcion");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();

            var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            Assert.IsNotNull(usuario);
            Assert.That(usuario.nombre.Equals("Juan"));
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/Eliminar/{id}.
        /// </summary>
        [Test]
        public async Task Eliminar_Test()
        {
            int id = 1;
            var response = await _client.GetAsync("/Usuarios/Eliminar/" + id);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();

            var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            Assert.IsNotNull(usuario);
            Assert.That(usuario.nombre.Equals("Juan"));
        }
























        /// <summary>
        /// Limpia los recursos después de cada prueba.
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}