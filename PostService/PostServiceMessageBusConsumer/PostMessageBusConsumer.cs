using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PostServiceModels.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedDTOs;

namespace PostServiceMessageBus;

public class PostMessageBusConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PostMessageBusConsumer> _logger;
    private IModel _channel;
    private string _queueName;

    public PostMessageBusConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory, ILogger<PostMessageBusConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        
        var factory = new ConnectionFactory() { HostName = configuration["RabbitMQ:Host"] };
        IConnection connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.ExchangeDeclare(exchange:"forum_created", type:ExchangeType.Direct);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: _queueName, exchange: "forum_created", routingKey: "forum_created");
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
            if (ParseMessage(message))
            {
                _channel.BasicAck(ea.DeliveryTag, false);
            }
            else
            {
                _logger.LogWarning("Message could not be parsed: {Message}", message);
            }
        };
        
        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        
        return Task.CompletedTask;
    }

    private bool ParseMessage(string message)
    {
        _logger.LogInformation("Parsing message: {Message}", message);
        using IServiceScope scope = _scopeFactory.CreateScope();
        ForumShared? forum = JsonConvert.DeserializeObject<ForumShared>(message);
        if (forum?.Name == null)
        {
            _logger.LogWarning("Message is not a forum: {Message}", message);
            return false;
        }

        var logic = scope.ServiceProvider.GetRequiredService<IPostMessageBusLogic>();
        return logic.AddForum(forum);
    }
}