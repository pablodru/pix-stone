using Microsoft.EntityFrameworkCore;
using Pix.Models;
using Pix.Data;

namespace Pix.Repositories;

public class KeysRepository
{
    private readonly AppDBContext _context;

    public KeysRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task<KeysToCreate> CreateKey(KeysToCreate key, int accountId)
    {
        var newKey = new Key
        {
            Type = key.Key.Type,
            Value = key.Key.Value,
            AccountId = accountId
        };
        _context.Keys.Add(newKey);
        await _context.SaveChangesAsync();

        return key;
    }

    public async Task<Key?> GetKeyByValue(string value, string type)
    {
        return await _context.Keys
            .FirstOrDefaultAsync(k => k.Value == value && k.Type == type);
    }

    public async Task<KeyWithAccountAndBank?> GetKeyByValueWithAccount(string value, string type)
    {
        var response = await _context.Keys
            .Include(k => k.Account)
                .ThenInclude(a => a.Bank)
            .FirstOrDefaultAsync(k => k.Value == value && k.Type == type);
        
        if (response == null) return null;
        var KeyWithAccount = new KeyWithAccountAndBank
        {
            Id = response.Id,
            Type = response.Type,
            Value = response.Value,
            AccountId = response.AccountId,
            Account = response.Account,
            Bank = response.Account.Bank
        };
        return KeyWithAccount;
    }
}
