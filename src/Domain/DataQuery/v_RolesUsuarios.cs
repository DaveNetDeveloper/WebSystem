using Domain;
using Domain.DataQuery;
using Domain.Entities;

namespace Domain.DataQuery
{
    public class v_RolesUsuarios
    {
        public string? rol { get; set; }
        public decimal? rolUsage { get; set; }
        public int? usuariosCount { get; set; } 
    }
}