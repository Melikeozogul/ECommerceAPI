using ECommerceAPI.Application.DTOs.Auth;

namespace ECommerceAPI.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
}