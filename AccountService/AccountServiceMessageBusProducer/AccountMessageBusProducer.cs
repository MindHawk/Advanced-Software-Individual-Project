using System.Text;
using AccountServiceModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AccountServiceMessageBusProducer;

public class AccountMessageBusProducer
{
    private readonly ILogger<AccountMessageBusProducer> _logger;
    private readonly IConfiguration _configuration;
    private readonly ConnectionFactory _connectionFactory;
    public AccountMessageBusProducer(ILogger<AccountMessageBusProducer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _connectionFactory = new ConnectionFactory() { HostName = _configuration["RabbitMQ:Host"] };
    }

    public void SendAccountCreatedMessage(Account account)
    {
        IModel channel = SetupChannel(account, out string message, out byte[] body);

        channel.BasicPublish(exchange: "forum_exchange",
            routingKey: "account_created",
            basicProperties: null,
            body: body);
        channel.BasicPublish(exchange: "post_exchange",
            routingKey: "account_created",
            basicProperties: null,
            body: body);
        _logger.LogInformation("Sent account created message: {Message} to message bus", message);
    }

    public void SendAccountDeletedMessage(Account account)
    {
        IModel channel = SetupChannel(account, out string message, out byte[] body);

        channel.BasicPublish(exchange: "forum_exchange",
            routingKey: "account_deleted",
            basicProperties: null,
            body: body);
        channel.BasicPublish(exchange: "post_exchange",
            routingKey: "account_deleted",
            basicProperties: null,
            body: body);
        _logger.LogInformation("Sent account deleted message: {Message} to message bus", message);
    }

    private IModel SetupChannel(Account account, out string message, out byte[] body)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "forum_exchange", ExchangeType.Direct, durable: true, autoDelete: false,
            arguments: null);
        channel.ExchangeDeclare(exchange: "post_exchange", ExchangeType.Direct, durable: true, autoDelete: false,
            arguments: null);
        message = JsonConvert.SerializeObject(account);
        body = Encoding.UTF8.GetBytes(message);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        return channel;
    }
}