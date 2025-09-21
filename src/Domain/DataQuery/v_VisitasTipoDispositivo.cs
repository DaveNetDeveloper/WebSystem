using Domain;
using Domain.DataQuery;
using Domain.Entities;

namespace Domain.DataQuery
{
    public class v_VisitasTipoDispositivo
    {
        public DateTime semana { get; set; }
        public string tipoDispositivo { get; set; }
        public int visitas { get; set; } 
    }
}