namespace Domain.Entities
{
    public class Log
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid id { get; set; } // PK

        /// <summary>
        /// 
        /// </summary>
        public string tipoLog { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string proceso { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string titulo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? detalle { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int? idUsuario { get; set; } //FK

        /// <summary>
        /// 
        /// </summary>
        public static class TipoLog
        {
            public const string Info = "Info";
            public const string Error = "Error";
            public const string Warning = "Warning";
        }




    }
}