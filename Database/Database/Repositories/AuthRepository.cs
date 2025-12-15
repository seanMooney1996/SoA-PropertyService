using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly PropertyServiceContext _context;

    public AuthenticationRepository(PropertyServiceContext context)
    {
        _context = context;
    }

    public async Task<Authentication?> GetByEmailAsync(string email)
    {
        return await _context.Authentications.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task AddAsync(Authentication auth)
    {
        await _context.Authentications.AddAsync(auth);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}