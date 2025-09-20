namespace Application.DTOs.DataQuery
{
    public class vVisitasTipoDispositivoDTO
    {
        public DateTime? Semana { get; }
        public string? TipoDispositivo { get; }
        public int Visitas { get; } 

        public vVisitasTipoDispositivoDTO(DateTime? semana,
                                 string? tipoDispositivo, 
                                 int visitas) {
            Semana = semana;
            TipoDispositivo = tipoDispositivo;
            Visitas = visitas; 
        }
    }
}