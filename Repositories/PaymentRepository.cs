using Microsoft.EntityFrameworkCore;
using Pix.Data;
using Pix.Models;

namespace Pix.Repositories;

public class PaymentRepository(AppDBContext context)
{
    private readonly AppDBContext _context = context;

    public async Task<Payment?> GetPaymentByIndempotenceKey(PaymentIndempotenceKey key, int time)
    {
        DateTime seconds = DateTime.UtcNow.AddSeconds(-time);
        return await _context.Payments
                .Where(p => p.KeyId == key.KeyId && p.AccountId == key.AccountId && p.Amount == key.Amount)
                .Where(p => p.CreatedAt >= seconds)
                .FirstOrDefaultAsync();
    }

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

    public async Task<Payment> PaymentFail(Payment payment)
    {
        payment.Status = "FAILED";
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment?> GetPaymentById(int id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<Payment> UpdatePayment(Payment payment, string status)
    {
        payment.Status = status;
        await _context.SaveChangesAsync();
        return payment;
    }
}