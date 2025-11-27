namespace Database.DTOs.Property;

public class UpdatePropertyDto
{
    public string? AddressLine1 { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public int? Bedrooms { get; set; }
    public int? Bathrooms { get; set; }
    public long? RentPrice { get; set; }
    public bool? IsAvailable { get; set; }
}