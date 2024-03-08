using Microsoft.EntityFrameworkCore;
using Pix.Models;
using Pix.Data;

namespace Pix.Repositories;

public class BankRepository
{
    private readonly AppDBContext _context;

    public BankRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task<Bank?> GetBankByToken(string token)
    {
        return await _context.Banks.FirstOrDefaultAsync(b => b.Token == token);
    }
}