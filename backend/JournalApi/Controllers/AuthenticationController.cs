using JournalApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JournalApi.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] ApiRequest<LoginRequest> request)
    {
        var response = new ApiResponse<LoginResponse>
        {
            Response = new LoginResponse() { AccessToken = "", RefreshToken = "" },
        };

        return Ok(response);
    }
}
