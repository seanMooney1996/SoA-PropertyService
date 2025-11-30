namespace Database.DTOs.Request;

public class RentalRequestDto
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public Guid TenantId { get; set; }
    public string Status { get; set; }
    public string? Message { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public DateTime RequestedAt { get; set; }
    public string? TenantName { get; set; }
    public string? Address { get; set; }
}