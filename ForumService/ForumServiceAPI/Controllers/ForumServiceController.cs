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

    [HttpGet("GetForums")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Forum>) )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetForums()
    {
        var forums = _forumLogic.GetForums();
        return Ok(forums);
    }

    [HttpGet("GetForum/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Forum))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetForum(int id)
    {
        var forum = _forumLogic.GetForum(id);
        if (forum is null)
        {
            _logger.Log(LogLevel.Information, "Forum with id {id} not found", id);
            return NotFound();
        }
        return Ok(forum);
    }
    
    [HttpPost("PostForum")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Forum))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostForum(Forum forum)
    {
        var result = _forumLogic.AddForum(forum);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Forum with id {id} attempted to be created, but already exists", forum.Id);
            return BadRequest();
        }
        return Created($"GetForum/{result.Id}", result);
    }

    [HttpPut("PutForum/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Forum))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutForum(int id, Forum forum)
    {
        var result = _forumLogic.UpdateForum(id, forum);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Forum with id {id} attempted to be updated, but does not exist", forum.Id);
            return NotFound();
        }
        return Ok(result);
    }
    
    [HttpDelete("DeleteForum")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteForum(int id)
    {
        var result = _forumLogic.DeleteForum(id);
        if (result is false)
        {
            _logger.Log(LogLevel.Information, "Forum with id {id} attempted to be deleted, but does not exist", id);
            return NotFound();
        }
        return Ok();
    }
}