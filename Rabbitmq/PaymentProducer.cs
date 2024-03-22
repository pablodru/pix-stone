using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Pix.Config;
using Pix.DTOs;
using Pix.Models;
using RabbitMQ.Client;

namespace Pix.RabbitMQ;

public class PaymentProducer(IOptions<QueueConfig> queueConfig)
{
    IOptions<QueueConfig> _queueConfig = queueConfig;
    
    public void PublishPayment(CreatePaymentDTO dto, CreatePaymentResponseMessage response)
    {
        ConnectionFactory _connectionFactory = new()
        {
            HostName = _queueConfig.Value.HostName,
            UserName = _queueConfig.Value.UserName,
            Password = _queueConfig.Value.Password,
        };
        string queueName = "Payments";
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var message = new PaymentMessage { DTO = dto, Response = response };
        var jsonMessage = message.SerializeToJson();
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.Headers = new System.Collections.Generic.Dictionary<string, object>
        {
            { "retry-count", 0 }
        };

        channel.BasicPublish(exchange: string.Empty,
                                     routingKey: "Payments",
                                     basicProperties: properties,
                                     body: body);

        Console.WriteLine("Payment message sent: {0}", jsonMessage);
    }
}