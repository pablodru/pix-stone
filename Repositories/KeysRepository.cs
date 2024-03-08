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

    public async Task<Keys> CreateKey(Keys key)
    {
        var newKey = new Key
        {
            Type = key.Key.Type,
            Value = key.Key.Value
        };
        _context.Keys.Add(newKey);
        await _context.SaveChangesAsync();

        return key;
    }
}