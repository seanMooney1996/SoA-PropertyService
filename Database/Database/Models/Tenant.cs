namespace Database.Models;

public class Tenant
{
  public Guid Id { get; set; }
  public Guid? PropertyId { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
}