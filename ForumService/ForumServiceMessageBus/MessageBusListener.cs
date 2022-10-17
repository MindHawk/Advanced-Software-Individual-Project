using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ForumServiceMessageBus;

public class MessageBusListener : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MessageBusListener> _logger;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;

    public MessageBusListener(IConfiguration configuration, IServiceScopeFactory scopeFactory, ILogger<MessageBusListener> logger)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
        _logger = logger;
        
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange:"test-exchange", type:ExchangeType.Direct);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: _queueName, exchange: "test-exchange", routingKey: "test-key");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        
        consumer.Received += (moduleHandle, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            using (var scope = _scopeFactory.CreateScope())
            {
                _logger.LogInformation("Message received: {message}", message);
            }
        };
        
        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        
        return Task.CompletedTask;
    }
}