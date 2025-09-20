namespace Domain.Entities
{
    public sealed class AuthUser
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; 
    }

    public sealed record LoginDto(string UserName, string Password); 
}