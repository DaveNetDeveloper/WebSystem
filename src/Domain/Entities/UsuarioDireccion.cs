namespace Domain.Entities
{
    public class UsuarioDireccion
    {
        public int idusuario { get; set; } //PK
        public int iddireccion { get; set; }  // PK
        public DateTime? fecha { get; set; }

        public Usuario Usuario { get; set; }
        public Direccion Direccion { get; set; } 
    }
}
