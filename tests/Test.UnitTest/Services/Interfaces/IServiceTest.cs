using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Test.UnitTest.Services.Interfaces
{
    public interface IServiceTests
    {
        void SetUp();
        void GetById_Test();
        void GetAll_Test();
        void Add_Test();
        void Update_Test();
        void Remove_Test(); 
    }
}
