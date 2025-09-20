namespace Domain.Entities
{
    public class Direccion
    {
        public int id { get; set; }
        public string tipoVia { get; set; }
        public string nombreVia { get; set; }
        public int numero { get; set; }
        public string? bloque { get; set; }
        public string? escalera { get; set; }
        public string? piso { get; set; }
        public int? puerta { get; set; }
        public string codigoPostal { get; set; }
        public string ciudad { get; set; }
        public string? provincia { get; set; }
        public string comunidadAutonoma { get; set; }
        public string pais { get; set; }
  
        public ICollection<UsuarioDireccion> UsuarioDirecciones { get; set; }
    }
}