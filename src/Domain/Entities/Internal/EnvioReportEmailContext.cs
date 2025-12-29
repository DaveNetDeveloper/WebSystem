using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public sealed class EnvioReportEmailContext : EmailContextBase
    {
        public string NombreEntidad { get; }
        public string NombreInforme { get; }

        public EnvioReportEmailContext(string email, 
                                       string nombre, 
                                       string nombreEntidad,
                                       string nombreInforme) : base(email, nombre)
        {
            NombreEntidad = nombreEntidad;
            NombreInforme = nombreInforme;
        }

        public IDictionary<string, string> GetTags()
        {
            var tags = GetBaseTags();
            tags["[_ENTITY_]"] = NombreEntidad;
            tags["[_NOMBRE-INFORME_]"] = NombreInforme;
            return tags;
        }
    }
}