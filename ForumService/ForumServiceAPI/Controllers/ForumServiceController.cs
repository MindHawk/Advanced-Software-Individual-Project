using Microsoft.AspNetCore.Mvc;

namespace ForumServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ForumServiceController : ControllerBase
{
    private readonly ILogger<ForumServiceController> _logger;

    public ForumServiceController(ILogger<ForumServiceController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Get")]
    public IActionResult Get()
    {
        return Ok();
    }
}