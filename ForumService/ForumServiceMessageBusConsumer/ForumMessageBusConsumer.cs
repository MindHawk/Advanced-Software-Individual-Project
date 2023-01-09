using System.Text;
using ForumServiceModels;
using ForumServiceModels.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ForumServiceMessageBus;

public class ForumMessageBusConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ForumMessageBusConsumer> _logger;
    private readonly IModel _channel;
    private readonly string _queueName;

    public ForumMessageBusConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory, ILogger<ForumMessageBusConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        
        var factory = new ConnectionFactory() { HostName = configuration["RabbitMQ:Host"] };
        IConnection connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.ExchangeDeclare(exchange:"forum_exchange", type:ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: _queueName, exchange: "forum_exchange", routingKey: "account_created");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        
        consumer.Received += (moduleHandle, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Message received: {Message}", message);
            bool success = false;
            switch (ea.RoutingKey)
            {
                case "account_created":
                    success = ParseAccountCreatedMessage(message);
                    break;
                default:
                    _logger.LogError("No handler for routing key {RoutingKey}", ea.RoutingKey);
                    break;
            }
            if (success)
            {
                _channel.BasicAck(ea.DeliveryTag, false);
                _logger.LogInformation("Message processed: {Message}", message);
            }
            else
            {
                _logger.LogWarning("Message could not be parsed: {Message}", message);
            }
        };
        
        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        
        return Task.CompletedTask;
    }

    private bool ParseAccountCreatedMessage(string message)
    {
        _logger.LogInformation("Parsing forum created message: {Message}", message);
        using IServiceScope scope = _scopeFactory.CreateScope();
        Account? account = JsonConvert.DeserializeObject<Account>(message);
        if (account?.Name == null)
        {
            _logger.LogWarning("Message is not a forum: {Message}", message);
            return false;
        }
        
        var logic = scope.ServiceProvider.GetRequiredService<IForumLogic>();
        return logic.AddAccount(account);
    }
}