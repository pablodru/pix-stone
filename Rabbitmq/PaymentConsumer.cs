using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Pix.Models;
using System.Text.Json;
using Pix.DTOs;

namespace Pix.RabbitMQ;
public class PaymentConsumer
{
    private readonly ConnectionFactory _connectionFactory = new()
    {
        HostName = "localhost",
        UserName = "admin",
        Password = "admin"
    };

    public void ConsumePayments()
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();
        string queueName = "Payments";

        channel.QueueDeclare(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        Console.WriteLine("[*] Waiting for messages...");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var jsonMessage = Encoding.UTF8.GetString(body);
            var payment = JsonSerializer.Deserialize<CreatePaymentDTO>(jsonMessage);

            Console.WriteLine("Received payment message: {0}", jsonMessage);

            // Process payment here
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        Console.WriteLine("Waiting for payment messages...");
        Console.ReadLine();
    }
}
