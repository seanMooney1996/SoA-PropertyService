using Microsoft.EntityFrameworkCore;

namespace Database.Models;

public class PropertyServiceContext: DbContext
{
    public PropertyServiceContext(DbContextOptions<PropertyServiceContext> options)
        : base(options)
    {
    }

    public DbSet<Landlord> Landlords { get; set; } = null!;
    public DbSet<Property> Properties { get; set; } = null!;
    public DbSet<Tenant> Tenants { get; set; } = null!;
    public DbSet<Authentication> Authentications { get; set; } = null!;
    
    public DbSet<RentalRequest> RentalRequests { get; set; } = null!;
    
}