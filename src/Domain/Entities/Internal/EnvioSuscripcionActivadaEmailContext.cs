using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public sealed class EnvioSuscripcionActivadaEmailContext : EmailContextBase
    {
        //public string Token { get; }

        //public EnvioSuscripcionActivadaEmailContext(string email, string nombre, string token) : base(email, nombre)
        public EnvioSuscripcionActivadaEmailContext(string email,
                                                    string nombre) : base(email, nombre)
        {
            //Token = token;
        }

        public IDictionary<string, string> GetTags()
        {
            var tags = GetBaseTags();
            //tags["[_TOKEN_]"] = Token;
            return tags;
        }
    }
}