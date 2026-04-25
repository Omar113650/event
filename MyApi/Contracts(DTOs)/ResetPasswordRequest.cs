namespace Eventix_Project.DTOs.Auth;

public class ResetPasswordRequest
{
    public string UserId { get; set; }
    public string ResetPasswordToken { get; set; }
    public string Password { get; set; }
}