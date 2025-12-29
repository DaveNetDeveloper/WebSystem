using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public sealed class EnvioContrasenaCambiadaEmailContext : EmailContextBase
    {
        public string NombreEntidad { get; }

        public EnvioContrasenaCambiadaEmailContext(string email, string nombre, string nombreEntidad) : base(email, nombre)
        {
            NombreEntidad = nombreEntidad;
        }

        public IDictionary<string, string> GetTags()
        {
            var tags = GetBaseTags();
            tags["[_ENTITY_]"] = NombreEntidad;
            return tags;
        }
    }
}