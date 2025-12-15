namespace Application.DTOs.Responses
{
    public class ReservaActividadDTO
    {  
        public Guid IdReserva { get; set; }
        public string CodigoReserva { get; set; }
        public int IdActividad { get; set; } 
        public int IdUsuario { get; set; }
        public DateTime FechaActividad { get; set; } 
        public DateTime FechaReserva { get; set; }
        public string EstadoReserva { get; set; }
        public string ImagenActividad { get; set; }
    }
}