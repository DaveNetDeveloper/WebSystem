using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}