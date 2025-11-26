namespace Database.DTOs.Landlord;

public class LandlordDto
{
    public Guid LandlordId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int PropertyCount { get; set; }
}