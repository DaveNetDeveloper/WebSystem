using Domain;
using Domain.DataQuery;
using Domain.Entities;

namespace Domain.DataQuery
{
    public class v_ActividadUsuarios
    {
        public int idUsuario { get; set; }
        public string? nombre { get; set; }
        public string? apellidos { get; set; }
        public string? correo { get; set; }
        public string? pais { get; set; }
        public string? region { get; set; }
        public int? totalLogins { get; set; }
        public DateTime? ultimaConexion { get; set; } 
    }
}