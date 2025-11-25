namespace Domain.Entities
{
    public class GlobalConfiguration
    {
        public string BaseUrl { get; set; }
        public string Port { get; set; }
        public string AccessTokenCookieName { get; set; }
        public string RefreshTokenCookieName { get; set; }
        public string RoleCookieName { get; set; }
        public string ProfileCookieName { get; set; }
        public ControllerNames ControllerName { get; set; }

        public class ControllerNames { 
            public string AuthController { get; set; }
            public string UsuariosController { get; set; }
            public string PerfilesController { get; set; }
            public string RolesController { get; set; }
            public string NotificationsController { get; set; }
            public string EmailTokensController { get; set; }
            public string InAppNotificationController { get; set; }
            public string LoginsController { get; set; }
            public string ProductosController { get; set; }
        }
    }
}