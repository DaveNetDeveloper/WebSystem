using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class ActividadReservaFilters : IFilters<ActividadReserva>
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? IdReserva { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? CodigoReserva { get; set; } 
        
        /// <summary>
        /// 
        /// </summary>
        public int? IdActividad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? IdUsuario { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string? Estado { get; set; }
        
    }
}