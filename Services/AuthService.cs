using BCrypt.Net;
using Eventix_Project.Data;
using Eventix_Project.DTOs.Auth;
using Eventix_Project.Models;
using Eventix_Project.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eventix_Project.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // ================= REGISTER =================
    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        var userExists = await _context.Users.AnyAsync(x => x.Email == request.Email);
        if (userExists) throw new Exception("User already exists");

        if (request.Password != request.ConfirmPassword)
            throw new Exception("Passwords do not match");

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsAccountVerified = false,
            Otp = "123456",
            OtpExpiresAt = DateTime.UtcNow.AddMinutes(10)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == request.RoleName);

        if (role != null)
        {
            _context.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            });

            await _context.SaveChangesAsync();
        }

        return "User registered successfully";
    }

    // ================= LOGIN =================
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new Exception("Invalid credentials");

        if (!user.IsAccountVerified)
            throw new Exception("Email not verified");

        var roles = user.UserRoles.Select(x => x.Role!.Name!).ToList();

        return new AuthResponse
        {
            UserId = user.Id,
            FullName = user.FullName!,
            Email = user.Email,
            Token = GenerateToken(user, roles),
            Roles = roles
        };
    }

    // ================= VERIFY OTP =================
    public async Task<bool> VerifyOtpAsync(VerifyOtpRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null) return false;

        if (user.Otp != request.Otp) return false;
        if (user.OtpExpiresAt < DateTime.UtcNow) return false;

        user.IsAccountVerified = true;
        user.Otp = null;

        await _context.SaveChangesAsync();
        return true;
    }

    // ================= FORGOT PASSWORD =================
    public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null) throw new Exception("User not found");

        user.ResetPasswordToken = Guid.NewGuid().ToString();
        await _context.SaveChangesAsync();

        return user.ResetPasswordToken!;
    }

    // ================= RESET PASSWORD =================
    public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request)
    {
        if (!int.TryParse(request.UserId, out int userId))
            return false;

        var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.Id == userId &&
            x.ResetPasswordToken == request.ResetPasswordToken);

        if (user == null) return false;

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.ResetPasswordToken = null;

        await _context.SaveChangesAsync();
        return true;
    }

    // ================= JWT =================
    private string GenerateToken(User user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}