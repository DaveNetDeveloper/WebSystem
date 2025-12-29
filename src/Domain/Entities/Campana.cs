using System.Collections.Generic;
using System.Text.Json.Serialization;

using Utilities;

namespace Domain.Entities
{
    /// <summary> Entidad con datos de una campaña </summary>
    public class Campana
    {
        /// <summary> Id de la campaña <summary>
        public int id { get; set; }

        /// <summary> Tipo de la campaña <summary>
        public Guid idTipoCampana { get; set; }

        /// <summary> Nombre de la campaña <summary>
        public string nombre { get; set; }

        /// <summary> Estado de activación de la campaña <summary>
        public bool activo { get; set; }

        /// <summary> Descripción de la campaña <summary>
        public string? descripcion { get; set; }

        /// <summary> Frecuencia de la campaña <summary>
        public string frecuencia { get; set; }

        /// <summary> Fecha de inicio de la campaña <summary>
        public DateTime fechaInicio { get; set; }

        /// <summary> Fecha de fin de la campaña <summary>
        public DateTime? fechaFin { get; set; }

        [JsonIgnore]
        public ICollection<CampanaAcciones> CampanaAcciones { get; set; }

        [JsonIgnore]
        public ICollection<CampanaSegmentos> CampanaSegmentos { get; set; }

        /// <summary> Clase estática que define las constantes para el tipo de frecuencia de una una campaña <summary>
        public static class TipoFrecuencia
        {
            public const string UnaVez = "UnaVez";
            public const string Diaria = "Diaria";
            public const string Semanal = "Semanal";
            public const string Mensual = "Mensual";
        }
        
        /// <summary> Determina si la campaña debe ejecutarse ahora en funcion de la hora actual, el tipo de frecuencia, la fecha de inicio, 
        ///           la fecha fin, si se ha ejecutado o no y con que resultado </summary>
        /// <param name="lastCampaignExecution"> Entidad de tipo 'CampanaExecution' con los datos de la última ejecución de la campaña </param> 
        public bool IsTimeToRun(CampanaExecution? lastCampaignExecution) {

            var now = DateTime.UtcNow;
            bool dentroDeRango = now >= fechaInicio && (fechaFin == null || now <= fechaFin.Value);
            if (!dentroDeRango) return false; // Esta condicion es comun para todos los casos

            bool hasExecution = null != lastCampaignExecution;
            string? estadoEjecucion = !hasExecution ? null : lastCampaignExecution.estado;
            DateTime? lastDate = lastCampaignExecution != null ? lastCampaignExecution.fechaEjecucion : null;
            bool IsFirstTimeOrRetry() => !hasExecution || estadoEjecucion == CampanaExecution.EstadoEjecucion.Error;

            switch (frecuencia)  
            {
                case TipoFrecuencia.UnaVez:
                    // Se ejecuta si:
                    //   - nunca se ha ejecutado o si se ejecutó con estado 'Error' (permitir reintento)
                    //   - ahora >= fechaInicio AND (fechaFin == null OR ahora <= fechaFin)

                    if (IsFirstTimeOrRetry())
                        return true;
                    break;

                case TipoFrecuencia.Diaria:
                    // Se ejecuta si:
                    //   - lastCampaignExecution no es del dia de hoy AND hace mas de un dia de la ejecucio anterior
                    //   - OR lastCampaignExecution == null
                    //   - AND  ha llegado la fechaInicio AND no ha llegado fecha fin (si la hay) 

                    int hoursFromExecution = !hasExecution ? 0 : TimeHelper.GetTimeFromDate(lastDate.Value, TimeHelper.TipoIntervalo.Horas);
                    if (IsFirstTimeOrRetry() || hoursFromExecution >= 24)
                        return true;
                    break;

                case TipoFrecuencia.Semanal:
                    // Se ejecuta si:
                    //  - (lastCampaignExecution no es de la semana actual AND hace mas de una semana de la ejecucio anterior
                    //  - OR lastCampaignExecution == null
                    //  - AND  ha llegado la fechaInicio AND no ha llegado fecha fin (si la hay) 

                    int daysFromExecution = !hasExecution ? 0 : TimeHelper.GetTimeFromDate(lastDate.Value, TimeHelper.TipoIntervalo.Dias);
                    if (IsFirstTimeOrRetry() || daysFromExecution >= 7)
                            return true; 
                    break;

                case TipoFrecuencia.Mensual:
                    // Se ejecuta si:
                    //  - lastCampaignExecution no es del mes actual AND hace mas de un mes de la ejecucio anterior
                    //  - OR lastCampaignExecution == null
                    //  - AND  ha llegado la fechaInicio AND no ha llegado fecha fin (si la hay)  

                    int monthsFromExecution = !hasExecution ? 0 : TimeHelper.GetTimeFromDate(lastDate.Value, TimeHelper.TipoIntervalo.Meses);
                    if (IsFirstTimeOrRetry() || monthsFromExecution >= 1)
                        return true;
                    break;
            }
            return false;
        }
    }
}