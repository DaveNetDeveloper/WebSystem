using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    public static class EncryptHelper
    {
        public static string ComputeSha256Base64(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}