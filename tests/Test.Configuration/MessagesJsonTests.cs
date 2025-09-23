using System.Text.Json; 

namespace Test.Configuration
{
    [Category("Configuration")]
    [TestFixture]
    internal class MessagesJsonTests
    {
        private string _messagesFilePath;

        [SetUp]
        public void Setup()
        {
            var baseDir = AppContext.BaseDirectory;  
            var solutionDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "..")); 
            _messagesFilePath = Path.Combine(solutionDir, "src", "API", "Resources", "messages.json"); 
        }

        [Test]
        public void MessagesJson_ShouldExist()
        {
            Assert.IsTrue(File.Exists(_messagesFilePath), $"El fichero {_messagesFilePath} no existe.");
        }

        [Test]
        public void MessagesJson_ShouldBeValidJson()
        {
            var jsonString = File.ReadAllText(_messagesFilePath);
            Assert.DoesNotThrow(() => JsonDocument.Parse(jsonString), "El fichero messages.json no es un JSON válido.");
        }

        [Test]
        public void MessagesJson_ShouldContainExpectedRoot()
        {
            var jsonString = File.ReadAllText(_messagesFilePath);
            using var doc = JsonDocument.Parse(jsonString);
            Assert.IsTrue(doc.RootElement.TryGetProperty("Messages", out _), "El JSON no contiene la propiedad raíz 'Messages'.");
        }

        [Test]
        public void AllActions_ShouldHaveSuccessAndErrorKeys()
        {
            var jsonString = File.ReadAllText(_messagesFilePath); 
            using var doc = JsonDocument.Parse(jsonString); 
            var messagesRoot = doc.RootElement.GetProperty("Messages");

            foreach (var entity in messagesRoot.EnumerateObject()) {

                var actions = entity.Value.EnumerateObject();

                foreach (var action in actions) {
                    var actionName = action.Name;
                    var actionValue = action.Value;

                    Assert.IsTrue(actionValue.TryGetProperty("Success", out var successProp),
                        $"La acción '{entity.Name}.{actionName}' no tiene la clave 'Success'");
                    //Assert.IsFalse(string.IsNullOrWhiteSpace(successProp.GetString()),
                        //$"La acción '{entity.Name}.{actionName}' tiene 'Success' vacío");

                    Assert.IsTrue(actionValue.TryGetProperty("Error", out var errorProp),
                        $"La acción '{entity.Name}.{actionName}' no tiene la clave 'Error'");
                    //Assert.IsFalse(string.IsNullOrWhiteSpace(errorProp.GetString()),
                       // $"La acción '{entity.Name}.{actionName}' tiene 'Error' vacío");
                }
            }
        }
    }
}