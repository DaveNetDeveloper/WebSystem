namespace Domain.Entities
{
    public class EntidadCategoria
    { 
        public Guid idcategoria { get; set; } // PK
        public int identidad { get; set; }  // PK


        public Entidad Entidad { get; set; }
        public Categoria Categoria { get; set; }
    }
}