using Microsoft.AspNetCore.Mvc;

namespace JobsityChallenge.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController() : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }
}
