using Application.DTOs.Requests;
using Application.Interfaces.Messaging;
using Domain.Entities;

namespace Application.Messaging.Handler
{
    public class NotificationRequestHandler
    {
        private readonly IMessageBusService _messageBus;

        public NotificationRequestHandler(IMessageBusService messageBus)
        {
            _messageBus = messageBus; 
        }

        public async Task HandleAsync(NotificationRequest request, string queueName)
        {
            // TODO: agregar validaciones o logging...
            _messageBus.Publish(request, queueName);
        }
    }
}
