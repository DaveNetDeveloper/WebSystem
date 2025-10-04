using Application.Interfaces.DTOs.DataQuery;

namespace Application.DTOs.DataQuery
{
    public class vAllUserDataDTO : IView
    {
        public int? IdUsuario { get; }
        public string? NombreUsuario { get; }
        public string? Correo { get; }
        public bool? Activo { get; }
        public DateTime? FechaCreacion { get; }
        public DateTime? FechaNacimiento { get; }
        public bool? Suscrito { get; }
        public int? Puntos { get; }
        public string? Genero { get; }
        public string? Telefono { get; }
        public int? Edad { get; }
        public string? CodigoRecomendacion { get; }
        public string? CodigoRecomendacionRef { get; }
        public int? TotalRoles { get; }
        public string? Roles { get; }
        public int? TotalLogins { get; }
        public DateTime? UltimaConexion { get; }
        public string? UltimoDispositivo { get; }
        public string? Ciudad { get; }
        public string? Provincia { get; }
        public string? ComunidadAutonoma { get; }
        public string? Pais { get; }
        public int? TotalRecompensas { get; }
        public int? TotalEntidades { get; }
        public string? Entidades { get; }
        public int? TotalSegmentos { get; }
        public string? Segmentos { get; }
        public int? TotalTransacciones { get; }
        public int? TotalTransaccionesProducto { get; }
        public int? TotalTransaccionesEvento { get; }

        public vAllUserDataDTO(  int? idUsuario,
                                 string? nombreUsuario,
                                 string? correo,
                                 bool? activo,
                                 DateTime? fechaCreacion,
                                 DateTime? fechaNacimiento,
                                 bool? suscrito,
                                 int? puntos,
                                 string? genero,
                                 string? telefono,
                                 int? edad,
                                 string? codigoRecomendacion,
                                 string? codigoRecomendacionRef,
                                 int? totalRoles,
                                 string? roles,
                                 int? totalLogins,
                                 DateTime? ultimaConexion,
                                 string? ultimoDispositivo,
                                 string? ciudad,
                                 string? provincia,
                                 string? comunidadAutonoma,
                                 string? pais,
                                 int? totalRecompensas,
                                 int? totalEntidades,
                                 string? entidades,
                                 int? totalSegmentos,
                                 string? segmentos,
                                 int? totalTransacciones,
                                 int? totalTransaccionesProducto,
                                 int? totalTransaccionesEvento) { 

            IdUsuario = idUsuario;
            NombreUsuario = nombreUsuario;
            Correo = correo;
            Activo = activo;
            FechaCreacion = fechaCreacion;
            FechaNacimiento = fechaNacimiento;
            Suscrito = suscrito;
            Puntos = puntos;
            Genero = genero;
            Telefono = telefono;
            Edad = edad;
            CodigoRecomendacion = codigoRecomendacion;
            CodigoRecomendacionRef = codigoRecomendacionRef;
            TotalRoles = totalRoles;
            Roles = roles;
            TotalLogins = totalLogins;
            UltimaConexion = ultimaConexion;
            UltimoDispositivo = ultimoDispositivo;
            Ciudad = ciudad;
            Provincia = provincia;
            ComunidadAutonoma = comunidadAutonoma;
            Pais = pais;
            TotalRecompensas = totalRecompensas;
            TotalEntidades = totalEntidades;
            Entidades = entidades;
            TotalSegmentos = totalSegmentos;
            Segmentos = segmentos;
            TotalTransacciones = totalTransacciones;
            TotalTransaccionesProducto = totalTransaccionesProducto;
            TotalTransaccionesEvento = totalTransaccionesEvento;
        }
    }
}