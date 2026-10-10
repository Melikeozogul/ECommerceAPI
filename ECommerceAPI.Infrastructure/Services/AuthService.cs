using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceAPI.Application.DTOs.Auth;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceAPI.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(
        RegisterDto registerDto)
    {
        var existingUser =
            await _userManager.FindByEmailAsync(registerDto.Email);

        if (existingUser is not null)
        {
            throw new Exception(
                "Bu email adresi zaten kayıtlı.");
        }

        var user = new ApplicationUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName
        };

        var result = await _userManager.CreateAsync(
            user,
            registerDto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(e => e.Description));

            throw new Exception(errors);
        }

        var roleResult = await _userManager.AddToRoleAsync(
    user,
    "Customer");

    if (!roleResult.Succeeded)
    {
        var errors = string.Join(
            ", ",
            roleResult.Errors.Select(e => e.Description));

        throw new Exception(errors);
    }

        return new AuthResponseDto
        {
            Token = await GenerateTokenAsync(user),
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(
        LoginDto loginDto)
    {
        var user =
            await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null)
            return null;

        var passwordValid =
            await _userManager.CheckPasswordAsync(
                user,
                loginDto.Password);

        if (!passwordValid)
            return null;

        return new AuthResponseDto
        {
            Token = await GenerateTokenAsync(user),
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }


private async Task<string> GenerateTokenAsync(ApplicationUser user)
{
    var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Id),
        new(JwtRegisteredClaimNames.Email, user.Email!),
        new(ClaimTypes.Name, user.UserName!),
        new("FirstName", user.FirstName),
        new("LastName", user.LastName)
    };

    var roles = await _userManager.GetRolesAsync(user);

    foreach (var role in roles)
    {
        claims.Add(new Claim(ClaimTypes.Role, role));
    }

    var key = _configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("JWT Key bulunamadı.");

    var issuer = _configuration["Jwt:Issuer"]
        ?? throw new InvalidOperationException("JWT Issuer bulunamadı.");

    var audience = _configuration["Jwt:Audience"]
        ?? throw new InvalidOperationException("JWT Audience bulunamadı.");

    var expireMinutes = int.Parse(
        _configuration["Jwt:ExpireMinutes"] ?? "60");

    var securityKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(key));

    var credentials = new SigningCredentials(
        securityKey,
        SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(expireMinutes),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

}