using System.Text.Json;

namespace Domain.Entities
{
    public class CampanaAcciones
    {
        public int idCampana { get; set; }  // PK
        public Guid idAccion { get; set; } //PK
        public DateTime fecha { get; set; }
        public string accionDetalle { get; set; }

        public Campana Campana { get; set; }
        public Accion Accion { get; set; }  
    }
}