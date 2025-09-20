namespace Domain.Entities.DataQuery
{
    public class v_UsuariosInactivos
    {
        public int idUsuario { get; set; }
        public string? nombre { get; set; }
        public string? apellidos { get; set; }
        public string? correo { get; set; }
        public string? rol { get; set; }
        public DateTime? ultimaConexion { get; set; }
        public int diasInactivo { get; set; }  
    }
}