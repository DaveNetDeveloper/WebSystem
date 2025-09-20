using System.Text.Json;

namespace Test.Configuration
{
    [Category("Configuration")]
    [TestFixture]
    internal class AppSettingsJsonTests
    {
        private string _appSettingsPath; 
        private JsonElement _root;

        [OneTimeSetUp]
        public void Setup()
        {
            string baseDir = AppContext.BaseDirectory;
            string solutionDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", ".."));
            _appSettingsPath = Path.Combine(solutionDir, "API", "appsettings.json");

            string jsonString = File.ReadAllText(_appSettingsPath);
            JsonDocument doc = JsonDocument.Parse(jsonString);
            _root = doc.RootElement.Clone(); 
        }

        [Test]
        public void AppSettingsJson_ShouldExist()
        {
            Assert.IsTrue(File.Exists(_appSettingsPath), $"El fichero {_appSettingsPath} no existe.");
        }

        [Test]
        public void AppSettingsJson_ShouldBeValidJson()
        {
            var jsonString = File.ReadAllText(_appSettingsPath);
            Assert.DoesNotThrow(() => JsonDocument.Parse(jsonString), "El fichero appsettings.json no es un JSON válido.");
        }

        [Test]
        public void ConnectionStrings_ShouldBeValid()
        {
            Assert.IsTrue(_root.TryGetProperty("ConnectionStrings", out var conn), "Falta 'ConnectionStrings'");
            Assert.IsTrue(conn.TryGetProperty("DefaultConnection", out var defaultConn), "Falta 'DefaultConnection'");
            Assert.IsFalse(string.IsNullOrWhiteSpace(defaultConn.GetString()), "'DefaultConnection' está vacío");
        }

        [Test]
        public void Logging_ShouldBeValid()
        {
            Assert.IsTrue(_root.TryGetProperty("Logging", out var logging), "Falta 'Logging'");
            Assert.IsTrue(logging.TryGetProperty("LogLevel", out var logLevel), "Falta 'Logging:LogLevel'");
            Assert.IsTrue(logLevel.TryGetProperty("Default", out var defaultLevel), "Falta 'Logging:LogLevel:Default'");
            Assert.IsFalse(string.IsNullOrWhiteSpace(defaultLevel.GetString()), "'Logging:LogLevel:Default' está vacío");
            Assert.IsTrue(logLevel.TryGetProperty("Microsoft.AspNetCore", out var msLevel), "Falta 'Logging:LogLevel:Microsoft.AspNetCore'");
            Assert.IsFalse(string.IsNullOrWhiteSpace(msLevel.GetString()), "'Logging:LogLevel:Microsoft.AspNetCore' está vacío");
        }

        [Test]
        public void AllowedHosts_ShouldBeValid()
        {
            Assert.IsTrue(_root.TryGetProperty("AllowedHosts", out var allowedHosts), "Falta 'AllowedHosts'");
            Assert.IsFalse(string.IsNullOrWhiteSpace(allowedHosts.GetString()), "'AllowedHosts' está vacío");
        }

        [Test]
        public void AppConfiguration_ShouldBeValid()
        {
            Assert.IsTrue(_root.TryGetProperty("AppConfiguration", out var appConfig), "Falta 'AppConfiguration'");
            var requiredKeys = new[] { "ServidorSmtp", "PuertoSmtp", "UsuarioSmtp", "ContraseñaSmtp", "apiServer", "apiPort", "LinkScanQR" };
            foreach (var key in requiredKeys)
            {
                Assert.IsTrue(appConfig.TryGetProperty(key, out var prop), $"Falta '{key}' en AppConfiguration");
                if (key != "ContraseñaSmtp") // Contraseña puede estar vacía
                    Assert.IsFalse(string.IsNullOrWhiteSpace(prop.GetString()), $"'{key}' en AppConfiguration está vacío");
            }
        }

        [Test]
        public void Jwt_ShouldBeValid()
        {
            Assert.IsTrue(_root.TryGetProperty("Jwt", out var jwt), "Falta 'Jwt'");
            var requiredKeys = new[] { "Key", "Issuer", "Audience", "AccessTokenMinutes" };
            foreach (var key in requiredKeys)
            {
                Assert.IsTrue(jwt.TryGetProperty(key, out var prop), $"Falta '{key}' en Jwt");
                Assert.IsFalse(string.IsNullOrWhiteSpace(prop.ToString()), $"'{key}' en Jwt está vacío");
            }
        }

        [Test]
        public void Paths_ShouldBeValid()
        {
            Assert.IsTrue(_root.TryGetProperty("Paths", out var paths), "Falta 'Paths'");
            Assert.IsTrue(paths.TryGetProperty("LogFilePath", out var logPath), "Falta 'LogFilePath' en Paths");
            Assert.IsFalse(string.IsNullOrWhiteSpace(logPath.GetString()), "'LogFilePath' en Paths está vacío");
        } 
    }
}