using ECommerceAPI.Application.DTOs.Auth;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(
        RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(
        LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);

        if (result is null)
            return Unauthorized("Email veya şifre hatalı.");

        return Ok(result);
    }
}