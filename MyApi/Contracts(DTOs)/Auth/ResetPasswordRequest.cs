namespace Eventix_Project.DTOs.Auth;

public class ResetPasswordRequest
{
    public string UserId { get; set; } = string.Empty;
    public string ResetPasswordToken { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}