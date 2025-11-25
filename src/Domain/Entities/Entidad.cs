using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Entidad
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string? ubicacion { get; set; }
        public DateTime? fechaAlta { get; set; }
        public int? popularidad { get; set; }
        public string? descripcion { get; set; }
        public bool activo { get; set; }
        public Guid idTipoEntidad { get; set; }
        public string? imagen { get; set; }
        public string manager { get; set; }

        [JsonIgnore]
        public ICollection<UsuarioRol> UsuarioRoles { get; set; }
        [JsonIgnore]
        public ICollection<UsuarioEntidad> UsuarioEntidades { get; set; }
        [JsonIgnore]
        public ICollection<EntidadCategoria> EntidadadCategorias { get; set; }
    }
}