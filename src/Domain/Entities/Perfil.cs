using System.Text.Json.Serialization;

namespace Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Perfil
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
        public bool activo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid idRol { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public static class Roles
        {
            public const string x = "xxx";
            public const string xx = "xxx";
            public const string xxx = "xxx";
        }
    }
}