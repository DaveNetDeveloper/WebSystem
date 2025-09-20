using Microsoft.AspNetCore.Identity;

namespace Utilities
{
    public static class PasswordHelper
    {
        private static readonly IPasswordHasher<string> _passwordHasher = new PasswordHasher<string>();

        public static string HashPassword(string password) {
            return _passwordHasher.HashPassword(null, password);
        }

        public static bool VerifyPassword(string password, string hashedPassword) {
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, password) == PasswordVerificationResult.Success;
        }
    }
}