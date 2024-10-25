using JournalApi.DTOs;
using JournalApi.Models;
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
    private readonly IPasswordStrengthValidatorService _passwordStrengthValidatorService;

    public AuthenticationController(
        ITokenService tokenService,
        IUserRepository userRepository,
        IPasswordHasherService passwordHasherService,
        IPasswordStrengthValidatorService passwordStrengthValidatorService
    )
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _passwordHasherService = passwordHasherService;
        _passwordStrengthValidatorService = passwordStrengthValidatorService;
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

    [AllowAnonymous]
    [HttpPost("accounts")]
    public async Task<IActionResult> CreateAccount(
        [FromBody] ApiRequest<CreateAccountRequest> request
    )
    {
        // Check if the user already exists
        var user = await _userRepository.FindByEmailAsync(request.Request.Email);

        if (user != null)
        {
            return BadRequest();
        }

        // Validate the password is strong enough
        if (!_passwordStrengthValidatorService.IsPasswordStrong(request.Request.Password))
        {
            return BadRequest();
        }

        // Hash the password
        var hashedPassword = _passwordHasherService.HashPassword(request.Request.Password);

        var newUser = new User { Email = request.Request.Email, Password = hashedPassword };

        await _userRepository.AddAsync(newUser);

        var response = new ApiResponse<CreateAccountResponse>
        {
            Response = new CreateAccountResponse() { IsSuccess = true, Email = newUser.Email },
        };

        return Created("", response);
    }
}
