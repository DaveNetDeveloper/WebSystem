using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bogus;

using Domain.Entities;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Test.UnitTest.Repositories.Interfaces;
using Test.UnitTest.DataSeeder;
using Application.DTOs.Filters;

namespace Test.UnitTest.Repositories
{
    [Category("UnitTest")]
    [TestFixture]
    internal class CategoriaRepositoryTests : BaseRepositoryTest<Categoria>, IRepositoryTests
    { 
        private ICategoriaRepository _repo;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _entitiesToSeed = new List<Enums.EntityType> { Enums.EntityType.Categoria };
            _seeder.Count = 5;
            _seeder.Seed(_entitiesToSeed, count: 5, seed: 333);
            _repo = new CategoriaRepository(_context);
        }

        public async Task GetAllAsync_ReturnsAffectedEntities_AfterSetup()
        {
            var categorias = await _repo.GetAllAsync();
            Assert.That(categorias.Count, Is.EqualTo(_seeder.Count));

            string entityDetails = string.Empty;
            foreach (var category in categorias) {
                entityDetails = entityDetails + $"Id = {category.id}, Nombre={category.nombre}, Descripcion={category.descripcion} ";
                entityDetails = entityDetails + Environment.NewLine;
            }

            Assert.Pass("Test passed succesfully. All {0} categories have been added succesfully during Setup:" + Environment.NewLine + entityDetails, [categorias.Count().ToString()]);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsEntity_WhenIdExist()
        { 
            var allCategories = await _repo.GetAllAsync();
            Assert.IsNotNull(allCategories);
           
            var firstCategory = await _repo.GetByIdAsync(allCategories.First().id);
            Assert.IsNotNull(firstCategory);

            Assert.NotNull(firstCategory.id);

            Assert.NotNull(firstCategory.idTipoEntidad);

            Assert.NotNull(firstCategory.nombre);
            Assert.IsNotEmpty(firstCategory.nombre);

            Assert.NotNull(firstCategory.descripcion);
            Assert.IsNotEmpty(firstCategory.descripcion);

            Assert.Pass("Test passed succesfully. Category with Id {0} has all data properties succesfully informed.", [firstCategory.id.ToString()]);
        }

        [Test]
        public async Task GetByNameAsync_ReturnsEntity_WhenIdExist()
        {
            var allCategories = await _repo.GetAllAsync();
            Assert.IsNotNull(allCategories);
            var firstCategory = await _repo.GetByIdAsync(allCategories.First().id);
            Assert.IsNotNull(firstCategory);

            var filters = new CategoriaFilters();
            filters.Nombre = firstCategory.nombre;

            int? page = null;
            int? pageSize = null;
            string? orderBy = null;
            bool descending = false;
            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var categoriaByNameList = await _repo.GetByFiltersAsync(filters, queryOptions);

            var categoryByName = categoriaByNameList.First(); 
            Assert.NotNull(categoryByName);

            Assert.NotNull(categoryByName.id);
            Assert.That(categoryByName.id, Is.TypeOf<Guid>());
            Assert.AreEqual(categoryByName.id, firstCategory.id);

            Assert.NotNull(categoryByName.idTipoEntidad);
            Assert.That(categoryByName.idTipoEntidad, Is.TypeOf<Guid>());
            Assert.AreEqual(categoryByName.idTipoEntidad, firstCategory.idTipoEntidad); 

            Assert.NotNull(categoryByName.nombre);
            Assert.IsNotEmpty(categoryByName.nombre);
            Assert.AreEqual(categoryByName.nombre, firstCategory.nombre);

            Assert.NotNull(categoryByName.descripcion);
            Assert.IsNotEmpty(categoryByName.descripcion);
            Assert.AreEqual(categoryByName.descripcion, firstCategory.descripcion);

            Assert.Pass("Test passed succesfully. Category with name {0} has all data properties succesfully informed.", [categoryByName.nombre]);
        }

        [Test]
        public async Task AddAsync_AddEntityCorrectly()
        {  
            var faker = _seeder.GetFaker_Categoria();
            var newCategory = faker.Generate(1)[0];
            Assert.NotNull(newCategory);

            var result = await _repo.AddAsync(newCategory);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var createdCategory = await _repo.GetByIdAsync(newCategory.id);
            Assert.NotNull(createdCategory);
            Assert.AreEqual(newCategory.id, createdCategory.id);

            Assert.Pass("Test passed succesfully. Category with new Id {0} has been created succesfully.", [newCategory.id.ToString()]);
        }

        [Test]
        public async Task UpdateAsync_ReturnsEntity_WithValidData()
        { 
            var allCategories = await _repo.GetAllAsync();  
            var categoryToUpdate = await _repo.GetByIdAsync(allCategories.First().id);
            Assert.IsNotNull(categoryToUpdate);

            categoryToUpdate.nombre = "updated name";
            categoryToUpdate.descripcion = "updated description";

            var result = await _repo.UpdateAsync(categoryToUpdate);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var categoryUpdated = await _repo.GetByIdAsync(categoryToUpdate.id);
            Assert.IsNotNull(categoryUpdated);

            Assert.AreEqual(categoryUpdated.nombre, "updated name");
            Assert.AreEqual(categoryUpdated.descripcion, "updated description"); 
            Assert.AreEqual(categoryUpdated.id, categoryToUpdate.id);
            Assert.AreEqual(categoryUpdated.idTipoEntidad, categoryToUpdate.idTipoEntidad);

            Assert.Pass("Test passed succesfully. Category with id {0} has been updated succesfully.", [categoryToUpdate.id,ToString()]);
        }

        [Test]
        public async Task RemoveEntity_CheckEntities_AfterRemove()
        {
            var allCategories = await _repo.GetAllAsync(); 

            var categoryToRemove = await _repo.GetByIdAsync(allCategories.First().id);
            Assert.IsNotNull(categoryToRemove);

            var result = await _repo.Remove(categoryToRemove.id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var dbCategories = await _repo.GetAllAsync();
            Assert.IsNotNull(dbCategories);
            Assert.That(dbCategories.Count, Is.EqualTo(_seeder.Count - 1));

            Assert.Pass("Test passed succesfully. Category with Id {0} has been removed succesfully.", [categoryToRemove.id.ToString()]);
        } 

    }
}