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
        /// <returns> codigo de texto en formato [AA#A#A] </returns>
        public static string GenerarCodigoValidacionCuenta()
        {
            const string letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digitos = "0123456789";
            
            char GetRandomLetter() => letras[random.Next(letras.Length)];
            char GetRandomDigit() => digitos[random.Next(digitos.Length)];

            char[] codigo = new char[6];
            codigo[0] = GetRandomLetter();
            codigo[1] = GetRandomLetter();
            codigo[2] = GetRandomDigit(); 
            codigo[3] = GetRandomLetter();
            codigo[4] = GetRandomDigit(); 
            codigo[5] = GetRandomLetter();

            return new string(codigo).ToUpper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> codigo de texto en formato [REC_####] </returns>
        public static string GenerarCodigoRecomendacion()
        {
            string numeros = string.Empty;
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