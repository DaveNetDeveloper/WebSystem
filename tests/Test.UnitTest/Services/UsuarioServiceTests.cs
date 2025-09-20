using Application.DTOs.Filters;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities; 
using Moq;
using Test; 
using Test.UnitTest.Services;
using Test.UnitTest.Services.Interfaces;

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

        [SetUp]
        public void SetUp() {

            _mockRepoLogin = new Mock<ILoginRepository>();

            _mockRepoTipoEnvio = new Mock<ITipoEnvioCorreoRepository>();

            _mockRepo = new Mock<IUsuarioRepository>(); 
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Usuario { id = 1, nombre = "TestUser", apellidos = "apellidos", correo = "mail@test.com", contraseña = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8", token = "XYx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE=" });
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Usuario> {
                                                                new Usuario {
                                                                    id = 1,
                                                                    nombre = "TestUser 1",
                                                                    apellidos = "apellidos",
                                                                    correo = "mail@test.com",
                                                                    contraseña = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                                                                    token = "XYx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE="
                                                                },
                                                                new Usuario {
                                                                    id = 2,
                                                                    nombre = "TestUser 2",
                                                                    apellidos = "apellidos",
                                                                    correo = "mail@test.com",
                                                                    contraseña = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                                                                    token = "XYx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE="
                                                                },
                                                                new Usuario {
                                                                    id = 3,
                                                                    nombre = "TestUser 3",
                                                                    apellidos = "apellidos",
                                                                    correo = "mail@test.com",
                                                                    contraseña = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                                                                    token = "XYx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE="
                                                                }
                                                            });


            _mockRepo.Setup(r => r.ValidarCuenta("mail@test.com")).ReturnsAsync(true); 
            
            _userService = new UsuarioService(_mockRepo.Object);

            _loginService = new LoginService(_mockRepoLogin.Object);
            _correoService = new CorreoService(_mockRepoTipoEnvio.Object);

            _authService = new AuthService(_mockRepo.Object, _correoService, _loginService);
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

            int? page = null;
            int? pageSize = null;
            string? orderBy = "id";
            bool descending = false; 
            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var results =  _userService.GetByFiltersAsync(filters, queryOptions);

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
        public void CambiarContraseña_Test()
        { 
            var email = "mail@test.com"; 
            var nuevaContraseña = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8";
            var result = _userService.CambiarContraseña(email, nuevaContraseña);
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
            Usuario usuario = new Usuario { id = 9999, nombre = "TestUser", apellidos = "apellidos", correo = "mail@test.com", contraseña = "newPassword", token = "", activo=true, suscrito=true, fechaCreación = DateTime.Now, puntos=0, fechaNacimiento=DateTime.Now.AddYears(-39) };
            var result = _userService.AddAsync(usuario);
            Assert.IsTrue(result.Result);
        }
        [Test]
        public void Update_Test()
        {
            Usuario usuario = new Usuario { id = 1, nombre = "TestUser", apellidos = "apellidos", correo = "mail@test.com", contraseña = "newPassword", token = "VBx7U8rYIFKEhx/A8k6uDFpK9mjNpe9MhU7+lY1URKE=" };
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