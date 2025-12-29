using System.Text;
using System.Text.Json.Serialization;
using static Domain.Entities.TipoEnvioCorreo;

namespace Domain.Entities
{
    public sealed class EnvioComunicacionEmailContext : EmailContextBase
    {
        public string Token { get; }
        public string TituloComunicacion { get; }
        public string ContenidoComunicacion { get; }

        public EnvioComunicacionEmailContext(string email, 
                                             string nombre, 
                                             string token,
                                             string tituloComunicacion,
                                             string contenidoComunicacion) : base(email, nombre)
        {
            Token = token;
            TituloComunicacion = tituloComunicacion;
            ContenidoComunicacion = contenidoComunicacion;
        }

        public IDictionary<string, string> GetTags()
        {
            var tags = GetBaseTags();
            tags["[_TOKEN_]"] = Token;
            tags["[_TITULO-COMUNICACION_]"] = TituloComunicacion;
            tags["[_CONTENIDO-COMUNICACION_]"] = ContenidoComunicacion;
            return tags;
        }
    }
}