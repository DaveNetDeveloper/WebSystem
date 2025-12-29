using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public sealed class EnvioRecompensaEmailContext : EmailContextBase
    {
        public string NombreEntidad { get; }
        public string AccionRecompensa { get; }
        public string NombreRecompensa{ get; }
        public string PuntosRecompensa { get; }
        public string FechaRecompensa { get; }

        public EnvioRecompensaEmailContext(string email, 
                                           string nombre, 
                                           string nombreEntidad,
                                           string accionRecompensa,
                                           string nombreRecompensa,
                                           string puntosRecompensa,
                                           string fechaRecompensa) : base(email, nombre)
        {
            NombreEntidad = nombreEntidad;
            AccionRecompensa = accionRecompensa;
            NombreRecompensa = nombreRecompensa;
            PuntosRecompensa = puntosRecompensa;
            FechaRecompensa = fechaRecompensa;
        }

        public IDictionary<string, string> GetTags()
        {
            var tags = GetBaseTags();
            tags["[_ENTITY_]"] = NombreEntidad;
            tags["[_ACCION-RECOMPENSA_]"] = AccionRecompensa;
            tags["[_NOMBRE-RECOMPENSA_]"] = NombreRecompensa;
            tags["[_PUNTOS-RECOMPENSA_]"] = PuntosRecompensa;
            tags["[_FECHA-RECOMPENSA_]"] = FechaRecompensa;
            return tags;
        }
    }
}