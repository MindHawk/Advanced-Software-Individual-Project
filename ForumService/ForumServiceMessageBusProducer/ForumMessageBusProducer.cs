using System.Text;
using ForumServiceModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace ForumServiceMessageBusProducer;

public class ForumMessageBusProducer
{
    private readonly ILogger<ForumMessageBusProducer> _logger;
    private readonly IConfiguration _configuration;
    private readonly ConnectionFactory _connectionFactory;
    public ForumMessageBusProducer(ILogger<ForumMessageBusProducer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _connectionFactory = new ConnectionFactory() { HostName = _configuration["RabbitMQ:Host"] };
    }

    public void SendForumCreatedMessage(ForumShared forum)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        
        channel.ExchangeDeclare(exchange: "post_exchange", ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
        var message = JsonConvert.SerializeObject(forum);
        var body = Encoding.UTF8.GetBytes(message);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(exchange: "post_exchange",
            routingKey: "forum_created",
            basicProperties: null,
            body: body);
        _logger.LogInformation("Sent forum created message: {Message} to message bus", message);
    }

    public void SendForumDeletedMessage(ForumShared forum)
    {
        
    }
}