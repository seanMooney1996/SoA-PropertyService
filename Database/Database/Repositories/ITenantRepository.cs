using Database.Models;

namespace Database.Repositories;

public interface ITenantRepository
{
    Task<IEnumerable<Tenant>> GetAllAsync();           
    Task<Tenant?> GetByIdAsync(Guid id);
    Task<Tenant?> GetByAuthenticationIdAsync(Guid authId);
    Task AddAsync(Tenant tenant);
    Task DeleteAsync(Tenant tenant);                
    Task<bool> ExistsAsync(Guid id);                 
    Task SaveChangesAsync();
}