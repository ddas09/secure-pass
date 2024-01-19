using Microsoft.AspNetCore.Mvc;

namespace SecurePass.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    public HomeController() { }

    [HttpGet("[action]")]
    public IActionResult Ping()
    {
        return Ok("Secure pass api is live...");
    }
}
