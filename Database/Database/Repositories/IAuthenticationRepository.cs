using Database.Models;

namespace Database.Repositories;

using Database.Models;


public interface IAuthenticationRepository
{
    Task<Authentication?> GetByEmailAsync(string email);
    Task AddAsync(Authentication auth);
    Task SaveChangesAsync();
}
