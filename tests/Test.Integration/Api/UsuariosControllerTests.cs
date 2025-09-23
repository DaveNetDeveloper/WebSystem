using Domain.Entities;
using Infrastructure.Persistence; 

using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Utilities;

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

            int idUsuario = 1;
            context.Usuarios.AddRange(
                 new Usuario { id = idUsuario, nombre = "Juan", apellidos = "apellidos", correo = "juan@test.com", suscrito = false, activo = true, contrasena = "AQAAAAIAAYagAAAAECVRFLNzpiyzxS7peWWidxwcz3p3WtNEkx4ILLDsnMFWOj9sAbJgTxQV1tWt+gAs1w==", token = "VBx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE=", genero="Hombre" },
                 new Usuario { id = 2, nombre = "Ana",  apellidos = "apellidos", correo = "ana@test.com", suscrito = false, activo = false, contrasena = "abc", token = "Zly7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URCA=", genero = "Mujer" },
                 new Usuario { id = 999, nombre = "testAdmin", apellidos = "apellidos", correo = "mail@test.com", suscrito = true, activo = false, contrasena = "abc", token= "XYx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE=", genero = "Otro" }
            );
             
            Guid idTipoEntidad = Guid.NewGuid();
            context.TipoEntidades.AddRange(
                 new TipoEntidad
                 {
                     id = idTipoEntidad,  
                     nombre = " Tipo de Entidad 1",
                     descripcion = ""
                 }
            );

            int idEntidad = 1;
            context.Entidades.AddRange(
                 new Entidad{ id = idEntidad, idTipoEntidad = idTipoEntidad, activo= true, nombre = "Entidad 1" , descripcion= "", ubicacion = "", fechaAlta=DateTime.UtcNow, popularidad= 0, imagen=""}
            );

            Guid idRol = Guid.NewGuid();
            context.Roles.AddRange(
                 new Rol { id = idRol, nombre= "Admin",  level = 1, descripcion = "Rol Admin" }
            ); 

            context.UsuarioRoles.AddRange(
                 new UsuarioRol { idusuario = idUsuario, idrol = idRol, identidad = idEntidad }
            );
 
            Guid idTipoEnvioCorreo = Guid.NewGuid();
            context.TipoEnvioCorreos.AddRange(
                 new TipoEnvioCorreo { id = idTipoEnvioCorreo, nombre = "Tipo Envio 1", activo = true, descripcion = "", asunto = "", cuerpo = "" }
            );
 
            Guid token = Guid.NewGuid();
            context.EmailTokens.AddRange(
                 new EmailToken { id = idTipoEnvioCorreo, userId = idUsuario, consumido = false, token = token, fechaCreacion = DateTime.UtcNow, fechaConsumido = null, fechaExpiracion = DateTime.UtcNow.AddDays(3), emailAction = "", userAgent = null, ip = null }
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
        //[Test]
        //public async Task ObtenerUsuarioById_Test()
        //{
        //    int id = 1;
        //    var response = await _client.GetAsync("/Usuarios/ObtenerUsuario/" + id); 
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
             
        //    var content = await response.Content.ReadAsStringAsync();

        //    var usuario = await response.Content.ReadFromJsonAsync<Usuario>(); 
        //    Assert.IsNotNull(usuario);
        //    Assert.That(usuario.nombre.Equals("Juan"));
        //}

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/ObtenerUsuarioByEmail.
        /// </summary>
        //[Test]
        //public async Task ObtenerUsuarioByEmail_Test()
        //{
        //    var response = await _client.GetAsync("/Usuarios/ObtenerUsuarioByEmail");
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        //    var content = await response.Content.ReadAsStringAsync();

        //    var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
        //    Assert.IsNotNull(usuario);
        //    Assert.That(usuario.nombre.Equals("Juan"));
        //}

        ///// <summary>
        ///// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/ObtenerUsuarioByToken.
        ///// </summary>
        //[Test]
        //public async Task ObtenerUsuarioByToken_Test()
        //{
        //    var response = await _client.GetAsync("/Usuarios/ObtenerUsuarioByToken");
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        //    var content = await response.Content.ReadAsStringAsync();

        //    var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
        //    Assert.IsNotNull(usuario);
        //    Assert.That(usuario.nombre.Equals("Juan"));
        //}

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/CrearUsuario.
        /// </summary>
        [Test]
        public async Task CrearUsuario_Test()
        {
            var nuevoUsuario = new Usuario
            {
                id = -1,
                nombre = "Pedro",
                apellidos = "García",
                correo = "pedro@test.com",
                contrasena = "pass123",
                activo = true,
                suscrito = false,
                genero = "Hombre",
                fechaCreacion = DateTime.UtcNow,
                fechaNacimiento = DateTime.Now.AddYears(-39),
                puntos = 300
            };
             
            var response = await _client.PostAsJsonAsync("/Usuarios/CrearUsuario", nuevoUsuario);
            var content = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsNotNull(content);
            Assert.AreEqual(content, "true");
            //Assert.That(usuario.nombre.Equals("Juan"));
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/ActualizarUsuario.
        /// </summary>
        [Test]
        public async Task ActualizarUsuario_Test()
        {
            var usuario = new Usuario
            {
                id = 2,
                nombre = "xxx",
                apellidos = "xxx",
                correo = "xxx@test.com",
                contrasena = "xxx",
                genero = "xxx"
            };

            var response = await _client.PutAsJsonAsync("/Usuarios/ActualizarUsuario", usuario);

            var result = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsNotNull(result);
            
            //var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            //Assert.IsNotNull(usuario);
            //Assert.That(usuario.nombre.Equals("Juan"));
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/Login.
        /// </summary>
        [Test]
        public async Task Login_Test()
        {
            var loginDTO = new LoginDto("Juan", "abc"); 
            var response = await _client.PostAsJsonAsync("/auth/login", loginDTO);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();

            Assert.IsNotNull(content);

            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            
            string accessToken = root.GetProperty("access_token").GetString()!;
            string tokenType = root.GetProperty("token_type").GetString()!;
            DateTime expiresAtUtc = root.GetProperty("expires_at_utc").GetDateTime();

            Assert.That(accessToken, Is.Not.Empty);
            Assert.That(tokenType, Is.EqualTo("Bearer"));

            // TODO: HAcer DTO para recibir "TokenResponse"
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/CambiarContraseña.
        /// </summary>
        [Test]
        public async Task CambiarContraseña_Test()
        {
            string correo = "juan@test.com";
            string newPassword = "otraPassword";
            var response = await _client.PatchAsync($"/Usuarios/CambiarContraseña?email={correo}&nuevaContraseña={newPassword}", null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();

            //var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            Assert.IsNotNull(content);
            //Assert.That(usuario.nombre.Equals("Juan"));
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/ValidarCuenta.
        /// </summary>
        [Test]
        public async Task ValidarCuenta_Test()
        {
            string correo = "ana@test.com";
            var response = await _client.PatchAsync($"/Usuarios/ValidarCuenta?email={correo}", null);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var result = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(result);

            //var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            //Assert.IsNotNull(usuario);
            //Assert.That(usuario.nombre.Equals("Juan"));
        } 

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/ActivacionSuscripcion.
        /// </summary>
        [Test]
        public async Task ActivacionSuscripcion_Test()
        {
            var correo = "juan@test.com";

            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var token = context.EmailTokens.First().token;


            var response = await _client.PatchAsync($"/Usuarios/ActivacionSuscripcion?token={token}&email={correo}", null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(content);
            Assert.AreEqual(content, "true");

            //var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            //Assert.IsNotNull(usuario);
            //Assert.That(usuario.nombre.Equals("Juan"));
        }

        /// <summary>
        /// Verifica que se puede obtener un usuario ... desde el endpoint /Usuarios/Eliminar/{id}.
        /// </summary>
        [Test]
        public async Task Eliminar_Test()
        {
            int id = 1;
            var response = await _client.DeleteAsync("/Usuarios/Eliminar/" + id);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(content);
            Assert.AreEqual(content, "true");

            //var usuario = await response.Content.ReadFromJsonAsync<Usuario>();
            //Assert.IsNotNull(usuario);
            //Assert.That(usuario.nombre.Equals("Juan"));
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