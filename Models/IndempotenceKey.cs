namespace Pix.Models;

public class PaymentIndempotenceKey(int keyId, int accountId, int amount)
{
    public int KeyId { get; } = keyId;
    public int Amount { get; } = amount;
    public int AccountId { get; } = accountId;
}