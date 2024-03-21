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

    public async Task<AccountWithUserAndBank> GetAccountById(int id)
    {
        var account = await _context.Accounts
            .Include(a => a.User)
            .Include(a => a.Bank)
            .FirstOrDefaultAsync(a => a.Id == id);
        var accountWithUserAndBank = new AccountWithUserAndBank
        {
            Id = account.Id,
            Number = account.Number,
            Agency = account.Agency,
            BankId = account.BankId,
            UserId = account.UserId,
            User = account.User,
            Bank = account.Bank
        };

        return accountWithUserAndBank;
    }

    public async Task<AccountWithUserAndKeys?> GetAccountByNumberAndAgency(string number, string agency, int bankId)
    {

        var account = await _context.Accounts
            .Include(a => a.User)
            .Include(a => a.Keys)
            .FirstOrDefaultAsync(a =>
                a.Number == number &&
                a.Agency == agency &&
                a.BankId == bankId);

        if (account != null)
        {
            var existingAccount = new AccountWithUserAndKeys
            {
                Id = account.Id,
                Number = account.Number,
                Agency = account.Agency,
                UserId = account.UserId,
                User = account.User,
                Keys = account.Keys
            };
            return existingAccount;
        }
        else
        {
            return null;
        }
    }

    public async Task<AccountWithUserAndBank?> GetAccountWithUserAndBank(string number, string agency, int bankId)
    {
        var account = await _context.Accounts
            .Include(a => a.User)
            .Include(a => a.Bank)
            .FirstOrDefaultAsync(a =>
            a.Number == number &&
            a.Agency == agency &&
            a.BankId == bankId);
        if (account != null)
        {
            var accountWithUserAndBank = new AccountWithUserAndBank
            {
                Id = account.Id,
                Number = account.Number,
                Agency = account.Agency,
                BankId = account.BankId,
                UserId = account.UserId,
                User = account.User,
                Bank = account.Bank
            };

            return accountWithUserAndBank;
        }
        else return null;
    }
}