using Domain;
using Domain.DataQuery;
using Domain.Entities;

namespace Domain.DataQuery
{
    public class v_TotalUltimasTransacciones
    {
        public DateTime dia { get; set; }
        public int totalBienvenida { get; set; }
        public int totalCompletarPerfil { get; set; }
        public int totalEvento { get; set; }
        public int totalQR { get; set; }
        public int totalTransacciones { get; set; }
    }
}