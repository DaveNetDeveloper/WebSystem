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
    internal class ProductoRepositoryTests : BaseRepositoryTest<Producto> , IRepositoryTests
    { 
        private IProductoRepository _repo; 

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _entitiesToSeed = new List<Enums.EntityType> { Enums.EntityType.Producto };
            _seeder.Count = 5;
            _seeder.Seed(_entitiesToSeed, count: 5, seed: 666);
            _repo = new ProductoRepository(_context);
        }
        
        //
        public async Task GetAllAsync_ReturnsAffectedEntities_AfterSetup()
        {
            var productos = await _repo.GetAllAsync();
            Assert.That(productos.Count, Is.EqualTo(_seeder.Count));

            string entityDetails = string.Empty;
            foreach (var producto in productos) {
                entityDetails = entityDetails + $"Id = {producto.id.ToString()}, Nombre={producto.nombre}, Descripcion={producto.descripcionCorta} ";
                entityDetails = entityDetails + Environment.NewLine;
            }

            Assert.Pass("Test passed succesfully. All {0} products have been added succesfully during Setup:" + Environment.NewLine + entityDetails, [productos.Count().ToString()]);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsEntity_WhenIdExist()
        {
            var allProducts = await _repo.GetAllAsync();
            Assert.IsNotNull(allProducts);

            var firstProduct = await _repo.GetByIdAsync(allProducts.First().id);
            Assert.IsNotNull(firstProduct);

            Assert.NotNull(firstProduct.id);

            Assert.NotNull(firstProduct.idEntidad);
            Assert.That(firstProduct.idEntidad, Is.TypeOf<int>());

            Assert.NotNull(firstProduct.nombre);
            Assert.IsNotEmpty(firstProduct.nombre);

            Assert.NotNull(firstProduct.descripcion);
            Assert.IsNotEmpty(firstProduct.descripcion);

            Assert.NotNull(firstProduct.puntos);
            Assert.Greater(firstProduct.puntos, 0);
            Assert.That(firstProduct.puntos, Is.TypeOf<int>());

            Assert.NotNull(firstProduct.activo);
            Assert.That(firstProduct.activo, Is.TypeOf<bool>());

            //Assert.NotNull(firstProduct.descripcionCorta);
            //Assert.IsNotEmpty(firstProduct.descripcionCorta); 

            //Assert.NotNull(firstProduct.popularidad);
            //Assert.Greater(firstProduct.popularidad, 0);
            //Assert.Less(firstProduct.popularidad, 5);

            //Assert.NotNull(firstProduct.precio);
            //Assert.Greater(firstProduct.precio, 0);
            //Assert.That(firstProduct.puntos, Is.TypeOf<float?>());
             
            //Assert.NotNull(firstProduct.imagen);
            //Assert.IsNotEmpty(firstProduct.imagen); 

            //Assert.NotNull(firstProduct.disponible); 

            //Assert.NotNull(firstProduct.informacioExtra);
            //Assert.IsNotEmpty(firstProduct.informacioExtra); 

            //Assert.NotNull(firstProduct.linkInstagram);
            //Assert.IsNotEmpty(firstProduct.linkInstagram); 

            //Assert.NotNull(firstProduct.linkFacebook);
            //Assert.IsNotEmpty(firstProduct.linkFacebook); 

            //Assert.NotNull(firstProduct.linkYoutube);
            //Assert.IsNotEmpty(firstProduct.linkYoutube); 

            Assert.Pass("Test passed succesfully. Product with Id {0} has all data properties succesfully informed.", [firstProduct.id.ToString()]);
        }

        [Test]
        public async Task GetByNameAsync_ReturnsEntity_WhenIdExist()
        {
            var allProducts = await _repo.GetAllAsync();
            Assert.IsNotNull(allProducts);

            var firstProduct = await _repo.GetByIdAsync(allProducts.First().id);
            Assert.IsNotNull(firstProduct);

            var filters = new ProductoFilters();
            filters.Nombre = firstProduct.nombre;

            int? page = null;
            int? pageSize = null;
            string? orderBy = null;
            bool descending = false;
            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var productByNameList = await _repo.GetByFiltersAsync(filters, queryOptions);
             
            var productByName = productByNameList.First();
            Assert.NotNull(productByName);

            Assert.NotNull(productByName.id);
            Assert.That(productByName.id, Is.TypeOf<int>());
            Assert.AreEqual(productByName.id, firstProduct.id);

            Assert.NotNull(productByName.idEntidad);
            Assert.That(productByName.idEntidad, Is.TypeOf<int>());
            Assert.AreEqual(productByName.idEntidad, firstProduct.idEntidad);

            Assert.NotNull(productByName.nombre);
            Assert.IsNotEmpty(productByName.nombre);
            Assert.AreEqual(productByName.nombre, firstProduct.nombre);

            Assert.NotNull(productByName.descripcion);
            Assert.IsNotEmpty(productByName.descripcion);
            Assert.AreEqual(productByName.descripcion, firstProduct.descripcion);           

            Assert.NotNull(productByName.activo);
            Assert.That(productByName.activo, Is.TypeOf<bool>());
            Assert.AreEqual(productByName.activo, firstProduct.activo);

            Assert.NotNull(productByName.puntos);
            Assert.That(productByName.puntos, Is.TypeOf<int>());
            Assert.GreaterOrEqual(productByName.puntos, 0);
            Assert.AreEqual(productByName.puntos, firstProduct.puntos);

            Assert.Pass("Test passed succesfully. Product with name {0} has all data properties succesfully informed.", [productByName.nombre]);
        }

        [Test]
        public async Task AddAsync_AddEntityCorrectly()
        {
            var newId = _repo.GetAllAsync().Result.Count() + 1;

            var faker = _seeder.GetFaker_Producto();
            var newProduct = faker.Generate(1)[0];
            Assert.NotNull(newProduct);

            newProduct.id = newId;
            _context.Entry(newProduct).State = EntityState.Detached;  
            var result = await _repo.AddAsync(newProduct);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var createdProduct = await _repo.GetByIdAsync(newProduct.id);
            Assert.NotNull(createdProduct);

            Assert.NotNull(createdProduct.id); 
            Assert.AreEqual(newProduct.id, createdProduct.id); 

            Assert.NotNull(createdProduct.nombre);
            Assert.NotNull(createdProduct.idEntidad);
            Assert.NotNull(createdProduct.activo);
            Assert.NotNull(createdProduct.descripcion);
            Assert.NotNull(createdProduct.puntos); 

            Assert.Pass("Test passed succesfully. Product with new Id {0} has been created succesfully.", [newProduct.id]);
        }

        [Test]
        public async Task UpdateAsync_ReturnsEntity_WithValidData()
        {
            // Get any existing entity data for tests
            var allProducts = await _repo.GetAllAsync();
            var productToUpdate = await _repo.GetByIdAsync(allProducts.First().id);
            Assert.IsNotNull(productToUpdate);

            productToUpdate.nombre = "updated name";
            productToUpdate.descripcion = "updated description";
            productToUpdate.idEntidad = 1;
            productToUpdate.precio = 10;
            productToUpdate.activo = true;

            var result = await _repo.UpdateAsync(productToUpdate);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var updatedProduct = await _repo.GetByIdAsync(productToUpdate.id);
            Assert.IsNotNull(updatedProduct);

            Assert.AreEqual(updatedProduct.id, productToUpdate.id);

            Assert.AreEqual(updatedProduct.nombre, "updated name");
            Assert.AreEqual(updatedProduct.descripcion, "updated description"); 
            Assert.AreEqual(updatedProduct.idEntidad, 1); 
            Assert.AreEqual(updatedProduct.precio, 10);
            Assert.AreEqual(updatedProduct.activo, true); ;
            
            Assert.Pass("Test passed succesfully. Product with id {0} has been updated succesfully.", [productToUpdate.id, ToString()]);
        }

        [Test]
        public async Task RemoveEntity_CheckEntities_AfterRemove()
        {
            var allProducts = await _repo.GetAllAsync();

            var productToRemove = await _repo.GetByIdAsync(allProducts.First().id);
            Assert.IsNotNull(productToRemove);

            var result = await _repo.Remove(productToRemove.id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var updatedProduct = await _repo.GetAllAsync();
            Assert.IsNotNull(updatedProduct);
            Assert.That(updatedProduct.Count, Is.EqualTo(_seeder.Count - 1));

            Assert.Pass("Test passed succesfully. Product with Id {0} has been removed succesfully.", [productToRemove.id.ToString()]);
        }
    }
}