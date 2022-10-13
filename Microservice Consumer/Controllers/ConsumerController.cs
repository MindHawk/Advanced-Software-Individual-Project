using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Microservice_Consumer.Controllers;

[ApiController]
[Route("[controller]")]
public class ConsumerController : ControllerBase
{
    private readonly ILogger<ConsumerController> _logger;

    public ConsumerController(ILogger<ConsumerController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Consume")]
    public IActionResult Get()
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "task_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.Log(LogLevel.Information, "Message bus message received: {message}", message);
                int dots = message.Split('.').Length - 1;
                Thread.Sleep(dots * 1000);
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            
            channel.BasicConsume(queue: "task_queue",
                autoAck: false,
                consumer: consumer);
        }
        return Ok();
    }
}