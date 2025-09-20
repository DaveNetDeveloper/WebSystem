using Application.Common;
using Application.Interfaces.Common;
using Infrastructure.Persistence;
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
    internal abstract class BaseRepositoryTest<TRepository>
    {
        protected ApplicationDbContext _context; 
        protected DataSeeder.DataSeeder _seeder;
        protected List<Enums.EntityType> _entitiesToSeed;

        [SetUp]
        public virtual void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _seeder = new DataSeeder.DataSeeder(_context);
        }

        [TearDown]
        public void TearDown() {
            _context.Dispose();
        }

        protected IQueryOptions<TRepository> GetQueryOptions(int? page, int? pageSize, string? orderBy, bool descending = false)
        {
            return new QueryOptions<TRepository>
            {
                Page = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                Descending = descending
            };
        }

    }
}
