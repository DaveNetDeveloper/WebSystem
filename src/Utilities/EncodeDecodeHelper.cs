 using System.Text;

namespace Utilities
{
    public static class EncodeDecodeHelper
    { 
        public static string GetEncodeValue(string value) {

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        //
        public static string GetDecodeValue(string value)
        { 
            byte[] bytesDecodificados = Convert.FromBase64String(value);
            string textoDecodificado = Encoding.UTF8.GetString(bytesDecodificados);
            return textoDecodificado;
        }
    }
}