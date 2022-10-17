using ForumServiceModels;
using ForumServiceModels.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForumServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ForumServiceController : ControllerBase
{
    private readonly ILogger<ForumServiceController> _logger;
    private readonly IForumLogic _forumLogic;

    public ForumServiceController(ILogger<ForumServiceController> logger, IForumLogic forumLogic)
    {
        _logger = logger;
        _forumLogic = forumLogic;
    }

    [HttpPost(Name = "PostForum")]
    public async Task<ActionResult<Forum>> Post(Forum forum)
    {
        _forumLogic.AddForum(forum);

        return Created("GetRoom", forum);
    }
    
    [HttpGet(Name = "GetForums")]
    public async Task<IEnumerable<Forum>> Get()
    {
        return _forumLogic.GetForums();
    }

}