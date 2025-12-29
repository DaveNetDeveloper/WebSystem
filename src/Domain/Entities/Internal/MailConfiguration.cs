
namespace Domain.Entities
{
    public class MailConfiguration
    { 
        public string ServidorSmtp { get; set; }
        public string PuertoSmtp { get; set; }
        public string UsuarioSmtp { get; set; }
        public string ContrasenaSmtp { get; set; }
        public string LogoURL { get; set; }
    }
}