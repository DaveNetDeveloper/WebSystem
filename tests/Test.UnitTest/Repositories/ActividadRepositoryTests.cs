using Application.DTOs.Filters;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Test.UnitTest.DataSeeder;
using Test.UnitTest.Repositories.Interfaces;

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
    internal class ActividadRepositoryTests : BaseRepositoryTest<Actividad>, IRepositoryTests
    { 
        private IActividadRepository _repo;
        private ITipoActividadRepository _repoTipoActividad;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _entitiesToSeed = new List<Enums.EntityType> { Enums.EntityType.Actividad };
            _seeder.Count = 5;
            _seeder.Seed(_entitiesToSeed, count: 5, seed: 111);
            _repo = new ActividadRepository(_context);
            _repoTipoActividad = new TipoActividadRepository(_context);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAffectedEntities_AfterSetup()
        {
            var actividades = await _repo.GetAllAsync();
            Assert.That(actividades.Count, Is.EqualTo(_seeder.Count));

            string entities = string.Empty;
            foreach (var actividad in actividades) {
                entities = entities + $"Id={actividad.id}, Nombre={actividad.nombre}, Activo={actividad.activo}";
                entities = entities + Environment.NewLine;
            } 

            Assert.Pass("Test passed succesfully. All {0} activities have been added succesfully during Setup:" + Environment.NewLine + entities, [actividades.Count()]);
        } 

        [Test]
        public async Task GetByIdAsync_ReturnsEntity_WhenIdExist()
        { 
            var actividad = await _repo.GetByIdAsync(1);
             
            Assert.NotNull(actividad);

            Assert.NotNull(actividad.id);
            Assert.Greater(actividad.id, 0);

            Assert.NotNull(actividad.idEntidad);
            Assert.Greater(actividad.idEntidad, 0);

            Assert.NotNull(actividad.idTipoActividad);

            Assert.NotNull(actividad.nombre);
            Assert.IsNotEmpty(actividad.nombre);

            Assert.NotNull(actividad.descripcion);
            Assert.IsNotEmpty(actividad.descripcion);

            Assert.NotNull(actividad.descripcionCorta);
            Assert.IsNotEmpty(actividad.descripcionCorta);

            Assert.NotNull(actividad.informacioExtra);
            Assert.IsNotEmpty(actividad.informacioExtra);

            Assert.NotNull(actividad.popularidad);

            Assert.NotNull(actividad.gratis);

            Assert.NotNull(actividad.activo);

            Assert.NotNull(actividad.fechaInicio); 

            Assert.NotNull(actividad.fechaFin); 

            Assert.NotNull(actividad.ubicacion);
             
            Assert.NotNull(actividad.linkEvento);
            Assert.IsNotEmpty(actividad.linkEvento);

            Assert.NotNull(actividad.linkInstagram);
            Assert.IsNotEmpty(actividad.linkInstagram);

            Assert.NotNull(actividad.linkFacebook);
            Assert.IsNotEmpty(actividad.linkFacebook);

            Assert.NotNull(actividad.linkYoutube);
            Assert.IsNotEmpty(actividad.linkYoutube);

            Assert.Pass("Test passed succesfully. Activity with Id {0} has all data properties succesfully informed.", [actividad.id]);
        }

        [Test]
        public async Task GetByNameAsync_ReturnsEntity_WhenIdExist()
        {
            int activityId = 1;
            var activityById = await _repo.GetByIdAsync(activityId);
            Assert.IsNotNull(activityById);
            
            var filters = new ActividadFilters();
            filters.Nombre = activityById.nombre;

            int? page = null;
            int? pageSize = null;
            string? orderBy = null;
            bool descending = false;
            var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            var activityByNameList = await _repo.GetByFiltersAsync(filters, queryOptions);
            Assert.NotNull(activityByNameList); 

            var activityByName = activityByNameList.Items.First();

            Assert.NotNull(activityByName);

            Assert.NotNull(activityByName.id);
            Assert.Greater(activityByName.id, 0);

            Assert.NotNull(activityByName.idEntidad);
            Assert.Greater(activityByName.idEntidad, 0);

            Assert.NotNull(activityByName.idTipoActividad);

            Assert.NotNull(activityByName.nombre);
            Assert.IsNotEmpty(activityByName.nombre);

            Assert.NotNull(activityByName.descripcion);
            Assert.IsNotEmpty(activityByName.descripcion);

            Assert.NotNull(activityByName.descripcionCorta);
            Assert.IsNotEmpty(activityByName.descripcionCorta);

            Assert.NotNull(activityByName.informacioExtra);
            Assert.IsNotEmpty(activityByName.informacioExtra);

            Assert.NotNull(activityByName.popularidad);

            Assert.NotNull(activityByName.gratis);

            Assert.NotNull(activityByName.activo);

            Assert.NotNull(activityByName.fechaInicio);

            Assert.NotNull(activityByName.fechaFin);

            Assert.NotNull(activityByName.ubicacion);

            Assert.NotNull(activityByName.linkEvento);
            Assert.IsNotEmpty(activityByName.linkEvento);

            Assert.NotNull(activityByName.linkInstagram);
            Assert.IsNotEmpty(activityByName.linkInstagram);

            Assert.NotNull(activityByName.linkFacebook);
            Assert.IsNotEmpty(activityByName.linkFacebook);

            Assert.NotNull(activityByName.linkYoutube);
            Assert.IsNotEmpty(activityByName.linkYoutube);

            Assert.Pass("Test passed succesfully. Activity with name {0} has all data properties succesfully informed.", [activityByName.nombre]);
        }

        
        [Test]
        public async Task AddAsync_AddEntityCorrectly()
        {    
            var faker = _seeder.GetFaker_Actividad(); 
            var newActivity = faker.Generate(1)[0];
            Assert.NotNull(newActivity);

            var result = await _repo.AddAsync(newActivity);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var createdctivity = await _repo.GetByIdAsync(newActivity.id);
            Assert.NotNull(createdctivity);
            Assert.AreEqual(newActivity.id, createdctivity.id);

            Assert.Pass("Test passed succesfully. Activity with new Id {0} has been created succesfully.", [newActivity.id.ToString()]);
        }

        [Test]
        public async Task UpdateAsync_ReturnsEntity_WithValidData()
        {
            int activityIdToUpdate = 1;
            var activityToUpdate = await _repo.GetByIdAsync(activityIdToUpdate);
            Assert.IsNotNull(activityToUpdate);

            activityToUpdate.nombre = "updated name";
            activityToUpdate.descripcion = "updated description";
            activityToUpdate.activo = false;
            activityToUpdate.popularidad = 5;

            var result = await _repo.UpdateAsync(activityToUpdate);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var activityUpdated = await _repo.GetByIdAsync(activityToUpdate.id);
            Assert.IsNotNull(activityUpdated);

            Assert.AreEqual(activityUpdated.nombre, "updated name");
            Assert.AreEqual(activityUpdated.descripcion, "updated description");
            Assert.IsFalse(activityUpdated.activo);
            Assert.AreEqual(activityUpdated.popularidad, 5);

            Assert.Pass("Test passed succesfully.Activity with id {0} has been updated succesfully.", [activityIdToUpdate]);
        }

        [Test]
        public async Task RemoveEntity_CheckEntities_AfterRemove()
        {
            int activityIdToRemove = 1;
            var activityToRemove = await _repo.GetByIdAsync(activityIdToRemove);
            Assert.IsNotNull(activityToRemove);

            var result = await _repo.Remove(activityToRemove.id);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            var dbActivities = await _repo.GetAllAsync();
            Assert.IsNotNull(dbActivities);
            Assert.That(dbActivities.Count, Is.EqualTo(_seeder.Count - 1));

            Assert.Pass("Test passed succesfully. Activity with Id {0} has been removed succesfully.", [activityIdToRemove]);
        }

        [Test]
        public async Task GetActividadesByEntidad_ReturnsActivities_WhenIdExist()
        {
            int entidadId = 1;
            var activitiesByEntity = await _repo.GetActividadesByEntidad(entidadId);
            Assert.IsNotNull(activitiesByEntity);  

            Assert.Pass();

            //int? page = null;
            //int? pageSize = null;
            //string? orderBy = null;
            //bool descending = false;
            //var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            //var activityByNameList = await _repo.GetByFiltersAsync(filters, queryOptions);
            //Assert.NotNull(activityByNameList);

            //var activityByName = activityByNameList.Items.First(); 

            //Assert.Pass("Test passed succesfully. Activity with name {0} has all data properties succesfully informed.", [activityByName.nombre]);
        }

        [Test]
        public async Task GetActividadesByTipoActividad_ReturnsActivities_WhenIdExist()
        { 
            var idTipoActividad = _repoTipoActividad.GetAllAsync()?.Result?.First()?.id; 

            var activitiesByTipoActividad = await _repo.GetActividadesByTipoActividad(idTipoActividad.Value);
            Assert.IsNotNull(activitiesByTipoActividad);
            Assert.Greater(activitiesByTipoActividad.Count(), 0);

            Assert.Pass();

            //int? page = null;
            //int? pageSize = null;
            //string? orderBy = null;
            //bool descending = false;
            //var queryOptions = GetQueryOptions(page, pageSize, orderBy, descending);

            //var activityByNameList = await _repo.GetByFiltersAsync(filters, queryOptions);
            //Assert.NotNull(activityByNameList);

            //var activityByName = activityByNameList.Items.First(); 

            //Assert.Pass("Test passed succesfully. Activity with name {0} has all data properties succesfully informed.", [activityByName.nombre]);
        }

        [Test]
        public async Task GetImagenesActividad_ReturnsImages_WhenIdExist()
        {
            int entidadId = 1;
            var imagesActivity = await _repo.GetImagenesByActividad(entidadId);
            Assert.IsNotNull(imagesActivity);

            Assert.Pass(); 
        }

    }
}