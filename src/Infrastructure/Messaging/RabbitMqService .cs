using Application.Interfaces.Messaging;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Utilities;

namespace Infrastructure.Messaging
{
    public class RabbitMqService : IMessageBusService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService(IOptions<MQServiceConfiguration> options)
        {
            var config = options.Value;

            var factory = new ConnectionFactory()
                {
                    HostName = config.HostName,
                    Port = Convert.ToInt32(EncodeDecodeHelper.GetDecodeValue(config.Port)),
                    UserName = EncodeDecodeHelper.GetDecodeValue(config.UserName),
                    Password = EncodeDecodeHelper.GetDecodeValue(config.Password)
                };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Publish<T>(T message, string queueName)
        {
            _channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", $"dlx.{queueName}" },
                    { "x-dead-letter-routing-key", $"{queueName}.dlq" }
                }
            );

            //_channel.QueueDeclare(queue: queueName,
            //                       durable: true,
            //                       exclusive: false,
            //                       autoDelete: false,
            //                       arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(exchange: "",
                                  routingKey: queueName,
                                  basicProperties: null,
                                  body: body);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}