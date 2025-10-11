using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities
{
    public static class FormatValidationHelper
    { 
        /// <summary>
        /// Valida si un string es un número de teléfono español fijo o móvil.
        /// Permite el prefijo internacional (+34 o 0034) y distintos separadores.
        /// </summary>
        /// <param name="numero">El string a validar.</param>
        /// <returns>True si el formato es válido, False en caso contrario.</returns>
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) {
                return false;
            }

            string numLimpio = new string(phone.Where(c => char.IsDigit(c) || c == '+' || c == '0').ToArray());

            // Regex que acepta números de 9 dígitos.
            //  - Comienzan por 6, 7 (móvil) o 8, 9 (fijo).
            //  - Permite el prefijo: (+34) o (0034)
            const string patronRegex = @"^(\+34|0034)?([6789]\d{8})$";

            var match = Regex.Match(numLimpio, patronRegex); 
            return match.Success;
        }

        /// <summary>
        /// Valida si un string tiene formato de email correcto.
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try {
                var mail = new MailAddress(email);
                return true;
            }
            catch {
                return false;
            }
        }


        /// <summary>
        /// Intenta convertir un string en Guid. 
        /// Devuelve Guid? (null si la conversión falla).
        /// </summary>
        public static Guid? GetValidGuidFronString(string valor)
        {
            if (Guid.TryParse(valor, out var guid))
                return guid;

            return null;
        }
    }
}