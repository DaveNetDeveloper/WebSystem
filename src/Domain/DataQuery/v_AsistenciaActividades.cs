using Domain;
using Domain.DataQuery;
using Domain.Entities;

namespace Domain.DataQuery
{
    public class v_AsistenciaActividades
    {
        public Guid idReserva { get; set; }
        public string codigoReserva { get; set; }
        public int idActividad { get; set; }
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string nombreActividad { get; set; }
        public string tipoActividad { get; set; }
        public DateTime fechaReserva { get; set; }
        public DateTime? fechaActividad { get; set; }
        public string estado { get; set; }
        public DateTime? fechaValidacion { get; set; }
    }
}