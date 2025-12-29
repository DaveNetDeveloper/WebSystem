using Application.Interfaces.DTOs.DataQuery;

namespace Application.DTOs.DataQuery
{
    public class vAllCampanasDataDTO : IView
    {
        public int? IdCampana { get; }
        public string? NombreCampana { get; }
        public bool? EstadoCampana { get; }
        public string? FrecuenciaCampana { get; }
        public DateTime? FechaInicioCampana { get; }
        public DateTime? FechaFinCampana { get; }
        public string? TipoCampana { get; }
        public string? TipoAccion { get; }
        public string? Accion { get; }
        public string? AccionDetalle { get; }
        public int? IdSegmento { get; }
        public string? Segmento { get; }
        public string? TipoSegmento { get; }

        public vAllCampanasDataDTO(int? idCampana,
                                   string? nombreCampana,
                                   bool? estadoCampana,
                                   string? frecuenciaCampana,
                                   DateTime? fechaInicioCampana,
                                   DateTime? fechaFinCampana,
                                   string? tipoCampana,
                                   string? tipoAccion,
                                   string? accion,
                                   string? accionDetalle,
                                   int? idSegmento,
                                   string? segmento,
                                   string? tipoSegmento) {
            IdCampana = idCampana;
            NombreCampana = nombreCampana;
            EstadoCampana = estadoCampana;
            FrecuenciaCampana = frecuenciaCampana;
            FechaInicioCampana = fechaInicioCampana;
            FechaFinCampana = fechaFinCampana;
            TipoCampana = TipoCampana;
            TipoAccion = tipoAccion;
            Accion = accion;
            AccionDetalle = accionDetalle;
            IdSegmento = idSegmento;
            Segmento = segmento;
            TipoSegmento = tipoSegmento;
        }
    }
}