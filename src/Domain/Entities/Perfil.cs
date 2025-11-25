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
        public static class Perfiles
        {
            public const string Basic = "Basic";
            public const string Lover = "Lover";
            public const string Friend = "Friend";
            public const string Editor = "Editor";
            public const string Viewer = "Viewer";
            public const string SAdmin = "SAdmin";
        }
    }
}