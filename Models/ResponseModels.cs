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
    public CreatePaymentResponseMessage Response { get; set; }

    public string SerializeToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public static PaymentMessage DeserializeFromJson(string json)
    {
        return JsonSerializer.Deserialize<PaymentMessage>(json);
    }
}

public class CreatePaymentResponseMessage
{
    public int Id { get; set; }
    public string WebHookDestiny { get; set; }
    public string WebHookOrigin { get; set; }
}
