using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CampanaExecution
    {
        public Guid id { get; set; }
        public int idCampana { get; set; }
        public string estado { get; set; }
        public DateTime fechaEjecucion { get; set; }

        [Column(TypeName = "jsonb")]
        public string idsUsuarios { get; set; }

        //[JsonIgnore]
        //public Campana Campana { get; set; }

        public static class EstadoEjecucion
        {
            public const string Passed = "Passed";
            public const string Error = "Error";
        }

    }
}