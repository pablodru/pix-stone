using Microsoft.EntityFrameworkCore;
using Pix.Models;
using Pix.Data;

namespace Pix.Repositories;

public class UserRepository
{
    private readonly AppDBContext _context;

    public UserRepository(AppDBContext context)
    {
        _context = context;
    }
    public async Task<User?> GetUserByCPF(string cpf)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.CPF.Equals(cpf));
    }
}