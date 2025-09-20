 namespace Application.DTOs.Requests
{
    public class UsuarioRequest
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool Suscrito { get; set; }
    }
}
