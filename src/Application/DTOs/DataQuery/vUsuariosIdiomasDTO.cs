using Application.Interfaces.DTOs.DataQuery;

namespace Application.DTOs.DataQuery
{
    public class vUsuariosIdiomasDTO : IView
    {
        public int? IdUsuario { get; }
        public string? Nombre { get; }
        public string? Apellidos { get; }
        public string? Correo { get; } 
        public string? Idioma { get; }  

        public vUsuariosIdiomasDTO(int? idUsuario, 
                                   string? apellidos, 
                                   string? correo, 
                                   string? nombre, 
                                   string? idioma) {
            IdUsuario = idUsuario;
            Apellidos = apellidos;
            Correo = correo;
            Nombre = nombre;
            Idioma = idioma;
        }
    }
}