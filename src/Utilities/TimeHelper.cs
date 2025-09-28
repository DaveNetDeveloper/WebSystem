 using System.Text;

namespace Utilities
{
    public static class TimeHelper
    {
        public static int GetTimeFromDate(DateTime ultimaEjecucion, TipoIntervalo intervalo)
        {
            var ahora = DateTime.UtcNow;
            var diferencia = ahora - ultimaEjecucion;

            return intervalo switch
            {
                TipoIntervalo.Horas => (int)diferencia.TotalHours,
                TipoIntervalo.Dias => (int)diferencia.TotalDays,
                TipoIntervalo.Meses => GetFullMonthsDifference(ultimaEjecucion, ahora),
                _ => throw new ArgumentOutOfRangeException(nameof(intervalo), intervalo, null)
            };
        }

        public static int GetFullMonthsDifference(DateTime desde, DateTime hasta)
        {
            int months = (hasta.Year - desde.Year) * 12 + hasta.Month - desde.Month;
            if (hasta.Day < desde.Day)  // si el día actual es menor que el día de ejecución, aún no pasó un mes completo
                months--;
            return Math.Max(0, months);
        }

        public enum TipoIntervalo
        {
            Horas,
            Dias,
            Semanas,
            Meses
        }
    }
}