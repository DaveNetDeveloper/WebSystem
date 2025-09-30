namespace Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class InAppNotification
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int idUsuario { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string inAppNotificationType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string titulo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string mensaje { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool activo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime fechaCreacion { get; set; }

        /// <summary>
        /// Enumeración para los tipos de notificación InApp
        /// </summary>
        public static class InAppNotificationType
        {
            public const string Type1 = "Type1";
            public const string Type2 = "Type2";
            public const string Type3 = "Type3";
            public const string Type4 = "Type4";
        }
    }
}