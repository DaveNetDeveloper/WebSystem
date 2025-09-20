namespace Domain.Entities
{
    public class EmailToken
    {
        public Guid id { get; set; }
        public int userId { get; set; }
        public Guid token { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaExpiracion { get; set; }
        public string emailAction { get; set; }
        public bool consumido { get; set; }
        public DateTime? fechaConsumido { get; set; }
        public string? userAgent { get; set; }
        public string? ip { get; set; }

        public enum EmailTokenActions {
            SubscribeNewsletter, Login, AccountActivation, ResetPassword, ChangeEmail
        }
    }
}