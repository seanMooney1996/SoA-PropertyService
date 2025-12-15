using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public class LandlordRepository : ILandlordRepository
{
    private readonly PropertyServiceContext _context;

    public LandlordRepository(PropertyServiceContext context)
    {
        _context = context;
    }

    public async Task<Landlord?> GetByIdAsync(Guid id)
    {
        return await _context.Landlords
            .Include(l => l.Properties) 
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Landlord?> GetByAuthenticationIdAsync(Guid authId)
    {
        return await _context.Landlords
            .FirstOrDefaultAsync(l => l.AuthenticationId == authId);
    }
    
    public async Task<IEnumerable<Landlord>> GetAllAsync()
    {
        return await _context.Landlords.ToListAsync();
    }

    public async Task AddAsync(Landlord landlord)
    {
        await _context.Landlords.AddAsync(landlord);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}