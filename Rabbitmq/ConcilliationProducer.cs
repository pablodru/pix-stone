using Pix.DTOs;
using Pix.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Pix.RabbitMQ;

public class ConcilliationProducer
{
    public void PublishConcilliation(ConcilliationDTO dto)
    {
        ConnectionFactory _connectionFactory = new()
        {
            HostName = "localhost",
            UserName = "admin",
            Password = "admin"
        };
        string queueName = "Concilliations";
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.Headers = new System.Collections.Generic.Dictionary<string, object>
        {
            { "retry-count", 0 }
        };

        var message = new ConcilliationMessage { File = dto.File, Postback = dto.Postback, Date = dto.Date };
        var jsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: string.Empty,
                            routingKey: "Concilliations",
                            basicProperties: properties,
                            body: body);
    }
}