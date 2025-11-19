using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public DateTime ExpiresAtUtc { get; set; }
    }
}
