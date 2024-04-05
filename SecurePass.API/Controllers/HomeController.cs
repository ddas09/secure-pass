using Microsoft.AspNetCore.Mvc;

namespace SecurePass.API.Controllers;

/// <summary>
/// Controller responsible for testing APIs.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    /// <summary>
    /// Ping endpoint to check if the SecurePass API is live.
    /// </summary>
    /// <returns>A string indicating that the SecurePass API is live.</returns>
    [HttpGet("[action]")]
    public IActionResult Ping()
    {
        return Ok("Secure pass api is live...");
    }
}
