using Database.Models;

namespace Database.Repositories;

public interface IPropertyRepository
{
    Task<IEnumerable<Property>> GetAllAsync();       
    Task<Property?> GetByIdAsync(Guid id);
    Task<IEnumerable<Property>> GetByLandlordIdAsync(Guid landlordId);
    Task<bool> IsTenantRentingAnyPropertyAsync(Guid tenantId);
    Task AddAsync(Property property);            
    Task DeleteAsync(Property property);   
    
    Task<Property?> GetPropertyByTenantIdAsync(Guid tenantId);
    
    Task<IEnumerable<Property>> GetOpenPropertiesAsync();
    Task<bool> ExistsAsync(Guid id);                     
    Task SaveChangesAsync();
}