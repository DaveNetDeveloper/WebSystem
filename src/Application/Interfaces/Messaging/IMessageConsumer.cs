using Domain.Entities;

namespace Application.Interfaces.Messaging
{
    public interface IMessageConsumer
    {
        void StartConsuming(string queueName, CancellationToken cancellationToken = default);
    }
}