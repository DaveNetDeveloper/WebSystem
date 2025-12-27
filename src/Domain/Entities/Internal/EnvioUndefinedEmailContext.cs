using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public sealed class EnvioUndefinedEmailContext : EmailContextBase
    { 
        public EnvioUndefinedEmailContext(string email, 
                                          string nombre) : base(email, nombre) {
    
        }

        public IDictionary<string, string> GetTags()
        {
            var tags = GetBaseTags(); 
            return tags;
        }
    }
}