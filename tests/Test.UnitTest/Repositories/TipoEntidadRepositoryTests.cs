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

namespace Test.UnitTest.Repositories
{
    [Category("UnitTest")]
    [TestFixture]
    internal class TipoEntidadRepositoryTests : BaseRepositoryTest<TipoEntidad> //, IRepositoryTests
    { 
        private ITipoEntidadRepository _repo; 

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _entitiesToSeed = new List<Enums.EntityType> { Enums.EntityType.TipoEntidad };
            _seeder.Count = 5;
            _seeder.Seed(_entitiesToSeed, count: 5, seed: 101);
            _repo = new TipoEntidadRepository(_context);
        }
    }
}
