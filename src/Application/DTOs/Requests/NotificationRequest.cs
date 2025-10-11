namespace Application.DTOs.Requests
{
    /// <summary>
    /// 
    /// </summary>
    public class NotificationRequest
    {
        /// <summary>
        /// NotificationTypes: ["email", "sms", "push"]
        /// </summary>
        public string Type { get; set; } = default!;

        /// <summary>
        /// Id de usuario destinatario
        /// </summary>
        public string Recipient { get; set; } = default!;

        /// <summary>
        /// Asunto del mensaje
        /// </summary>
        public string Subject { get; set; } = default!;

        /// <summary>
        /// Cuerpo del mensaje
        /// </summary>
        public string Message { get; set; } = default!;
    }

    public static class NotificationTypes
    {
        public const string Email = "email";
        public const string SMS = "sms";
        public const string Push = "push"; 
    }
}