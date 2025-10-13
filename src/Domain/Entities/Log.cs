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
        public DateTime fecha { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static class TipoLog
        {
            public const string Info = "Info";
            public const string Error = "Error";
            public const string Warning = "Warning";
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Proceso
        {
            // User
            public const string Login = "Login";
            public const string ValidarCuenta = "ValidarCuenta";
            public const string Registro = "Registro";
            public const string ActivarSuscripcion = "ActivarSuscripcion";
            public const string NuevaContraseña = "NuevaContraseña";
            public const string CambioDatosPersonales = "CambioDatosPersonales";
            public const string Logout = "Logout";
            public const string ReservarActividad = "ReservarActividad";
            public const string ValidarAsistenciaActividad = "ValidarAsistenciaActividad";
            public const string ReservarProducto = "ReservarProducto";
            public const string ValidarQRProducto = "ValidarQRProducto";
            public const string SeguirEntidad = "SeguirEntidad";
            public const string ConsultaEntidad = "ConsultaEntidad";
            public const string GenerarRecompensa = "GenerarRecompensa";
            public const string EnvioEmail = "EnvioEmail";
            public const string EnvioSMS = "EnvioSMS";
            public const string EnvioInApp = "EnvioInApp";

            // Admin
            public const string Login_Admin = "Login_Admin";
            public const string Logout_Admin = "Logout_Admin";
            public const string GenerarReport_Admin = "GenerarReport_Admin";
            public const string EditarEntidad_Admin = "EditarEntidad_Admin";
            public const string EnvioEmail_Admin = "EnvioEmail_Admin";
            public const string EnvioSms_Admin = "EnvioSms_Admin";

            // SuperADmin
            public const string Login_SAdmin = "Login_SAdmin";
            public const string Logout_SAdmin = "Logout_SAdmin";
            public const string GenerarReport_SAdmin = "GenerarReport_SAdmin";
            public const string EditarEntidad_SAdmin = "EditarEntidad_SAdmin";
            public const string EnvioEmail_SAdmin = "EnvioEmail_SAdmin";
            public const string EnvioSms_SAdmin = "EnvioSms_SAdmin";

            // WorkerService
            public const string EjecutarCampanas_Ws = "EjecutarCampanas_Ws";
            public const string EnvioNewsletter_Ws = "EnvioNewsletter_Ws";
            public const string ActualizarSegmentos_Ws = "ActualizarSegmentos_Ws";

            // Internal - API
            public const string API_Middleware = "API_Middleware";
            public const string API_FakeJwtBearerHandler = "API_FakeJwtBearerHandler";
            public const string API_Program = "API_Program";
            public const string API_Authorization = "API_Authorization";

            // Internal - WorkerService

            // Internal - API

            // Internal - Test - Unit Test

            // Internal - Test - Integration Test

            // Internal - Test - UI Test
        }
    }
}