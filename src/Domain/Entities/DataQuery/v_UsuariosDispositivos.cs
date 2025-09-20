namespace Domain.Entities.DataQuery
{
    public class v_UsuariosDispositivos
    {
        public int idUsuario { get; set; }
        public string? nombre { get; set; }
        public string? apellidos { get; set; }
        public string? correo { get; set; }
        public string? browser { get; set; }
        public string? sistemaOperativo { get; set; }
        public string? plataforma { get; set; }
        public string? tipoDispositivo { get; set; }
        public string? modeloDispositivo { get; set; }
    }
}