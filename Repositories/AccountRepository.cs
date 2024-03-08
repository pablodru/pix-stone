using Microsoft.EntityFrameworkCore;
using Pix.Models;
using Pix.Data;

namespace Pix.Repositories;

public class AccountRepository
{
    private readonly AppDBContext _context;

    public AccountRepository(AppDBContext context)
    {
        _context = context;
    }
    public async Task<List<AccountWithKeys>?> GetAccountsByUserId(int id)
    {
        var accounts = await _context.Accounts
                .Include(a => a.Keys)
                .Where(a => a.UserId == id)
                .ToListAsync();

        // Mapear objetos Account para AccountWithKeys
        var accountsWithKeys = accounts.Select(a => new AccountWithKeys
        {
            Id = a.Id,
            Number = a.Number,
            Agency = a.Agency,
            BankId = a.BankId,
            UserId = a.UserId,
            Keys = a.Keys
        }).ToList();

        return accountsWithKeys;
    }

    public async Task<Account> CreateAccount(int userId, int bankId, string number, string agency)
    {
        var newAccount = new Account
        {
            Number = number,
            Agency = agency,
            BankId = bankId,
            UserId = userId
        };
        _context.Accounts.Add(newAccount);
        await _context.SaveChangesAsync();

        return newAccount;
    }
}