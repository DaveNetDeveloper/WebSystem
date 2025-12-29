using Domain;
using Domain.DataQuery;
using Domain.Entities;

namespace Domain.DataQuery
{
    public class v_TotalErrores
    { 
        public DateTime? dia { get; set; } 
        public int? totalErrores { get; set; }
        public string? proceso { get; set; }
    }
}