using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class SmsNotificationFilters : IFilters<SmsNotification>
    {
        public Guid? Id { get; set; }
        public int? IdUsuario { get; set; }
        public string? TipoEnvioSms { get; set; }
        public bool? Activo { get; set; }
        public string? Telefono { get; set; } 
    }
}