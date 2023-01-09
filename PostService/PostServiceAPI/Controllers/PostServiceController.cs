using PostServiceModels;
using PostServiceModels.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PostServiceAPI.Attributes;

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

    [HttpGet("GetPosts/{forumName}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Post>) )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPosts(string forumName)
    {
        var posts = _postLogic.GetPosts(forumName);
        if (posts == null)
        {
            _logger.LogInformation("Forum {ForumName} not found", forumName);
            return NotFound("Forum does not exist");
        }
        return Ok(posts);
    }
    
    [HttpGet("GetPostWithComments/{postId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof((Post,IEnumerable<Comment>)) )]
    public IActionResult GetPostWithComments(int postId)
    {
        (Post post, List<Comment> comments) result = _postLogic.GetPostWithComments(postId);
        if (result is (null, null))
        {
            _logger.LogInformation("Post {PostId} not found", postId);
            return NotFound("Post not found");
        }
        return Ok(result);
    }
    
    [HttpGet("GetCommentsForPost/{postId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Comment>) )]
    public IActionResult GetCommentsForPost(int postId)
    {
        var comments = _postLogic.GetCommentsForPost(postId);
        if (comments is null)
        {
            _logger.LogInformation("Post {PostId} not found", postId);
            return NotFound("Post not found");
        }
        return Ok(comments);
    }

    [HttpGet("GetPost/{postId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetPost(int postId)
    {
        var post = _postLogic.GetPost(postId);
        if (post is null)
        {
            _logger.Log(LogLevel.Information, "Post with name {Id} not found", postId);
            return NotFound("Post not found");
        }
        return Ok(post);
    }
    
    [ServiceFilter(typeof(AuthorizeGoogleTokenAttribute))]
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

    [ServiceFilter(typeof(AuthorizeGoogleTokenAttribute))]
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
    
    [ServiceFilter(typeof(AuthorizeGoogleTokenAttribute))]
    [HttpDelete("DeletePost/{postId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeletePost(int postId)
    {
        var result = _postLogic.DeletePost(postId);
        if (result is false)
        {
            _logger.Log(LogLevel.Information, "Post with id {Id} attempted to be deleted, but does not exist", postId);
            return NotFound("Post not found");
        }
        return Ok();
    }
    
    [ServiceFilter(typeof(AuthorizeGoogleTokenAttribute))]
    [HttpPost("PostComment")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Comment))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult PostComment(Comment comment)
    {
        var result = _postLogic.AddComment(comment);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Comment with id {Id} attempted to be created, but failed", comment.Id);
            return BadRequest("Unable to create comment");
        }
        return Created($"GetComment/{result.Id}", result);
    }
    
    [ServiceFilter(typeof(AuthorizeGoogleTokenAttribute))]
    [HttpPut("PutComment")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Comment))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PutComment(Comment comment)
    {
        var result = _postLogic.UpdateComment(comment);
        if (result is null)
        {
            _logger.Log(LogLevel.Information, "Comment with id {Id} attempted to be updated, but does not exist", comment.Id);
            return NotFound("Comment does not exist");
        }
        return Ok(result);
    }

    [ServiceFilter(typeof(AuthorizeGoogleTokenAttribute))]
    [HttpDelete("DeleteComment/{postId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteComment(int postId)
    {
        var result = _postLogic.DeleteComment(postId);
        if (result is false)
        {
            _logger.Log(LogLevel.Information, "Comment with id {Id} attempted to be deleted, but does not exist", postId);
            return NotFound("Comment not found");
        }

        return Ok();
    }
    
    private int GetIdFromGoogleId()
    {
        string googleId = HttpContext.Items["GoogleId"] as string ?? "";
        int id = _postLogic.GetAccountIdFromGoogleId(googleId);
        return id;
    }
}