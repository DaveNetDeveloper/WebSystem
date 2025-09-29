using Domain;
using Domain.DataQuery;
using Domain.Entities;

namespace Domain.DataQuery
{
    public class v_AllCampanasData
    {
        public int? idCampana { get; set; }
        public string? nombreCampana { get; set; }
        public bool? estadoCampana { get; set; }
        public string? frecuenciaCampana { get; set; }
        public DateTime? fechaInicioCampana { get; set; }
        public DateTime? fechaFinCampana { get; set; }
        public string? tipoCampana { get; set; }
        public string? tipoAccion { get; set; }
        public string? accion { get; set; }
        public string? accionDetalle { get; set; }
        public int? idSegmento { get; set; }
        public string? segmento { get; set; }
        public string? tipoSegmento { get; set; }
    }
}