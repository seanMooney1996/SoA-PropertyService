using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models;

public class Tenant
{
  public Guid Id { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  
  public ICollection<RentalRequest> Requests { get; set; }
  
  public Guid AuthenticationId { get; set; }

  [ForeignKey("AuthenticationId")]
  public virtual Authentication Authentication { get; set; }
}