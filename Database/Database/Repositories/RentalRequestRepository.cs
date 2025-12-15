using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public class RentalRequestRepository : IRentalRequestRepository
{
    private readonly PropertyServiceContext _context;

    public RentalRequestRepository(PropertyServiceContext context)
    {
        _context = context;
    }

    public async Task<RentalRequest?> GetByIdAsync(Guid id)
    {
        return await _context.RentalRequests.FindAsync(id);
    }

    public async Task<IEnumerable<RentalRequest>> GetByLandlordIdWithDetailsAsync(Guid landlordId)
    {
        return await _context.RentalRequests
            .Include(r => r.Property) 
            .Include(r => r.Tenant)  
            .Where(r => r.Property.LandlordId == landlordId)
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<RentalRequest>> GetOtherRequestsForPropertyAsync(Guid propertyId, Guid currentRequestId)
    {
        return await _context.RentalRequests
            .Where(r => r.PropertyId == propertyId && r.Id != currentRequestId)
            .ToListAsync();
    }
    
    public async Task AddAsync(RentalRequest request)
    {
        await _context.RentalRequests.AddAsync(request);
    }

    public async Task<IEnumerable<RentalRequest>> GetByTenantIdWithDetailsAsync(Guid tenantId)
    {
        return await _context.RentalRequests
            .Include(r => r.Property)
            .Include(r => r.Tenant)
            .Where(r => r.TenantId == tenantId)
            .OrderByDescending(r => r.RequestedAt)
            .ToListAsync();
    }

    public async Task<bool> HasPendingRequestAsync(Guid tenantId, Guid propertyId)
    {
        return await _context.RentalRequests
            .AnyAsync(r => r.TenantId == tenantId && r.PropertyId == propertyId && r.Status == "Pending");
    }

    public async Task<RentalRequest?> GetByTenantAndPropertyIdAsync(Guid tenantId, Guid propertyId)
    {
        return await _context.RentalRequests
            .FirstOrDefaultAsync(r => r.TenantId == tenantId && r.PropertyId == propertyId);
    }

    public async Task DeleteAsync(RentalRequest request)
    {
        _context.RentalRequests.Remove(request);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}