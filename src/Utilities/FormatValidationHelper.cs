using System.Net.Mail;
using System.Text;

namespace Utilities
{
    public static class FormatValidationHelper
    {
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