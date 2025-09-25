using Domain;
using Domain.DataQuery;
using Domain.Entities;

namespace Domain.DataQuery
{
    public class v_CampanasUsuarios
    {
        public int idCampana { get; set; }
        public string nombreCampana { get; set; }
        public string frecuenciaCampana { get; set; }
        public DateTime fechaEjecucion { get; set; }
        public string estadoEjecucion { get; set; }
        public string tipoCampana { get; set; }
        public string nombreSegmento { get; set; }
        public string tipoSegmento { get; set; }
        public string nombreUsuario { get; set; }
        public string correoUsuario { get; set; } 
    }
}