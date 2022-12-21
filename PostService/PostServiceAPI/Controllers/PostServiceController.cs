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
    
    public IActionResult GetPostWithComments(int postId)
    {
        (Post post, List<Comment> comments) result = _postLogic.GetPostWithComments(postId);
        return Ok(result);
    }
    
    public IActionResult GetCommentsForPost(int postId)
    {
        var comments = _postLogic.GetCommentsForPost(postId);
        return Ok(comments);
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
            return NotFound("Post not found");
        }
        return Ok(post);
    }
    
    [HttpPost("PostPost")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostPost(Post post)
    {
        var result = _postLogic.AddPost(post);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Post with id {Id} attempted to be created, but already exists", post.Id);
            return BadRequest("Post already exists");
        }
        return Created($"GetPost/{result.Id}", result);
    }

    [HttpPut("PutPost")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutPost(Post post)
    {
        var result = _postLogic.UpdatePost(post);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Post with id {Id} attempted to be updated, but does not exist", post.Id);
            return NotFound("Post does not exist");
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
            return NotFound("Post not found");
        }
        return Ok();
    }
}