namespace Domain.Entities
{
    public class UsuarioEntidad
    { 
        public int idusuario { get; set; } //PK
        public int identidad { get; set; }  // PK
        public DateTime? fecha { get; set; }
         

        public Usuario Usuario { get; set; } 
        public Entidad Entidad { get; set; }
    }
}
