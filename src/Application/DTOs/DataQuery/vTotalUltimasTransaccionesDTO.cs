namespace Application.DTOs.DataQuery
{
    public class vTotalUltimasTransaccionesDTO
    {
        public DateTime? Dia { get; }
        public int? TotalBienvenida { get; }
        public int? TotalCompletarPerfil { get; }
        public int? TotalQR { get; }
        public int? TotalEvento { get; }
        public int? TotalTransacciones { get; }

        public vTotalUltimasTransaccionesDTO(DateTime? dia,
                                              int? totalBienvenida,
                                              int? totalCompletarPerfil, 
                                              int? totalQR,
                                              int? totalEvento,
                                              int? totalTransacciones) {
            Dia = dia;
            TotalBienvenida = totalBienvenida;
            TotalCompletarPerfil = totalCompletarPerfil;
            TotalQR = totalQR;
            TotalEvento = totalEvento;
            TotalTransacciones = totalTransacciones;
        }
    }
}