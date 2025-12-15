using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

public class Landlord
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<Property> Properties { get; set; }
    
    public Guid AuthenticationId { get; set; } 
    
    [ForeignKey("AuthenticationId")]
    public Authentication Authentication { get; set; }
}