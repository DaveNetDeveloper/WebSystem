using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class ConsultaFilters : IFilters<Consulta>
    {
        public Guid? Id { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public Guid? IdMotivoConsulta { get; set; }        
        public int? IdEntidad { get; set; }
    }
}