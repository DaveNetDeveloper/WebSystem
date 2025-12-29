using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class LogFilters : IFilters<Log>
    {
        public Guid? Id { get; set; }
        public string? TipoLog { get; set; }
        public string? Proceso { get; set; }    
        public int? IdUsuario { get; set; }
        public DateTime? Fecha_Ini { get; set; }
        public DateTime? Fecha_Fin { get; set; }
    }
}