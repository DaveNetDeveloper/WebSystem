namespace Domain.Entities
{
    public class RefreshToken
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; } // PK

        /// <summary>
        /// 
        /// </summary>
        public int idUsuario { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string refreshToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime expiresAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? isRevoked { get; set; } //FK

        /// <summary>
        /// 
        /// </summary>
        public DateTime? revokedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? reasonRevoked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static class ReasonRevoked
        {
            public const string ReasonRevoked1 = "ReasonRevoked1";
            public const string ReasonRevoked2 = "ReasonRevoked2";
            public const string ReasonRevoked3 = "ReasonRevoked3";
        }
     }
}