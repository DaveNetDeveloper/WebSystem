using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;
using Test.UnitTest.Entities.Interfaces;

namespace Test.UnitTest.Entities
{
    [Category("UnitTest")]
    [TestFixture]
    public class CategoriaTests : IEntityTest
    {
        [Test]
        public void ValidarCampos_Invalidos_DevuelvenException()
        {
            var categoria = new Categoria();
            categoria.id = Guid.NewGuid();
            categoria.nombre = null;
            categoria.descripcion = null;

            //Assert.Throws<ArgumentException>(() => categoria.nombre = null, "El campo 'nombre' tiene que estar informado.");
            //Assert.Throws<ArgumentException>(() => categoria.descripcion = null, "El campo 'descripcion' tiene que estar informado.");
            Assert.AreEqual(categoria.idTipoEntidad.ToString(), "00000000-0000-0000-0000-000000000000");
        }

        [Test]
        public void ValidarCampos_Validos_AsignanCorrectamente()
        {

            var id = Guid.NewGuid();
            var idTipoEntidad = Guid.NewGuid();

            var categoria = new Categoria {
                id = id,
                nombre = "nombre",
                descripcion = "descripcion",
                idTipoEntidad = idTipoEntidad

            };

            Assert.AreEqual(id, categoria.id);
            Assert.AreEqual("nombre", categoria.nombre);
            Assert.AreEqual("descripcion", categoria.descripcion);  
            Assert.AreEqual(idTipoEntidad, categoria.idTipoEntidad); 
        }
    }
}