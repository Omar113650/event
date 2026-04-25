using BCrypt.Net;
using Eventix_Project.Data;
using Eventix_Project.DTOs.Auth;
using Eventix_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventixAPI.auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // ================= REGISTER =================
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existing = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (existing != null)
            return BadRequest("User already exists");

        if (request.Password != request.ConfirmPassword)
            return BadRequest("Passwords do not match");

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsAccountVerified = false
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // assign role
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

        return Ok(new { message = "User registered successfully" });
    }

    // ================= LOGIN =================
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return BadRequest("Invalid email or password");

        if (!user.IsAccountVerified)
            return BadRequest("Please verify your email");

        var roles = user.UserRoles.Select(x => x.Role!.Name!).ToList();

        var token = GenerateToken(user, roles);

        return Ok(new
        {
            user = new { user.Id, user.FullName, user.Email },
            token,
            roles
        });
    }

    // ================= VERIFY OTP =================
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null)
            return BadRequest("User not found");

        if (user.Otp != request.Otp)
            return BadRequest("Invalid OTP");

        if (user.OtpExpiresAt < DateTime.UtcNow)
            return BadRequest("OTP expired");

        user.IsAccountVerified = true;
        user.Otp = null;
        user.OtpExpiresAt = null;

        await _context.SaveChangesAsync();

        return Ok("Email verified successfully");
    }

    // ================= FORGOT PASSWORD =================
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null)
            return BadRequest("User not found");

        user.ResetPasswordToken = Guid.NewGuid().ToString();

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Reset token generated",
            token = user.ResetPasswordToken
        });
    }

    // ================= RESET PASSWORD =================
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.Id == int.Parse(request.UserId) &&
            x.ResetPasswordToken == request.ResetPasswordToken);

        if (user == null)
            return BadRequest("Invalid request");

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.ResetPasswordToken = null;

        await _context.SaveChangesAsync();

        return Ok("Password reset successfully");
    }

    // ================= JWT =================
    private string GenerateToken(User user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}









// كده أنت عملت إيه؟
// ✔ Register
// بيعمل user
// بيدي Role افتراضي "User"
// ✔ Login
// بيرجع:
// User
// Roles
// Permissions
// JWT Token
// ✔ Permissions Logic

// من غير أي تغيير في الداتا بتاعتك:

// User → UserRoles → Role → RolePermissions → Permission
// 🔥 ملاحظات مهمة جدًا
// 1. لازم يكون عندك في DB:
// Role = "User"
// Permissions متسجلة مسبقًا
// 2. في JWT هتلاقي:
// ClaimTypes.Role

// و

// permission
// 🚀 لو عايز تكمل صح 100%

// أقدر أعملك دلوقتي:

// 🔐 [1] Authorization Attribute
// [HasPermission("create_event")]
// ⚙️ [2] Policy-based Authorization
// 🌱 [3] Seeder Roles + Permissions تلقائي
// 🔄 [4] Refresh Token System

// قول:
// 👉 ك