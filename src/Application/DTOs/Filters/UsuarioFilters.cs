using Domain.Entities;
using Application.Interfaces.DTOs.Filters;

namespace Application.DTOs.Filters
{
    public class UsuarioFilters : IFilters<Usuario>
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? Correo { get; set; }
        public bool? Activo { get; set; }
        public bool? Suscrito { get; set; } 
        public string? Token { get; set; }  

        //public ExpressionStarter<Usuario> BuildPredicate()
        //{
        //    var predicate = PredicateBuilder.New<Usuario>(true);

        //    if (Id.HasValue)
        //        predicate = predicate.And(u => u.Id == Id.Value);

        //    if (!string.IsNullOrEmpty(Nombre))
        //        predicate = predicate.And(u => u.Nombre.Contains(Nombre));

        //    if (!string.IsNullOrEmpty(Apellidos))
        //        predicate = predicate.And(u => u.Apellidos.Contains(Apellidos));

        //    if (!string.IsNullOrEmpty(Correo))
        //        predicate = predicate.And(u => u.Correo.Contains(Correo));

        //    if (Status.HasValue)
        //        predicate = predicate.And(u => u.Status == Status.Value);

        //    return predicate;
        //}
    }
}
