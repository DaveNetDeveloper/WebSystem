using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class InAppNotificationFilters : IFilters<InAppNotification>
    {
        public Guid? Id { get; set; }
        public int? IdUsuario { get; set; }
        public string? TipoEnvioInApp { get; set; }
        public bool? Activo { get; set; }  
    }
}