using Database.Models;

namespace Database.Repositories;

public interface IRentalRequestRepository
{
    Task<RentalRequest?> GetByIdAsync(Guid id);
    Task<IEnumerable<RentalRequest>> GetByLandlordIdWithDetailsAsync(Guid landlordId);
    Task<IEnumerable<RentalRequest>> GetOtherRequestsForPropertyAsync(Guid propertyId, Guid currentRequestId);
    
    Task AddAsync(RentalRequest request);
    
    Task<IEnumerable<RentalRequest>> GetByTenantIdWithDetailsAsync(Guid tenantId);
    
    Task<bool> HasPendingRequestAsync(Guid tenantId, Guid propertyId);
    
    Task<RentalRequest?> GetByTenantAndPropertyIdAsync(Guid tenantId, Guid propertyId);
    
    Task DeleteAsync(RentalRequest request);
    Task SaveChangesAsync();
}