using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public sealed class EnvioReservaActividadEmailContext : EmailContextBase
    {
        public string NombreEntidad { get; }
        public string NombreActividad { get; }
        public string FechaActividad { get; }
        public EnvioReservaActividadEmailContext(string email, 
                                                 string nombre, 
                                                 string nombreEntidad,
                                                 string nombreActividad,
                                                 string fechaActividad) : base(email, nombre) {
            NombreEntidad = nombreEntidad;
            NombreActividad = nombreActividad;
            FechaActividad = fechaActividad;
        }

        public IDictionary<string, string> GetTags()
        {
            var tags = GetBaseTags();
            tags["[_ENTITY_]"] = NombreEntidad;
            tags["[_NOMBRE-ACTIVIDAD_]"] = NombreActividad;
            tags["[_FECHA-ACTIVIDAD_]"] = FechaActividad;
            return tags;
        }
    }
}