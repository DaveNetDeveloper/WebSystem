using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests
{
    public record CreateQRCodeRequest(string Payload, int? Ttl, string Origen);
}
