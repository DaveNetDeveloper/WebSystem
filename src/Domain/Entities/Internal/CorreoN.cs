using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    /// <summary> Entidad interna con datos del correo electrónico </summary>
    public class CorreoN
    {
        public string Destinatario { get; set; } = default!;
        public string Asunto { get; set; } = default!;
        public string Cuerpo { get; set; } = default!;
        public FicheroAdjunto? FicheroAdjunto { get; set; }

        public void ApplyTags(IDictionary<string, string> tags)
        {
            foreach (var tag in tags)
            {
                Asunto = Asunto.Replace(tag.Key, tag.Value);
                Cuerpo = Cuerpo.Replace(tag.Key, tag.Value);
            }
        }
    }
}