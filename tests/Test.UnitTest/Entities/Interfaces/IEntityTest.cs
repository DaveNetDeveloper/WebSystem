 using System.Threading.Tasks; 

namespace Test.UnitTest.Entities.Interfaces
{
    public interface IEntityTest
    {
        //void ValidarCampos_Invalidos_DevuelvenException();
        void ValidarCampos_Validos_AsignanCorrectamente();
    }
}