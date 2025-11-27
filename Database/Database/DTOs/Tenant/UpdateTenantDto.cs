namespace Database.DTOs.Tenant;

public class UpdateTenantDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Guid? PropertyId { get; set; }
}