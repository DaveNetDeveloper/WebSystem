using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Rol
    {
        public Guid id { get; set; }
        public string nombre { get; set; }
        public string? descripcion { get; set; }
        public int? level { get; set; }

        [JsonIgnore]
        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
    }
}