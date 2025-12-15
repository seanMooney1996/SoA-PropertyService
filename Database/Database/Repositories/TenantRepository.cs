using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly PropertyServiceContext _context;

    public TenantRepository(PropertyServiceContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Tenant>> GetAllAsync()
    {
        return await _context.Tenants.ToListAsync();
    }

    public async Task DeleteAsync(Tenant tenant)
    {
        _context.Tenants.Remove(tenant);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Tenants.AnyAsync(e => e.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Tenant?> GetByIdAsync(Guid id)
    {
        return await _context.Tenants.FindAsync(id);
    }

    public async Task<Tenant?> GetByAuthenticationIdAsync(Guid authId)
    {
        return await _context.Tenants.FirstOrDefaultAsync(x => x.AuthenticationId == authId);
    }

    public async Task AddAsync(Tenant tenant)
    {
        await _context.Tenants.AddAsync(tenant);
    }
}