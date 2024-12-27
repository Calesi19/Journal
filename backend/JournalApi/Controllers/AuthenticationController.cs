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
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userRepository.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return Unauthorized();
        }

        if (!_passwordHasherService.VerifyPasswordMatch(request.Password, user.Password))
        {
            return Unauthorized();
        }

        var token = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(user);

        var response = new LoginResponse { AccessToken = token, RefreshToken = refreshToken };

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh(
        [FromHeader] string Authorization,
        [FromHeader] string RefreshToken
    )
    {
        var userId = _tokenService.GetUserIdFromToken(Authorization);
        var user = await _userRepository.FindByIdAsync(userId);

        if (user == null)
        {
            return Unauthorized();
        }

        if (!_tokenService.VerifyRefreshToken(user, RefreshToken))
        {
            return Unauthorized();
        }

        var token = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(user);

        var response = new LoginResponse { AccessToken = token, RefreshToken = refreshToken };

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("accounts")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        // Check if the user already exists
        var user = await _userRepository.FindByEmailAsync(request.Email);

        if (user != null)
        {
            return BadRequest();
        }

        // Validate the password is strong enough
        if (!_passwordStrengthValidatorService.IsPasswordStrong(request.Password))
        {
            return BadRequest();
        }

        // Hash the password
        var hashedPassword = _passwordHasherService.HashPassword(request.Password);

        var newUser = new User { Email = request.Email, Password = hashedPassword };

        await _userRepository.AddAsync(newUser);

        var bearertoken = _tokenService.GenerateAccessToken(newUser);
        var refreshToken = _tokenService.GenerateRefreshToken(newUser);

        var response = new CreateAccountResponse
        {
            IsSuccess = true,
            Email = newUser.Email,
            AccessToken = bearertoken,
            RefreshToken = refreshToken,
        };

        return Created("", response);
    }

    [HttpDelete("accounts")]
    public async Task<IActionResult> CreateAccount([FromHeader] string Authorization)
    {
        var userId = _tokenService.GetUserIdFromToken(Authorization);

        var user = await _userRepository.FindByIdAsync(userId);

        if (user == null)
        {
            return BadRequest();
        }

        await _userRepository.DeleteAsync(userId);

        var response = new ActionStatusResponse { IsSuccess = true };

        return Created("", response);
    }
}
