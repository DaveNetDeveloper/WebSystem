using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Login
    {
        public Guid id { get; set; }
        public int idUsuario { get; set; }
        public DateTime fecha { get; set; }
        public string? plataforma { get; set; }
        public string? tipoDispositivo { get; set; }
        public string? modeloDispositivo { get; set; }
        public string? sistemaOperativo { get; set; }
        public string? browser { get; set; }
        public string? ip { get; set; }
        public string? pais { get; set; }
        public string? region { get; set; }
        public string? idiomaNavegador { get; set; }
    }
}