using Microsoft.AspNetCore.Mvc;
using System;
using RabbitMQ.Client;
using System.Text;

namespace Microservice_Producer.Controllers;

[ApiController]
[Route("[controller]")]
public class ProducerController : ControllerBase
{
    private readonly ILogger<ProducerController> _logger;

    public ProducerController(ILogger<ProducerController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "Produce")]
    public IActionResult Produce([FromBody]string message)
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "task_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            var body = Encoding.UTF8.GetBytes(message);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "",
                routingKey: "task_queue",
                basicProperties: properties,
                body: body);
            _logger.Log(LogLevel.Information, "Message bus message sent");
        }
        
        return Ok();
    }
}