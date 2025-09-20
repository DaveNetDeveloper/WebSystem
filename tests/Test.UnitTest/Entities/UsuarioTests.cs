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
    public class UsuarioTests : IEntityTest
    {
        [Test]
        public void ValidarCampos_Invalidos_DevuelvenException()
        {
            var usuario = new Usuario();

            // id nulo
            Assert.Throws<ArgumentException>(() => usuario.id = null, "El campo 'id' tiene que estar informado.");

            // nombre vacío o nulo
            Assert.Throws<ArgumentException>(() => usuario.nombre = string.Empty, "El nombre no puede estar vacío."); 
            Assert.Throws<ArgumentException>(() => usuario.nombre = null, "El nombre no puede estar vacío.");

            // apellidos vacíos
            Assert.Throws<ArgumentException>(() => usuario.apellidos = string.Empty, "Los apellidos no pueden estar vacíos."); 
            Assert.Throws<ArgumentException>(() => usuario.apellidos = null, "Los apellidos no pueden estar vacíos.");

            // correo inválido
            Assert.Throws<ArgumentException>(() => usuario.correo = string.Empty, "El email no es válido.");
            Assert.Throws<ArgumentException>(() => usuario.correo = null, "El email no es válido.");
            Assert.Throws<ArgumentException>(() => usuario.correo = "test", "El email no es válido.");
            Assert.Throws<ArgumentException>(() => usuario.correo = "test@", "El email no es válido.");
            Assert.Throws<ArgumentException>(() => usuario.correo = "test.com", "El email no es válido.");
            
            // contraseña vacía
            Assert.Throws<ArgumentException>(() => usuario.contraseña = "", "La contraseña no puede estar vacía.");
            Assert.Throws<ArgumentException>(() => usuario.contraseña = "   ", "La contraseña no puede estar vacía.");
            Assert.Throws<ArgumentException>(() => usuario.contraseña = null, "La contraseña no puede estar vacía.");

            // fechaNacimiento en el futuro
            Assert.Throws<ArgumentException>(() => usuario.fechaNacimiento = DateTime.Now.AddDays(1), "La fecha de nacimiento no puede estar vacía o en el futuro.");

            // ultimaConexion en el futuro
            Assert.Throws<ArgumentException>(() => usuario.ultimaConexion = DateTime.Now.AddMinutes(1), "La fecha de ultima conexion no puede estar en el futuro.");

            // fechaCreación en el futuro
            Assert.Throws<ArgumentException>(() => usuario.fechaCreación = DateTime.Now.AddMinutes(1), "La fecha de creación no puede estar en el futuro.");

            // puntos negativos
            Assert.Throws<ArgumentException>(() => usuario.puntos = -1, "Los puntos no pueden estar en negativo (-).");
        }

        [Test]
        public void ValidarCampos_Validos_AsignanCorrectamente()
        {
            var usuario = new Usuario {
                id = 1,
                nombre = "David",
                apellidos = "García",
                correo = "david@test.com",
                activo = true,
                contraseña = "12345",
                fechaNacimiento = DateTime.Now.AddYears(-30),
                suscrito = true,
                ultimaConexion = DateTime.Now.AddMinutes(-5),
                fechaCreación = DateTime.Now.AddDays(-10),
                puntos = 10,
                token = "token123",
                expiracionToken = DateTime.Now.AddHours(1)
            };

            Assert.AreEqual(1, usuario.id);
            Assert.AreEqual("David", usuario.nombre);
            Assert.AreEqual("García", usuario.apellidos);
            Assert.AreEqual("david@test.com", usuario.correo);
            Assert.IsTrue(usuario.activo);
            Assert.AreEqual("12345", usuario.contraseña);
            Assert.IsTrue(usuario.suscrito);
            Assert.AreEqual(10, usuario.puntos);
            Assert.AreEqual("token123", usuario.token);
            Assert.NotNull(usuario.expiracionToken);
        }
    }
}