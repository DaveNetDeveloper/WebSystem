namespace Domain.Entities
{
    public class UsuarioRecompensa
    {
        public int idusuario { get; set; } //PK
        public int idrecompensa { get; set; }  // PK
        public DateTime? fecha { get; set; }

        public Usuario Usuario { get; set; }
        public Recompensa Recompensa { get; set; }  
    }
}
