using Pix.Data;
using Pix.Models;

namespace Pix.Repositories;

public class PaymentRepository(AppDBContext context)
{
    private readonly AppDBContext _context = context;

    public async Task<Payment> CreatePayment(CreatePayment payment, int keyId, int AccountId)
    {
        Payment newPayment = new Payment
        {
            Status = "PROCESSING",
            KeyId = keyId,
            AccountId = AccountId,
            Amount = payment.Amount,
            Description = payment.Description
        };
        _context.Payments.Add(newPayment);
        await _context.SaveChangesAsync();
        
        return newPayment;
    }
}