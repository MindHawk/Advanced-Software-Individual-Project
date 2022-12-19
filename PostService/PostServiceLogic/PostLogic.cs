using PostServiceDAL;
using PostServiceModels;
using PostServiceModels.Interfaces;
using Microsoft.Extensions.Logging;

namespace PostServiceLogic;

public class PostLogic : IPostLogic
{
    private readonly IPostRepository _repository;
    private readonly ILogger<IPostLogic> _logger;
    public PostLogic(ILogger<IPostLogic> logger, IPostRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    public Post? GetPost(int id)
    {
        _logger.Log(LogLevel.Information, "Getting Post with name {id}", id);
        return _repository.GetPost(id);
    }

    public IEnumerable<Post> GetPosts()
    {
        _logger.Log(LogLevel.Information, "Getting all Posts");
        return _repository.GetPosts();
    }

    public Post? AddPost(Post post)
    {
        if (_repository.PostExists(post.Id))
        {
            _logger.Log(LogLevel.Information, "Post with id {Id} already exists", post.Id);
            return null;
        }
        _logger.Log(LogLevel.Information, "Adding post with id {Id}", post.Id);
        if (_repository.AddPost(post))
        {
            return _repository.GetPost(post.Id);
        }
        return null;
    }

    public Post? UpdatePost(Post post)
    {
        _logger.Log(LogLevel.Information, "Updating post with id {Id}", post.Id);
        return _repository.UpdatePost(post) ? _repository.GetPost(post.Id) : null;
    }

    public bool DeletePost(int id)
    { 
        _logger.Log(LogLevel.Information, "Deleting post with id {Id}", id);
        return _repository.DeletePost(id);
    }
}