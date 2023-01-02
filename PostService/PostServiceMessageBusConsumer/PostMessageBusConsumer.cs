using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PostServiceModels;
using PostServiceModels.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PostServiceMessageBus;

public class PostMessageBusConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PostMessageBusConsumer> _logger;
    private readonly IModel _channel;
    private readonly string _queueName;

    public PostMessageBusConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory, ILogger<PostMessageBusConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        
        var factory = new ConnectionFactory() { HostName = configuration["RabbitMQ:Host"] };
        IConnection connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.ExchangeDeclare(exchange:"post_exchange", type:ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: _queueName, exchange: "post_exchange", routingKey: "forum_created");
        _channel.QueueBind(queue: _queueName, exchange: "post_exchange", routingKey: "forum_deleted");
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
                case "forum_created":
                    success = ParseForumCreatedMessage(message);
                    break;
                case "forum_deleted":
                    
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

    private bool ParseForumCreatedMessage(string message)
    {
        _logger.LogInformation("Parsing forum created message: {Message}", message);
        using IServiceScope scope = _scopeFactory.CreateScope();
        Forum? forum = JsonConvert.DeserializeObject<Forum>(message);
        if (forum?.Name == null)
        {
            _logger.LogWarning("Message is not a forum: {Message}", message);
            return false;
        }
        
        var logic = scope.ServiceProvider.GetRequiredService<IPostMessageBusLogic>();
        return logic.AddForum(forum);
    }
    
    private bool ParseForumDeletedMessage(string message)
    {
        _logger.LogInformation("Parsing forum deleted message: {Message}", message);
        using IServiceScope scope = _scopeFactory.CreateScope();
        Forum? forum = JsonConvert.DeserializeObject<Forum>(message);
        if (forum?.Name == null)
        {
            _logger.LogWarning("Message is not a forum: {Message}", message);
            return false;
        }
        
        var logic = scope.ServiceProvider.GetRequiredService<IPostMessageBusLogic>();
        return logic.AddForum(forum);
    }
}