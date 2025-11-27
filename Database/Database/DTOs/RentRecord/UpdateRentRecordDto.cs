namespace Database.DTOs.RentRecord;

public class UpdateRentRecordDto
{
    public long? RecordedRent { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
}