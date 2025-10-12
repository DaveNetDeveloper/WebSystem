namespace Domain.Entities
{
    public class SmsConfiguration
    { 
        public string AccountSid { get; set; } 
        public string AuthToken { get; set; }
        public string FromNumber { get; set; } 
    }
}