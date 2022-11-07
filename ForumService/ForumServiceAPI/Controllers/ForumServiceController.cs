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

    [HttpGet(Name = "GetForums")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Forum>) )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetForums()
    {
        var forums = _forumLogic.GetForums();
        return Ok(forums);
    }

    [HttpGet(Name = "GetForum")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Forum))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("{id:int}")]
    public IActionResult GetForum(int id)
    {
        var forum = _forumLogic.GetForum(id);
        if (forum is null)
        {
            return NotFound();
        }
        return Ok(forum);
    }
    
    [HttpPost(Name = "PostForum")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Forum))]
    public IActionResult PostForum(Forum forum)
    {
        var result = _forumLogic.AddForum(forum);
        if (result is null)
        {
            return BadRequest();
        }
        return Created($"GetForum/{result.Id}", result);
    }
}