using System.Text.Json;
using Pix.DTOs;

namespace Pix.Models;

public class CreatePaymentResponse
{
    public int Id { get; set; }
}

public class PaymentMessage
{
    public CreatePaymentDTO DTO { get; set; }
    public CreatePaymentResponse Response { get; set; }

    public string SerializeToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public static PaymentMessage DeserializeFromJson(string json)
    {
        return JsonSerializer.Deserialize<PaymentMessage>(json);
    }
}