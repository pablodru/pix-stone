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
            .Include(k => k.Account)
            .FirstOrDefaultAsync(k => k.Value == value && k.Type == type);

    }
}
