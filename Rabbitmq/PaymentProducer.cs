using System.Text;
using System.Text.Json;
using Pix.DTOs;
using RabbitMQ.Client;

namespace Pix.RabbitMQ;

public class PaymentProducer
{
    private readonly ConnectionFactory _connectionFactory = new()
    {
        HostName = "localhost",
        UserName = "admin",
        Password = "admin"
    };
    string queueName = "Payments";
    public void PublishPayment(CreatePaymentDTO dto)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var jsonMessage = JsonSerializer.Serialize(dto);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: "",
                             basicProperties: null,
                             body: body);

        Console.WriteLine("Payment message sent: {0}", jsonMessage);
    }
}