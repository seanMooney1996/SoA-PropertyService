namespace Database.DTOs.RentRecord;

public class CreateRentDto
{
    public Guid PropertyId { get; set; }
    public Guid UserId { get; set; }
    public long RecordedRent { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime EffectiveTo { get; set; }
}