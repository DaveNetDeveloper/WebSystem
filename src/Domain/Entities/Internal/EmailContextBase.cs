using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public abstract class EmailContextBase
    {
        public string Email { get; }
        public string Nombre { get; }

        protected EmailContextBase(string email, 
                                   string nombre)
        {
            Email = email;
            Nombre = nombre;
        }

        public virtual IDictionary<string, string> GetBaseTags()
            => new Dictionary<string, string>
            {
                ["[_EMAIL_]"] = Email,
                ["[_NAME_]"] = Nombre
            };
    }
}