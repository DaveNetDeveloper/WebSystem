using Application.Interfaces.DTOs.DataQuery;

namespace Application.DTOs.DataQuery
{
    public class vVisitasTipoDispositivoDTO : IView
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