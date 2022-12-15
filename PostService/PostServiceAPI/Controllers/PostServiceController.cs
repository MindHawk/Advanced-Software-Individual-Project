using PostServiceModels;
using PostServiceModels.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PostServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostServiceController : ControllerBase
{
    private readonly ILogger<PostServiceController> _logger;
    private readonly IPostLogic _PostLogic;

    public PostServiceController(ILogger<PostServiceController> logger, IPostLogic PostLogic)
    {
        _logger = logger;
        _PostLogic = PostLogic;
    }

    [HttpGet("GetPosts")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Post>) )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPosts()
    {
        var Posts = _PostLogic.GetPosts();
        return Ok(Posts);
    }

    [HttpGet("GetPost/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPost(string name)
    {
        var Post = _PostLogic.GetPost(name);
        if (Post is null)
        {
            _logger.Log(LogLevel.Information, "Post with name {name} not found", name);
            return NotFound();
        }
        return Ok(Post);
    }
    
    [HttpPost("PostPost")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostPost(Post Post)
    {
        var result = _PostLogic.AddPost(Post);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Post with id {name} attempted to be created, but already exists", Post.Name);
            return BadRequest();
        }
        return Created($"GetPost/{result.Name}", result);
    }

    [HttpPut("PutPost")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutPost(Post Post)
    {
        var result = _PostLogic.UpdatePost(Post);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Post with id {name} attempted to be updated, but does not exist", Post.Name);
            return NotFound();
        }
        return Ok(result);
    }
    
    [HttpDelete("DeletePost/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeletePost(string name)
    {
        var result = _PostLogic.DeletePost(name);
        if (result is false)
        {
            _logger.Log(LogLevel.Information, "Post with name {name} attempted to be deleted, but does not exist", name);
            return NotFound();
        }
        return Ok();
    }
}