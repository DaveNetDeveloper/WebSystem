using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;

using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities; 
using Moq;
using Test; 
using Test.UnitTest.Services;
using Test.UnitTest.Services.Interfaces;

using Microsoft.Extensions.Options;

namespace Test.UnitTest.Services
{
    [Category("UnitTest")]
    [TestFixture]
    internal class UsuarioServiceTests : BaseServiceTests<Usuario>, IServiceTests
    {
        private IUsuarioService _userService;
        private IAuthService _authService;
        private Mock<IUsuarioRepository> _mockRepo;

        private ICorreoService _correoService;
        private Mock<ITipoEnvioCorreoRepository> _mockRepoTipoEnvio;

        private ILoginService _loginService;
        private Mock<ILoginRepository> _mockRepoLogin;

        private int? page = null;
        private int? pageSize = null;
        private string? orderBy = "id";
        private bool descending = false;
        private IQueryOptions<Usuario> _queryOptions;
        private IOptions<MailConfiguration> _configOptions;

        [SetUp]
        public void SetUp() {

            //email = "mail@test.com";
            //newPassword = "newPassword";

            _queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            _mockRepoLogin = new Mock<ILoginRepository>(); 
            _mockRepoTipoEnvio = new Mock<ITipoEnvioCorreoRepository>();

            _mockRepo = new Mock<IUsuarioRepository>(); 
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Usuario { id = 1, nombre = "TestUser", apellidos = "apellidos", correo = "mail@test.com", contrasena = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8", token = "XYx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE=" });
            
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Usuario> {
                                                                new Usuario {
                                                                    id = 1,
                                                                    nombre = "TestUser 1",
                                                                    apellidos = "apellidos",
                                                                    correo = "mail@test.com",
                                                                    contrasena = "AQAAAAIAAYagAAAAELW6NstElhq6DQMmb0vLiGZaS8JJ+u0941cgEOyVkulcK+Zg7NGisT2EJ4zxZlLlIQ==",
                                                                    token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRGF2aWQiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImp0aSI6Ijg1N2ZlNWI5LWU4ZTktNDk4OS05MDc0LWE1MDRhZGE4OTlhZSIsIm5iZiI6MTc1ODQ1NTIxOSwiZXhwIjoxNzU4NDU4ODE5LCJpc3MiOiJBUEkiLCJhdWQiOiJBUEkifQ.RteS8s-cUCcdCS95fUqMRqOtzyuePMNJ2KcHk74LkZE"
                                                                },
                                                                new Usuario {
                                                                    id = 2,
                                                                    nombre = "TestUser 2",
                                                                    apellidos = "apellidos",
                                                                    correo = "mail2@test.com",
                                                                    contrasena = "AQAAAAIAAYagAAAAELW6NstElhq6DQMmb0vLiGZaS8JJ+u0941cgEOyVkulcK+Zg7NGisT2EJ4zxZlLlIQ==",
                                                                    token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRGF2aWQiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImp0aSI6Ijg1N2ZlNWI5LWU4ZTktNDk4OS05MDc0LWE1MDRhZGE4OTlhZSIsIm5iZiI6MTc1ODQ1NTIxOSwiZXhwIjoxNzU4NDU4ODE5LCJpc3MiOiJBUEkiLCJhdWQiOiJBUEkifQ.RteS8s-cUCcdCS95fUqMRqOtzyuePMNJ2KcHk74LkZE"
                                                                },
                                                                new Usuario {
                                                                    id = 3,
                                                                    nombre = "TestUser 3",
                                                                    apellidos = "apellidos",
                                                                    correo = "mail3@test.com",
                                                                    contrasena = "AQAAAAIAAYagAAAAELW6NstElhq6DQMmb0vLiGZaS8JJ+u0941cgEOyVkulcK+Zg7NGisT2EJ4zxZlLlIQ==",
                                                                    token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRGF2aWQiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImp0aSI6Ijg1N2ZlNWI5LWU4ZTktNDk4OS05MDc0LWE1MDRhZGE4OTlhZSIsIm5iZiI6MTc1ODQ1NTIxOSwiZXhwIjoxNzU4NDU4ODE5LCJpc3MiOiJBUEkiLCJhdWQiOiJBUEkifQ.RteS8s-cUCcdCS95fUqMRqOtzyuePMNJ2KcHk74LkZE"
                                                                }
                                                            });

            _mockRepo.Setup(r => r.GetByFiltersAsync(It.Is<UsuarioFilters>(f => f.Correo == "mail@test.com"), It.IsAny<QueryOptions<Usuario>>())).ReturnsAsync(new List<Usuario> {
                                                                new Usuario {
                                                                    id = 1,
                                                                    nombre = "TestUser 1",
                                                                    apellidos = "apellidos",
                                                                    correo = "mail@test.com",
                                                                    contrasena = "AQAAAAIAAYagAAAAELW6NstElhq6DQMmb0vLiGZaS8JJ+u0941cgEOyVkulcK+Zg7NGisT2EJ4zxZlLlIQ==",
                                                                    token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRGF2aWQiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImp0aSI6Ijg1N2ZlNWI5LWU4ZTktNDk4OS05MDc0LWE1MDRhZGE4OTlhZSIsIm5iZiI6MTc1ODQ1NTIxOSwiZXhwIjoxNzU4NDU4ODE5LCJpc3MiOiJBUEkiLCJhdWQiOiJBUEkifQ.RteS8s-cUCcdCS95fUqMRqOtzyuePMNJ2KcHk74LkZE"
                                                                } });
            
            _mockRepo.Setup(r => r.ValidarCuenta("mail@test.com")).ReturnsAsync(true);
            _mockRepo.Setup(r => r.Remove(1)).ReturnsAsync(true);
            _mockRepo.Setup(r => r.CambiarContrasena("mail@test.com", It.IsAny<string>())).ReturnsAsync(true);
            _mockRepo.Setup(r => r.Login("mail@test.com", It.IsAny<string>())).ReturnsAsync(new AuthUser() { Id = 1, UserName = "name", Role = "Admin" });
            _mockRepo.Setup(r => r.ActivarSuscripcion("mail@test.com")).ReturnsAsync(true);
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Usuario>())).ReturnsAsync(true);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Usuario>())).ReturnsAsync(true);

            _userService = new UsuarioService(_mockRepo.Object);

            _loginService = new LoginService(_mockRepoLogin.Object);

            _correoService = new CorreoService(_mockRepoTipoEnvio.Object);


            var mailConfig = new MailConfiguration() {
                ServidorSmtp = "",
                UsuarioSmtp = "",
                ContrasenaSmtp = "",
                PuertoSmtp = "",
                LogoURL = ""
            }; 
            _configOptions = Options.Create(mailConfig);

            _authService = new AuthService(_mockRepo.Object, _correoService, _loginService , _configOptions);
        } 
        [Test]
        public void GetById_Test()
        {
            var idUser = 1;
            var result = _userService.GetByIdAsync(idUser);
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(1, result.Result.id);
            Assert.AreEqual("TestUser", result.Result.nombre);
        } 
        [Test]
        public void GetAll_Test()
        { 
            var result = _userService.GetAllAsync();
            Assert.IsNotNull(result.Result); 
        }
        [Test]
        public void GetByEmail_Test()
        {
            var filters = new UsuarioFilters();
            filters.Correo = "mail@test.com";

            //int? page = null;
            //int? pageSize = null;
            //string? orderBy = "id";
            //bool descending = false; 
            //var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);  

            var results =  _userService.GetByFiltersAsync(filters, _queryOptions);

            Assert.IsNotNull(results);

            if (null != results?.Result && results.Result.Any())
                Assert.Pass("La colección de usuarios tiene elementos.");
            else
                Assert.Fail("La colección de usuarios no tiene elementos.");
        }
        [Test]
        public void GetByToken_Test()
        {
            var filters = new UsuarioFilters();
            filters.Token = "XYx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE=";

            int? page = null;
            int? pageSize = null;
            string? orderBy = "id";
            bool descending = false;
            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var results = _userService.GetByFiltersAsync(filters, queryOptions); 

            Assert.IsNotNull(results?.Result);
        }
        [Test]
        public void Login_Test()
        {
            var email = "mail@test.com";
            var password = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";

            var result = _authService.Login(email, password);
            Assert.IsNotNull(result.Result);
        }
        [Test]
        public void ActivarSuscripcion_Test()
        { 
            var email = "mail@test.com";
            var suscrito = true;
            var result = _userService.ActivarSuscripcion(email);
            Assert.IsTrue(result.Result);
        }
        [Test]
        public void CambiarContrasena_Test()
        {
            var email = "mail@test.com"; 
            var nuevaContrasena = "newPassword";
            
            var result = _userService.CambiarContrasena(email, nuevaContrasena);
            Assert.IsTrue(result.Result);
        }
        [Test]
        public void ValidarCuenta_Test()
        {
            var email = "mail@test.com";
            var result = _userService.ValidarCuenta(email);
            Assert.IsTrue(result.Result);
        }
        [Test]
        public void Add_Test()
        {
            Usuario usuario = new Usuario { id = 9999, nombre = "TestUser", apellidos = "apellidos", correo = "mail@test.com", contrasena = "newPassword", token = "", activo=true, suscrito=true, fechaCreacion = DateTime.Now, puntos=0, fechaNacimiento=DateTime.Now.AddYears(-39), genero = "Hombre" };
            var result = _userService.AddAsync(usuario);
            Assert.IsTrue(result.Result);
        }
        [Test]
        public void Update_Test()
        {
            Usuario usuario = new Usuario { id = 1, nombre = "TestUser", apellidos = "apellidos", correo = "mail@test.com", contrasena = "newPassword", token = "VBx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE=", genero = "Hombre" };
            var result = _userService.UpdateAsync(usuario);
            Assert.IsTrue(result.Result);
        } 
        [Test]
        public void Remove_Test()
        {
            var id = 1;
            var result = _userService.Remove(id);
            Assert.IsTrue(result.Result);
        } 
    }
}