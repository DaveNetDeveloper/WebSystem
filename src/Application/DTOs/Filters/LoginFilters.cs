using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class LoginFilters : IFilters<Login>
    {
        public Guid? Id { get; set; }
        public int? IdUsuario { get; set; }  
    }
}