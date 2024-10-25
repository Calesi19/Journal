using JournalApi.DTOs;
using JournalApi.Repositories;
using JournalApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JournalApi.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasherService _passwordHasherService;

    public AuthenticationController(
        ITokenService tokenService,
        IUserRepository userRepository,
        IPasswordHasherService passwordHasherService
    )
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _passwordHasherService = passwordHasherService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] ApiRequest<LoginRequest> request)
    {
        var user = await _userRepository.FindByEmailAsync(request.Request.Email);

        if (user == null)
        {
            return Unauthorized();
        }

        if (!_passwordHasherService.VerifyPasswordMatch(request.Request.Password, user.Password))
        {
            return Unauthorized();
        }

        var token = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var response = new ApiResponse<LoginResponse>
        {
            Response = new LoginResponse() { AccessToken = token, RefreshToken = refreshToken },
        };

        return Ok(response);
    }
}
