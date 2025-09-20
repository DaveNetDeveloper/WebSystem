namespace Domain.Entities
{
    public class ProductoImagen
    {
        public int id { get; set; } // PK
        public string imagen { get; set; }  
        public int idproducto { get; set; }
         
        public Producto Producto { get; set; }
    }
}