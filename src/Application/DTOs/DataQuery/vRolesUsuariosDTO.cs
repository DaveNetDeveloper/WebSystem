using Application.Interfaces.DTOs.DataQuery;

namespace Application.DTOs.DataQuery
{
    public class vRolesUsuariosDTO : IView
    {
        public string? Rol { get; }
        public decimal? RolUsage { get; }
        public int? UsuariosCount { get; } 

        public vRolesUsuariosDTO(string? rol, 
                                 decimal? rolUsage, 
                                 int? usuariosCount) {
            Rol = rol;
            RolUsage = rolUsage;
            UsuariosCount = usuariosCount; 
        }
    }
}