using Application.Interfaces.DTOs.DataQuery;

namespace Application.DTOs.DataQuery
{
    public class vTotalErroresDTO : IView
    {
        public DateTime? Dia { get; }
        public int? TotalErrores { get; }
        public string? Proceso { get; }

        public vTotalErroresDTO(DateTime? dia, 
                                int? totalErrores,
                                string? proceso) {
            Dia = dia;
            TotalErrores = totalErrores;
            Proceso = proceso;
        }
    }
}