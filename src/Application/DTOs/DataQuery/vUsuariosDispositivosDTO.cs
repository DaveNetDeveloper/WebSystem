namespace Application.DTOs.DataQuery
{
    public class vUsuariosDispositivosDTO
    {
        public int? IdUsuario { get; }
        public string? Nombre { get; }
        public string? Apellidos { get; }
        public string? Correo { get; }
        public string? Browser { get; }
        public string? SistemaOperativo { get; }
        public string? Plataforma { get; }
        public string? TipoDispositivo { get; }
        public string? ModeloDispositivo { get; }

        public vUsuariosDispositivosDTO(int? idUsuario, 
                                     string? apellidos, 
                                     string? correo, 
                                     string? nombre,
                                     string? browser,
                                     string? plataforma,
                                     string? sistemaOperativo, 
                                     string? tipoDispositivo,
                                     string? modeloDispositivo) {
            IdUsuario = idUsuario;
            Apellidos = apellidos;
            Correo = correo;
            Nombre = nombre;
            Browser = browser;
            SistemaOperativo = sistemaOperativo;
            Plataforma = plataforma;
            TipoDispositivo = tipoDispositivo;
            ModeloDispositivo = modeloDispositivo;
        }
    }
}