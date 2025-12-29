namespace Application.DTOs.Responses
{
    public class CategoriaDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public Guid IdTipoEntidad { get; set; }
        public string NombreTipoEntidad { get; set; }

        public int IdEntidad { get; set; }
        public string NombreEntidad { get; set; }

    }
}