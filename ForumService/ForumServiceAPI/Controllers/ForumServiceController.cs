using ForumServiceAPI.Attributes;
using ForumServiceModels;
using ForumServiceModels.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    
    [HttpGet("GetForum/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Forum))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetForum(string name)
    {
        var forum = _forumLogic.GetForum(name);
        if (forum is null)
        {
            _logger.Log(LogLevel.Information, "Forum with name {Name} not found", name);
            return NotFound();
        }
        return Ok(forum);
    }
    
    [ServiceFilter(typeof(AuthorizeGoogleTokenAttribute))]
    [HttpPost("PostForum")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Forum))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostForum(Forum forum)
    {
        int id = GetIdFromGoogleId();
        if(id == -1)
        {
            _logger.Log(LogLevel.Error, "User id not found in token");
            return BadRequest();
        }

        forum.AdminId = id;
        var result = _forumLogic.AddForum(forum);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Forum with name {Name} attempted to be created, but already exists", forum.Name);
            return BadRequest();
        }
        return Created($"GetForum/{result.Name}", result);
    }

    [ServiceFilter(typeof(AuthorizeGoogleTokenAttribute))]
    [HttpPut("PutForum")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Forum))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutForum(Forum forum)
    {
        int id = GetIdFromGoogleId();
        if(id == -1)
        {
            _logger.Log(LogLevel.Error, "User id not found in token");
            return BadRequest();
        }

        var result = _forumLogic.UpdateForum(forum);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Forum with name {Name} attempted to be updated, but does not exist", forum.Name);
            return NotFound();
        }
        return Ok(result);
    }
    
    [ServiceFilter(typeof(AuthorizeGoogleTokenAttribute))]
    [HttpDelete("DeleteForum/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteForum(string name)
    {
        int id = GetIdFromGoogleId();
        if(id == -1)
        {
            _logger.Log(LogLevel.Error, "User id not found in token");
            return BadRequest();
        }

        var result = _forumLogic.DeleteForum(name, id);
        if (result is false)
        {
            _logger.Log(LogLevel.Information, "Forum with name {Name} attempted to be deleted, but does not exist", name);
            return NotFound();
        }
        return Ok();
    }
    
    private int GetIdFromGoogleId()
    {
        string googleId = HttpContext.Items["GoogleId"] as string ?? "";
        int id = _forumLogic.GetAccountIdFromGoogleId(googleId);
        return id;
    }
}