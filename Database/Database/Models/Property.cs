namespace Database.Models;

public class Property
{
    public Guid Id { get; set; }
    public Guid LandlordId { get; set; }
    public string? AddressLine1 { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public long RentPrice { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}