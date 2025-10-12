using Application.DTOs.Requests;
using Application.Interfaces.Messaging;
using Domain.Entities;

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Messaging
{
    public class RabbitMqConsumer : IMessageConsumer
    {
        private readonly MQServiceConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMqConsumer(IOptions<MQServiceConfiguration> config,
                                IServiceScopeFactory scopeFactory)
        {
            _config = config.Value;
            _scopeFactory = scopeFactory;
        }

        public void StartConsuming(string queueName, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Consumidor escuchando la cola '{queueName}'");

            var factory = new ConnectionFactory {
                HostName = _config.HostName,
                UserName = _config.UserName,
                Password = _config.Password
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: queueName, 
                durable: true, 
                exclusive: false, 
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", $"dlx.{queueName}" }, // exchange DLQ
                    { "x-dead-letter-routing-key", $"{queueName}.dlq" } // clave DLQ
                }
            );

            // Declarar la cola DLQ
            channel.QueueDeclare(
                queue: $"{queueName}.dlq",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            // Declarar el exchange para la DLQ
            channel.ExchangeDeclare(
                exchange: $"dlx.{queueName}",
                type: ExchangeType.Direct,
                durable: true
            );

            // Enlazar la DLQ a su exchange
            channel.QueueBind(
                queue: $"{queueName}.dlq",
                exchange: $"dlx.{queueName}",
                routingKey: $"{queueName}.dlq"
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<NotificationRequest>(json);

                Console.WriteLine($"Mensaje recibido: {message}");

                using var scope = _scopeFactory.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<INotificationProcessor>();

                try { 
                    await processor.ProcessAsync(message!); // llama al servicio para procesar la notificación
                    channel.BasicAck(
                            deliveryTag: ea.DeliveryTag, 
                            multiple: false);

                    Console.WriteLine("Mensaje procesado correctamente.");
                }
                catch (OperationCanceledException) {
                    channel.BasicNack(
                            deliveryTag: ea.DeliveryTag, 
                            multiple: false, 
                            requeue: true);

                    Console.WriteLine("Error procesando mensaje. Se reintentara. El mensaje vuelve a la cola");
                }
                catch (Exception ex) {
                    channel.BasicNack(
                            deliveryTag: ea.DeliveryTag, 
                            multiple: false, 
                            requeue: false);

                    Console.WriteLine("Error procesando mensaje. No reencolar. El mensaje se enviará a DLQ.");
                }
            };
            channel.BasicConsume(
                queue: queueName, 
                autoAck: false, 
                consumer: consumer);
        }
    }
}