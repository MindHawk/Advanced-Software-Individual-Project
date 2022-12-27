using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SharedDTOs;

namespace ForumServiceMessageBusProducer;

public class ForumMessageBusProducer
{
    private readonly ILogger<ForumMessageBusProducer> _logger;
    private readonly IConfiguration _configuration;
    public ForumMessageBusProducer(ILogger<ForumMessageBusProducer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void SendMessage(ForumShared forum)
    {
        var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQ:Host"] };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "forum_created", ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
            
            var message = JsonConvert.SerializeObject(forum);
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