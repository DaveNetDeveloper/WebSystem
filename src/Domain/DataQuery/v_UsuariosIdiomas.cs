using Domain;
using Domain.DataQuery;
using Domain.Entities;

namespace Domain.DataQuery
{
    public class v_UsuariosIdiomas
    {
        public int idUsuario { get; set; }
        public string? nombre { get; set; }
        public string? apellidos { get; set; }
        public string? correo { get; set; } 
        public string? idioma { get; set; } 
    }
}