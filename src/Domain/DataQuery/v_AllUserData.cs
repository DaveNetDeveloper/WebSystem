
using Domain;
using Domain.DataQuery;
using Domain.Entities;

namespace Domain.DataQuery
{
    public class v_AllUserData
    {
        public int? idUsuario { get; set; }
        public string? nombreUsuario { get; set; }
        public string? correo { get; set; }
        public bool? activo { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaNacimiento { get; set; }
        public bool? suscrito { get; set; }
        public int? puntos { get; set; }
        public string? genero { get; set; }
        public int? edad { get; set; }
        public int? totalRoles { get; set; }
        public string? roles { get; set; }
        public int? totalLogins { get; set; }
        public DateTime? ultimaConexion { get; set; } 
        public string? ultimoDispositivo { get; set; }
        public string? ciudad { get; set; }
        public string? provincia { get; set; }
        public string? comunidadAutonoma { get; set; }
        public string? pais { get; set; }
        public int? totalRecompensas { get; set; }
        public int? totalEntidades { get; set; }
        public string? entidades { get; set; }
        public int? totalSegmentos { get; set; }
        public string? segmentos { get; set; }
        public int? totalTransacciones { get; set; }
        public int? totalTransaccionesProducto { get; set; }
        public int? totalTransaccionesEvento { get; set; }
    }
}