namespace Domain.Entities
{
    public class MQServiceConfiguration
    { 
        public string HostName { get; set; } 
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }
}