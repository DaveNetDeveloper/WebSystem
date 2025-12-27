using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public sealed class EnvioRecordatorioSuscripcionEmailContext : EmailContextBase
    {
        public string Token { get; }

        public EnvioRecordatorioSuscripcionEmailContext(string email, 
                                                        string nombre,
                                                        string token) : base(email, nombre)
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