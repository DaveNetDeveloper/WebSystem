namespace Application.DTOs.DataQuery
{
    public class vActividadUsuariosDTO
    {
        public int? IdUsuario { get; }
        public string? Nombre { get; }
        public string? Apellidos { get; }
        public string? Correo { get; }
        public string? Pais { get; }
        public string? Region { get; }
        public int? TotalLogins { get; }
        public DateTime? UltimaConexion { get; } 

        public vActividadUsuariosDTO(int? idUsuario, 
                                     string? apellidos, 
                                     string? correo, 
                                     string? nombre,
                                     string? region,
                                     string? pais,
                                     DateTime? ultimaConexion, 
                                     int? totalLogins) {
            IdUsuario = idUsuario;
            Apellidos = apellidos;
            Correo = correo;
            Nombre = nombre;
            Pais = pais;
            UltimaConexion = ultimaConexion;
            Region = region;
            TotalLogins = totalLogins;
        }
    }
}