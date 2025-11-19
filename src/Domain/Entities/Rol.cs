using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Rol
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string nombre { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? descripcion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? level { get; set; }

        [JsonIgnore]
        public ICollection<UsuarioRol> UsuarioRoles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string WebUser = "WebUser";
            public const string SAdmin = "SAdmin";
        }
    }
}