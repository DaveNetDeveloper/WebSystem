 using System.Threading.Tasks; 

namespace Test.UnitTest.Repositories.Interfaces
{
    public interface IRepositoryTests
    { 
        // TODO Repensar los titulos de los metodos para que sean genericos y comunes para todos los IRepositoryTests
        void Setup();
        Task GetAllAsync_ReturnsAffectedEntities_AfterSetup();
        Task GetByIdAsync_ReturnsEntity_WhenIdExist();
        //Task GetByIdAsync_ReturnsNull_WhenIdDoesNotExist(); TODO Implementar este test en los repositoryTest
        //Task GetByNameAsync_ReturnsEntity_WhenIdExist();
        Task AddAsync_AddEntityCorrectly();
        Task UpdateAsync_ReturnsEntity_WithValidData();
        Task RemoveEntity_CheckEntities_AfterRemove(); 
    }
}