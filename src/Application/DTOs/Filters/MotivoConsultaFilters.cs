using Domain.Entities;
using Application.Interfaces.DTOs.Filters;
namespace Application.DTOs.Filters
{
    public class MotivoConsultaFilters : IFilters<MotivoConsulta>
    {
        public Guid? Id { get; set; }
        public string? Nombre { get; set; }
        public Guid? IdTipoEntidad { get; set; }
    }
}