namespace Database.DTOs.Tenant;

public class TenantDto
{
    public Guid TenantId { get; set; }
    public string FullName { get; set; }
    public Guid? PropertyId { get; set; }
}