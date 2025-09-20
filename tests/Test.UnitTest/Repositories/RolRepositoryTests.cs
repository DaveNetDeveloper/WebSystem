using Application.DTOs.Filters;
using Application.Interfaces.Repositories;
using Bogus;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.UnitTest.DataSeeder;
using Test.UnitTest.Repositories.Interfaces;

namespace Test.UnitTest.Repositories
{
    [Category("UnitTest")]
    [TestFixture]
    internal class RolRepositoryTests : BaseRepositoryTest<Rol>, IRepositoryTests
    { 
        private IRolRepository _repo;

        [SetUp]
        public override void Setup()
        { 
            base.Setup();
            _entitiesToSeed = new List<Enums.EntityType> { Enums.EntityType.Rol };
            _seeder.Count = 3;
            _seeder.Seed(_entitiesToSeed, count: 3, seed: 888);
            _repo = new RolRepository(_context);
        }

        public async Task GetAllAsync_ReturnsAffectedEntities_AfterSetup()
        {
            var roles = await _repo.GetAllAsync();
            Assert.That(roles.Count, Is.EqualTo(_seeder.Count));

            string entityDetails = string.Empty;
            foreach (var rol in roles) {
                entityDetails = entityDetails + $"Id = {rol.id}, Nombre={rol.nombre}, Descripcion={rol.descripcion} ";
                entityDetails = entityDetails + Environment.NewLine;
            }

            Assert.Pass("Test passed succesfully. All {0} roles have been added succesfully during Setup:" + Environment.NewLine + entityDetails, [roles.Count().ToString()]);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsEntity_WhenIdExist()
        {
            var allRoles = await _repo.GetAllAsync();
            Assert.IsNotNull(allRoles);

            var firstRol = await _repo.GetByIdAsync(allRoles.First().id);
            Assert.IsNotNull(firstRol);

            Assert.NotNull(firstRol.id);
            Assert.That(firstRol.id, Is.TypeOf<Guid>());

            Assert.NotNull(firstRol.nombre);
            Assert.IsNotEmpty(firstRol.nombre);

            Assert.NotNull(firstRol.descripcion);
            Assert.IsNotEmpty(firstRol.descripcion);

            Assert.Pass("Test passed succesfully. Rol with Id {0} has all data properties succesfully informed.", [firstRol.id.ToString()]);
        }

        [Test]
        public async Task GetByNameAsync_ReturnsEntity_WhenIdExist()
        {
            var allRoles = await _repo.GetAllAsync();
            Assert.IsNotNull(allRoles);
            var firstRol = await _repo.GetByIdAsync(allRoles.First().id);
            Assert.IsNotNull(firstRol);

            var filters = new RolFilters();
            filters.Nombre = firstRol.nombre;

            int? page = null;
            int? pageSize = null;
            string? orderBy = null;
            bool descending = false;
            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var rolByNameList = await _repo.GetByFiltersAsync(filters, queryOptions);

            var rolByName = rolByNameList.First();
            Assert.NotNull(rolByName);

            Assert.NotNull(rolByName.id);
            Assert.That(rolByName.id, Is.TypeOf<Guid>());
            Assert.AreEqual(rolByName.id, firstRol.id); 

            Assert.NotNull(rolByName.nombre);
            Assert.IsNotEmpty(rolByName.nombre);
            Assert.AreEqual(rolByName.nombre, firstRol.nombre);

            Assert.NotNull(rolByName.descripcion);
            Assert.IsNotEmpty(rolByName.descripcion);
            Assert.AreEqual(rolByName.descripcion, firstRol.descripcion);

            Assert.Pass("Test passed succesfully. Rol with name {0} has all data properties succesfully informed.", [rolByName.nombre]);
        }

        [Test]
        public async Task AddAsync_AddEntityCorrectly()
        {
            var faker = _seeder.GetFaker_Rol();
            var newRol = faker.Generate(1)[0];
            Assert.NotNull(newRol);

            var result = await _repo.AddAsync(newRol);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var createdRol = await _repo.GetByIdAsync(newRol.id);
            Assert.NotNull(createdRol);
            Assert.AreEqual(newRol.id, createdRol.id);

            Assert.Pass("Test passed succesfully. Rol with new Id {0} has been created succesfully.", [newRol.id]);
        }

        [Test]
        public async Task UpdateAsync_ReturnsEntity_WithValidData()
        { 
            var allCategories = await _repo.GetAllAsync();
            var rolToUpdate = await _repo.GetByIdAsync(allCategories.First().id);
            Assert.IsNotNull(rolToUpdate);

            rolToUpdate.nombre = "updated name";
            rolToUpdate.descripcion = "updated description";

            var result = await _repo.UpdateAsync(rolToUpdate);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var updatedRol = await _repo.GetByIdAsync(rolToUpdate.id);
            Assert.IsNotNull(updatedRol);

            Assert.AreEqual(updatedRol.nombre, "updated name");
            Assert.AreEqual(updatedRol.descripcion, "updated description");

            Assert.That(updatedRol.id, Is.TypeOf<Guid>());
            Assert.AreEqual(updatedRol.id, rolToUpdate.id);
             
            Assert.Pass("Test passed succesfully. Rol with id {0} has been updated succesfully.", [rolToUpdate.id.ToString()]);
        }

        [Test]
        public async Task RemoveEntity_CheckEntities_AfterRemove()
        {
            var allRoles = await _repo.GetAllAsync();

            var rolToRemove = await _repo.GetByIdAsync(allRoles.First().id);
            Assert.IsNotNull(rolToRemove);

            var result = await _repo.Remove(rolToRemove.id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var dbRoles = await _repo.GetAllAsync();
            Assert.IsNotNull(dbRoles);
            Assert.That(dbRoles.Count, Is.EqualTo(_seeder.Count - 1));

            Assert.Pass("Test passed succesfully. Rol with Id {0} has been removed succesfully.", [rolToRemove.id.ToString()]);
        } 
    }
}