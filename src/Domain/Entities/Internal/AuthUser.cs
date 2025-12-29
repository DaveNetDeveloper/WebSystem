namespace Domain.Entities
{
    public sealed class AuthUser
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Profile { get; set; } = string.Empty;
        public string[] Entidades { get; set; } = null;
    }

    public sealed record LoginDto(
        string Email,
        string Password,
        string LoginType); 
}