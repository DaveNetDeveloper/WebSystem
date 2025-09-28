using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Segmento 
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string? descripcion { get; set; }
        public Guid idTipoSegmento { get; set; }
        public string campoRegla { get; set; }
        public string operadorRegla { get; set; }
        public string valorRegla { get; set; }

        [JsonIgnore]
        public ICollection<CampanaSegmentos> CampanaSegmentos { get; set; }
        [JsonIgnore]
        public ICollection<UsuarioSegmentos> UsuarioSegmentos { get; set; }
    }
}