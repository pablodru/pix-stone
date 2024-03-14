using System.Text;
using System.Text.Json;
using Pix.DTOs;
using Pix.Models;
using RabbitMQ.Client;

namespace Pix.RabbitMQ;

public class PaymentProducer
{
    public void PublishPayment(CreatePaymentDTO dto, CreatePaymentResponse response)
    {
        ConnectionFactory _connectionFactory = new()
        {
            HostName = "localhost",
            UserName = "admin",
            Password = "admin"
        };
        string queueName = "Payments";
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var message = new PaymentMessage { DTO = dto, Response = response };
        var jsonMessage = message.SerializeToJson();
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        var properties = channel.CreateBasicProperties();
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