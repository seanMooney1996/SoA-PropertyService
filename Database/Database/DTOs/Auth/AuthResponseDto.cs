namespace Database.DTOs.Auth;

public class AuthResponseDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}