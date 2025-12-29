using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Recompensa
    {
        public int id { get; set; } // PK
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public int? identidad { get; set; }
        public Guid? idtiporecompensa { get; set; }

        [JsonIgnore]
        public ICollection<UsuarioRecompensa> UsuarioRecompensas { get; set; }
    }
}