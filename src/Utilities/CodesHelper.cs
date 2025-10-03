using System.Text;

namespace Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class CodesHelper
    { 
        private static Random random = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <returns> codigo de texto en formato [REC_####] </returns>
        public static string GenerarCodigoRecomendacion()
        {
            string numeros = "";
            for (int i = 0; i < 4; i++) {
                numeros += random.Next(0, 10);
            } 
            return $"REC_{numeros}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> codigo de texto en formato [RSV_###_####] </returns>
        public static string GenerarCodigoReservaActividad(int id)
        {
            string formatedId = id.ToString("D3");

            var numeros = string.Empty;
            for (int i = 0; i < 4; i++) {
                numeros += random.Next(0, 10);
            }
            return $"RSV_{formatedId}_{numeros}";
        }
    }
}