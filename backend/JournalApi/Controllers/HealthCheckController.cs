using Microsoft.AspNetCore.Mvc;

namespace JournalApi.Controllers;

[ApiController]
public class HealthController : ControllerBase {
  [HttpGet("health")]
  public async Task<IActionResult> HealthCheck() { return Ok("I'm okay. :)"); }
}
