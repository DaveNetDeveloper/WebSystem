namespace Application.DTOs.DataQuery
{
    public class vAsistenciaActividadesDTO
    {
        public Guid? IdReserva { get; set; }
        public string? CodigoReserva { get; set; }
        public int? IdActividad { get; set; }
        public int? IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? NombreActividad { get; set; }
        public string? TipoActividad { get; set; }
        public DateTime? FechaReserva { get; set; }
        public DateTime? FechaActividad { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaValidacion { get; set; } 

        public vAsistenciaActividadesDTO(Guid? idReserva, 
                                         string? codigoReserva,
                                         int? idActividad,
                                         int? idUsuario,
                                         string? nombreUsuario,
                                         string? nombreActividad,
                                         string? tipoActividad,
                                         DateTime? fechaReserva,
                                         DateTime? fechaActividad,
                                         string? estado,
                                         DateTime? fechaValidacion) {

            IdReserva = idReserva;
            CodigoReserva = codigoReserva;
            IdActividad = idActividad;
            IdUsuario = idUsuario;
            NombreUsuario = nombreUsuario;
            NombreActividad = nombreActividad;
            FechaReserva = fechaReserva;
            FechaActividad = fechaActividad;
            Estado = estado;
            FechaValidacion = fechaValidacion;
        }
    }
}