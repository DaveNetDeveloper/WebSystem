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
        /// <returns> string </returns>
        public static string GenerarCodigo()
        {
            string numeros = "";
            for (int i = 0; i < 4; i++)
            {
                numeros += random.Next(0, 10); // genera un dígito entre 0 y 9
            } 
            return $"REC_{numeros}";
        } 
    }
}