namespace Application.DTOs.DataQuery
{
    public class vCampanasUsuariosDTO
    {
        public int? IdCampana { get; }
        public string? NombreCampana { get; }
        public string? FrecuenciaCampana { get; }
        public DateTime? FechaEjecucion { get; }
        public string? EstadoEjecucion { get; }
        public string? TipoCampana { get; }
        public string? NombreSegmento { get; }
        public string? TipoSegmento { get; }
        public string? NombreUsuario { get; }
        public string? CorreoUsuario { get; } 
         
        public vCampanasUsuariosDTO(int? idCampana, 
                                    string? nombreCampana, 
                                    string? frecuenciaCampana,
                                    DateTime? fechaEjecucion,
                                    string? estadoEjecucion,
                                    string? tipoCampana,
                                    string? nombreSegmento,
                                    string? tipoSegmento,
                                    string nombreUsuario,
                                    string correoUsuario) {
            IdCampana = idCampana;
            NombreCampana = nombreCampana;
            FrecuenciaCampana = frecuenciaCampana;
            FechaEjecucion = fechaEjecucion;
            EstadoEjecucion = estadoEjecucion;
            TipoCampana = tipoCampana;
            NombreSegmento = nombreSegmento;
            TipoSegmento = tipoSegmento;
            NombreUsuario = nombreUsuario;
            CorreoUsuario = correoUsuario;
        }
    }
}