using Application.DTOs.Requests;
using Domain.Entities;

namespace Application.Interfaces.Messaging
{
    public interface INotificationProcessor
    { 
        Task ProcessAsync(NotificationRequest message);
    }
}