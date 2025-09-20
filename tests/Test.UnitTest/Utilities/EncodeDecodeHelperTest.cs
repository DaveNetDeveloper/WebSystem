using NUnit.Framework;
using System;
using System.Text;
 
using Utilities;

namespace Test.UnitTest.Utilities
{
    [Category("UnitTest")]
    [TestFixture]
    internal class EncodeDecodeHelperTest
    {   
        [SetUp]
        public void Setup() 
        {  

        }

        [Test]
        public void GetEncodeValue_DeberiaRetornarBase64()
        {
            // Arrange
            string textoPlano = "Texto de prueba";

            // Act
            string encoded = EncodeDecodeHelper.GetEncodeValue(textoPlano);

            // Assert
            // Verifico que sea un Base64 válido
            Assert.DoesNotThrow(() => Convert.FromBase64String(encoded));
            Assert.IsNotEmpty(encoded);
        }

        [Test]
        public void GetDecodeValue_DeberiaRetornarTextoPlano()
        {
            // Arrange
            string textoPlano = "Texto de prueba";
            string encoded = EncodeDecodeHelper.GetEncodeValue(textoPlano);

            // Act
            string decoded = EncodeDecodeHelper.GetDecodeValue(encoded);

            // Assert
            Assert.AreEqual(textoPlano, decoded);
        } 
    }
}