namespace Domain.Entities
{
    public class UsuarioRol
    {
        public Guid idrol { get; set; }
        public int idusuario { get; set; } 
        public int identidad { get; set; }
         
        public Usuario Usuario { get; set; } 
        public Rol Rol { get; set; }
        public Entidad Entidad { get; set; }
    }
}