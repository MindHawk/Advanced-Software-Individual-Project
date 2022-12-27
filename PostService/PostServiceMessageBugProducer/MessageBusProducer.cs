using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace AuthService.Messaging;

public class MessageBusProducer
{
    private readonly ILogger<MessageBusProducer> _logger;
    public MessageBusProducer(ILogger<MessageBusProducer> logger)
    {
        _logger = logger;
    }

    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "forum_created", ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
            
            
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "forum_created",
                routingKey: "forum_created",
                basicProperties: null,
                body: body);
        }
    }
}