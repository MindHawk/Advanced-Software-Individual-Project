using PostServiceModels;
using PostServiceModels.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PostServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostServiceController : ControllerBase
{
    private readonly ILogger<PostServiceController> _logger;
    private readonly IPostLogic _postLogic;

    public PostServiceController(ILogger<PostServiceController> logger, IPostLogic postLogic)
    {
        _logger = logger;
        _postLogic = postLogic;
    }

    [HttpGet("GetPosts")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Post>) )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPosts()
    {
        var posts = _postLogic.GetPosts();
        return Ok(posts);
    }

    [HttpGet("GetPost/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPost(int id)
    {
        var post = _postLogic.GetPost(id);
        if (post is null)
        {
            _logger.Log(LogLevel.Information, "Post with name {Id} not found", id);
            return NotFound();
        }
        return Ok(post);
    }
    
    [HttpPost("PostPost")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostPost(Post Post)
    {
        var result = _postLogic.AddPost(Post);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Post with id {Id} attempted to be created, but already exists", Post.Id);
            return BadRequest();
        }
        return Created($"GetPost/{result.Id}", result);
    }

    [HttpPut("PutPost")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutPost(Post Post)
    {
        var result = _postLogic.UpdatePost(Post);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Post with id {Id} attempted to be updated, but does not exist", Post.Id);
            return NotFound();
        }
        return Ok(result);
    }
    
    [HttpDelete("DeletePost/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeletePost(int id)
    {
        var result = _postLogic.DeletePost(id);
        if (result is false)
        {
            _logger.Log(LogLevel.Information, "Post with id {Id} attempted to be deleted, but does not exist", id);
            return NotFound();
        }
        return Ok();
    }
}