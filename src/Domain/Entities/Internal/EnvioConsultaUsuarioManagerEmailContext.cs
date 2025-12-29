using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public sealed class EnvioConsultaUsuarioManagerEmailContext : EmailContextBase
    {
        public string NombreEntidad { get; }
        public string NombreUsuario { get; }
        public string CorreoUsuario { get; }

        public EnvioConsultaUsuarioManagerEmailContext(string email, 
                                                       string nombre, 
                                                       string nombreEntidad,
                                                       string nombreUsuario,
                                                       string correoUsuario) : base(email, nombre) {
            NombreEntidad = nombreEntidad;
            NombreUsuario = nombreUsuario;
            CorreoUsuario = correoUsuario;
        }

        public IDictionary<string, string> GetTags()
        {
            var tags = GetBaseTags();
            tags["[_ENTITY_]"] = NombreEntidad;
            tags["[_USER-NAME_]"] = NombreUsuario;
            tags["[_USER-EMAIL_]"] = CorreoUsuario;
            return tags;
        }
    }
}