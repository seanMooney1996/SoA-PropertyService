using Database.Models;

namespace Database.Repositories;

public interface ILandlordRepository
{
    Task<IEnumerable<Landlord>> GetAllAsync(); 
    Task<Landlord?> GetByIdAsync(Guid id);
    Task AddAsync(Landlord landlord);
    Task SaveChangesAsync();
}