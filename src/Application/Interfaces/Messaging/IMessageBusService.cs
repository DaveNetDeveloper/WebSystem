using Domain.Entities;

namespace Application.Interfaces.Messaging
{
    public interface IMessageBusService
    {
        void Publish<T>(T message, string queueName);
    }
}