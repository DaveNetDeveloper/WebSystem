using Application.DTOs.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Test.UnitTest.DataSeeder;
using Test.UnitTest.Repositories.Interfaces;
using Utilities;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;

namespace Test.UnitTest.Repositories 
{
    [Category("UnitTest")]
    [TestFixture]
    internal class UsuarioRepositoryTests : BaseRepositoryTest<Usuario>, IRepositoryTests
    { 
        private IUsuarioRepository _repo; 

        [SetUp]
        public override void Setup()
        { 
            base.Setup();
            _entitiesToSeed = new List<Enums.EntityType> { Enums.EntityType.Usuario };
            _seeder.Count = 5;
            _seeder.Seed(_entitiesToSeed, count: 5, seed: 120);
            _repo = new UsuarioRepository(_context);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAffectedEntities_AfterSetup()
        {
            var usuarios = await _repo.GetAllAsync();
            Assert.That(usuarios.Count, Is.EqualTo(_seeder.Count));

            Assert.Pass("Test passed succesfully. All {0} users have been added succesfully during Setup.", [usuarios.Count()]);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsEntity_WhenIdExist()
        {
            var usuario = await _repo.GetByIdAsync(1);

            Assert.NotNull(usuario);

            Assert.NotNull(usuario.id);
            Assert.Greater(usuario.id, -1);

            Assert.NotNull(usuario.nombre);
            Assert.IsNotEmpty(usuario.nombre);

            Assert.NotNull(usuario.apellidos);
            Assert.IsNotEmpty(usuario.apellidos);

            Assert.NotNull(usuario.correo);
            Assert.IsNotEmpty(usuario.correo);

            Assert.NotNull(usuario.contrasena);
            Assert.IsNotEmpty(usuario.contrasena);

            Assert.NotNull(usuario.token);
            Assert.IsNotEmpty(usuario.token);

            Assert.NotNull(usuario.activo); 

            Assert.NotNull(usuario.fechaNacimiento); 
            Assert.That(usuario.fechaNacimiento, Is.LessThan(DateTime.Now));

            Assert.NotNull(usuario.suscrito); 

            Assert.NotNull(usuario.fechaCreacion); 

            Assert.NotNull(usuario.puntos); 
            Assert.GreaterOrEqual(usuario.puntos, 0);

            Assert.Pass("Test passed succesfully. User with Id {0} has all data properties succesfully informed.", [usuario.id]);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenIdDoesNotExist()
        {
            int nonExistId= -9999;
            var usuario = await _repo.GetByIdAsync(nonExistId);

            Assert.IsNull(usuario); 
            Assert.Pass("Test passed succesfully. User Id {0} doesn't exist.", [nonExistId]);
        }

        [Test]
        public async Task GetByEmailAsync_ReturnsEntity_WhenIdExists()
        {
            int id = 1;
            var usuarioById = await _repo.GetByIdAsync(id);
            Assert.IsNotNull(usuarioById);

            var filters = new UsuarioFilters();
            filters.Correo = usuarioById.correo;

            int? page = null;
            int? pageSize = null;
            string? orderBy = "id";
            bool descending = false;
            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);
             
            var filteredByEmail = await _repo.GetByFiltersAsync(filters, queryOptions);
             
            Assert.IsNotNull(filteredByEmail);
            Assert.AreEqual(filteredByEmail.First().correo, usuarioById.correo);

            Assert.Pass("Test passed succesfully. User with email {0} exist.", [usuarioById.correo]);
        }

        [Test]
        public async Task GetByTokenAsync_ReturnsEntity_WhenIdExists()
        {
            int id = 1;
            var usuarioById = await _repo.GetByIdAsync(id);
            Assert.IsNotNull(usuarioById);

            var filters = new UsuarioFilters();
            filters.Token = usuarioById.token;

            int? page = null;
            int? pageSize = null;
            string? orderBy = null;
            bool descending = false;
            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var filteredByToken = await _repo.GetByFiltersAsync(filters, queryOptions);

            Assert.IsNotNull(filteredByToken);
            Assert.AreEqual(usuarioById.token, filteredByToken.First().token);

            Assert.Pass("Test passed succesfully. User with token {0} exist.", [usuarioById.token]);
        }

        [Test]
        public async Task LoginUser_ReturnsEntity_WithValidData()
        {
            int id = 1;
            var usuariobyId = await _repo.GetByIdAsync(id);
            Assert.IsNotNull(usuariobyId);

            string userName = usuariobyId.nombre;
            string password = "MyPassword-123";
            var authUser = await _repo.Login(userName, password);

            Assert.IsNotNull(authUser);
            Assert.IsNotNull(authUser.Id);
            Assert.IsNotNull(authUser.UserName);

            var roles = await _repo.GetRolesByUsuarioId(authUser.Id);
            if (roles != null && roles.Any()) 
            {
                var maxRole = roles.OrderByDescending(r => r.level).FirstOrDefault();
                authUser.Role = maxRole.nombre;
            }
            else authUser.Role = string.Empty;

            Assert.IsNotNull(authUser.Role);

            //Assert.AreEqual(usuariobyId.id, usuarioByLogin.id);

            //Assert.IsNotNull(usuarioByLogin.expiracionToken);
            //Assert.Greater(usuarioByLogin.expiracionToken, DateTime.UtcNow);

            //Assert.IsNotNull(usuarioByLogin.ultimaConexion);
            //Assert.Less(usuarioByLogin.ultimaConexion, DateTime.UtcNow);

            //Assert.IsNotNull(usuarioByLogin.token);

            ////Assert.DoesNotThrow(() => Convert.FromBase64String(usuarioByLogin.token));
            ////Assert.AreEqual(51, usuarioByLogin.token.Length);

            Assert.Pass("Test passed succesfully. Login successful for user with Id {0}.", [id]);
        }

        [Test]
        public async Task CambiarContraseña_ReturnsEntity_WithValidData()
        {
            int userIdToUpdate = 1;
            var userToUpdate = await _repo.GetByIdAsync(userIdToUpdate);
            Assert.IsNotNull(userToUpdate);

            var newPassword = "TestPassword";
            var result = await _repo.CambiarContraseña(userToUpdate.correo, newPassword);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var filters = new UsuarioFilters();
            filters.Correo = userToUpdate.correo;

            int? page = null;
            int? pageSize = null;
            string? orderBy = null;
            bool descending = false;
            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var userUpdated = await _repo.GetByFiltersAsync(filters, queryOptions);

            Assert.IsNotNull(userUpdated);
            Assert.IsNotNull(userUpdated.First().contrasena); 

            Assert.Pass("Test passed succesfully.Password for user with email {0} has been updated succesfully.", [userIdToUpdate]);
        }

        [Test]
        public async Task ValidarCuenta_ReturnsEntity_WithValidData()
        {
            int userIdToValidate = 1;
            var userToValidate = await _repo.GetByIdAsync(userIdToValidate);
            Assert.IsNotNull(userToValidate);

            var result = await _repo.ValidarCuenta(userToValidate.correo);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var userValidated = await _repo.GetByIdAsync(userIdToValidate);
            Assert.IsNotNull(userValidated);
            Assert.IsTrue(userValidated.activo);

            Assert.Pass("Test passed succesfully. User with Id {0} has been activated succesfully.", [userIdToValidate]);
        }

        [Test]
        public async Task AddAsync_AddEntityCorrectly()
        {   
            var newId = _repo.GetAllAsync().Result.Count() + 1;

            var opcionesGenero = new[] { "Hombre", "Mujer", "Otro" };
            var faker = new Faker<Usuario>()
                .RuleFor(u => u.id, newId)  
                .RuleFor(u => u.nombre, f => f.Person.FirstName)
                .RuleFor(u => u.apellidos, f => f.Person.LastName)
                .RuleFor(u => u.correo, f => f.Internet.Email())
                .RuleFor(u => u.contrasena, f => f.Internet.Password())
                .RuleFor(u => u.activo, f => f.Random.Bool())
                .RuleFor(u => u.fechaNacimiento, f => f.Person.DateOfBirth)
                .RuleFor(u => u.suscrito, f => f.Random.Bool())
                .RuleFor(u => u.fechaCreacion, DateTime.Now)
                .RuleFor(u => u.token, f => f.Random.AlphaNumeric(45))
                .RuleFor(u => u.genero, f => f.PickRandom(opcionesGenero))
                .RuleFor(u => u.puntos, f => f.Random.Number(0, 1000));

            var usuario = faker.Generate(1)[0];
            Assert.NotNull(usuario);

            await _repo.AddAsync(usuario);
            await _context.SaveChangesAsync();

            var dbUsuario = await _repo.GetByIdAsync(newId);
            Assert.NotNull(dbUsuario);
            Assert.AreEqual(newId, dbUsuario.id);

            Assert.Pass("Test passed succesfully. User with new Id {0} has been created succesfully.", [newId]);
        }

        [Test]
        public async Task UpdateAsync_ReturnsEntity_WithValidData()
        {
            int userIdToUpdate = 1;
            var userToUpdate = await _repo.GetByIdAsync(userIdToUpdate);
            Assert.IsNotNull(userToUpdate);

            userToUpdate.nombre = "updated name";
            userToUpdate.apellidos = "updated surnames";
            userToUpdate.activo = !userToUpdate.activo;
            userToUpdate.contrasena = "updated password";
            userToUpdate.correo = "updatedMail@mail.com";
            userToUpdate.fechaNacimiento = DateTime.UtcNow.AddYears(-39);
            userToUpdate.suscrito = !userToUpdate.suscrito;
            userToUpdate.fechaCreacion = DateTime.UtcNow;
            //userToUpdate.token = "updated token";
            userToUpdate.genero = "updated genero";
            userToUpdate.puntos = userToUpdate.puntos + 1000; 

            var result = await _repo.UpdateAsync(userToUpdate);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var userUpdated = await _repo.GetByIdAsync(userToUpdate.id.Value);
            Assert.IsNotNull(userUpdated);

            Assert.AreEqual(userUpdated.nombre, "updated name");
            Assert.AreEqual(userUpdated.apellidos, "updated surnames");
            Assert.IsNotNull(userUpdated.activo);
            Assert.AreEqual(userUpdated.contrasena, "updated password");
            Assert.IsNotNull(userUpdated.suscrito);
            Assert.AreEqual(userUpdated.correo, "updatedMail@mail.com");
            //Assert.IsNotNull(userUpdated.fechaNacimiento);
            Assert.IsNotNull(userUpdated.fechaCreacion);
           // Assert.AreEqual(userUpdated.token, "updated token"); 
            Assert.AreEqual(userUpdated.genero, "updated genero"); 
            Assert.IsNotNull(userUpdated.puntos);

            Assert.Pass("Test passed succesfully.User with id {0} has been updated succesfully.", [userIdToUpdate]);
        }
        
        [Test]
        public async Task RemoveEntity_CheckEntities_AfterRemove()
        {
            int userIdToRemove = 1;
            var userToRemove = await _repo.GetByIdAsync(userIdToRemove);
            Assert.IsNotNull(userToRemove);

            var result = await _repo.Remove(userToRemove.id.Value);
            Assert.IsNotNull(result);

            var usuarios = await _repo.GetAllAsync();
            Assert.IsNotNull(usuarios);
            Assert.That(usuarios.Count, Is.EqualTo(_seeder.Count -1));
        
            Assert.Pass("Test passed succesfully. User with Id {0} has been removed succesfully.", [userIdToRemove]);
        } 
    }
}