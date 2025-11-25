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
            public const string Manager = "Manager";
            public const string WebUser = "WebUser";
            public const string Admin = "Admin";
        }
        public static class RolesIDs
        {
            public static Guid Manager = Guid.Parse("b983f534-f0ba-4f2d-bf82-013bc6fa7909");
            public static Guid WebUser = Guid.Parse("2c26e211-1764-4b93-9168-c1e73bce91c7");
            public static Guid Admin = Guid.Parse("f1d76bba-7266-4035-b66a-cdff138de76f");
        }
    }
}