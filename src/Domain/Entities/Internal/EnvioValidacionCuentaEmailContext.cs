using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public sealed class EnvioValidacionCuentaEmailContext : EmailContextBase
    {
        public string Token { get; }

        public EnvioValidacionCuentaEmailContext(string email, string nombre, string token) : base(email, nombre)
        {
            Token = token;
        }

        public IDictionary<string, string> GetTags()
        {
            var tags = GetBaseTags();
            tags["[_TOKEN_]"] = Token;
            return tags;
        }
    }
}