using Eventix_Project.DTOs.Auth;

namespace Eventix_Project.Services.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);

    Task<bool> VerifyOtpAsync(VerifyOtpRequest request);
    Task<string> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<bool> ResetPasswordAsync(ResetPasswordRequest request);
}