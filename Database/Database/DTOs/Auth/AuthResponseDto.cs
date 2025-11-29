namespace Database.DTOs.Auth;

public class AuthResponseDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    
    public string FirstName { get; set; }
    
    public string Role { get; set; }
}