using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

public class RentalRequest
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; }  

    public Guid PropertyId { get; set; }
    [ForeignKey("PropertyId")]
    public Property Property { get; set; }

    public string Status { get; set; } = "Pending";
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
}