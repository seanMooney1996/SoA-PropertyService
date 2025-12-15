using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly PropertyServiceContext _context;

    public PropertyRepository(PropertyServiceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await _context.Properties.ToListAsync();
    }

    public async Task<Property?> GetByIdAsync(Guid id)
    {
        return await _context.Properties.FindAsync(id);
    }
    public async Task<IEnumerable<Property>> GetByLandlordIdAsync(Guid landlordId)
    {
        return await _context.Properties.Where(p => p.LandlordId == landlordId).ToListAsync();
    }

    public async Task<bool> IsTenantRentingAnyPropertyAsync(Guid tenantId)
    {
        return await _context.Properties.AnyAsync(p => p.TenantId == tenantId);
    }
    
    public async Task<Property?> GetPropertyByTenantIdAsync(Guid tenantId)
    {
        return await _context.Properties.FirstOrDefaultAsync(p => p.TenantId == tenantId);
    }

    public async Task<IEnumerable<Property>> GetOpenPropertiesAsync()
    {
        return await _context.Properties.Where(p => p.TenantId == null).ToListAsync();
    }

    public async Task AddAsync(Property property)
    {
        await _context.Properties.AddAsync(property);
    }

    public async Task DeleteAsync(Property property)
    {
        _context.Properties.Remove(property);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Properties.AnyAsync(e => e.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}