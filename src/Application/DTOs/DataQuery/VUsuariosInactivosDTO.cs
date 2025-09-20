namespace Application.DTOs.DataQuery
{
    public class vUsuariosInactivosDTO
    {
        public int IdUsuario { get; }
        public string? Nombre { get; }
        public string? Apellidos { get; }
        public string? Correo { get; }
        public string? Rol { get; }
        public DateTime? UltimaConexion { get; }
        public int DiasInactivo { get; }

        public vUsuariosInactivosDTO(int idUsuario, 
                                     string apellidos, 
                                     string correo, 
                                     string nombre,
                                     string rol,
                                     DateTime? ultimaConexion, 
                                     int diasInactivo) {
            IdUsuario = idUsuario;
            Apellidos = apellidos;
            Correo = correo;
            Nombre = nombre;
            Rol = rol;
            UltimaConexion = ultimaConexion;
            DiasInactivo = diasInactivo;
        }
    }
}