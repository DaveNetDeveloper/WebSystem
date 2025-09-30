namespace Application.DTOs.Responses
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool Suscrito { get; set; }
        public DateTime? UltimaConexion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? Puntos { get; set; }
        public string? CodigoRecomendacion { get; set; }
        public string? CodigoRecomendacionRef { get; set; }
        public string? Telefono { get; set; }
    }
}
