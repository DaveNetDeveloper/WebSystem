using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.UnitTest.Entities.Interfaces;

namespace Test.UnitTest.Entities
{
    [Category("UnitTest")]
    [TestFixture]
    public class ActividadTests : IEntityTest
    {
        [Test]
        public void ValidarCampos_Invalidos_DevuelvenException()
        {
            var actividad = new Actividad();

            Assert.IsNotNull(actividad);
            Assert.IsNull(actividad.nombre);
            Assert.IsNull(actividad.descripcion);
            Assert.IsNull(actividad.descripcionCorta);
            Assert.IsFalse(actividad.activo);
            Assert.AreEqual(actividad.idEntidad, 0);
            Assert.AreEqual(actividad.idTipoActividad.ToString(), "00000000-0000-0000-0000-000000000000");

            // id nulo
            // Assert.Throws<ArgumentException>(() => actividad.id = 0, "El campo 'id' tiene que estar informado."); 
        }

        [Test]
        public void ValidarCampos_Validos_AsignanCorrectamente()
        {
            var actividad = new Actividad {
                id = 1,
                nombre = "nombre",
                descripcion = "descripcion",
                descripcionCorta = "descripcion corta",
                activo = true,
                idEntidad = 1,
                idTipoActividad = Guid.NewGuid()

            };

            Assert.AreEqual(1, actividad.id);
            Assert.AreEqual("nombre", actividad.nombre);
            Assert.AreEqual("descripcion", actividad.descripcion); 
            Assert.IsTrue(actividad.activo);
            Assert.AreEqual("descripcion corta", actividad.descripcionCorta); 
            Assert.NotNull(actividad.idTipoActividad);
            Assert.AreEqual(1, actividad.idEntidad); 
        }
    }
}
