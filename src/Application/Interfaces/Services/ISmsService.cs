using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ISmsService
    {
        Task SendAsync(string to, string message);
    }
}