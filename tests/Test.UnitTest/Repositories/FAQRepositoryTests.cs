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
    internal class FAQRepositoryTests : BaseRepositoryTest<FAQ> //, IRepositoryTests
    { 
        private IFAQRepository _repo;

        public override void Setup()
        {
            base.Setup();
            _entitiesToSeed = new List<Enums.EntityType> { Enums.EntityType.FAQ };
            _seeder.Count = 5;
            _seeder.Seed(_entitiesToSeed, count: 5, seed: 555);

            _repo = new FAQRepository(_context);
        }
    }
}
