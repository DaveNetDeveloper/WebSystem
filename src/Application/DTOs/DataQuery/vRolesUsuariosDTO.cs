namespace Application.DTOs.DataQuery
{
    public class vRolesUsuariosDTO
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