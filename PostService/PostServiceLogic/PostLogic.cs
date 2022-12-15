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
    public Post? GetPost(string name)
    {
        _logger.Log(LogLevel.Information, "Getting Post with name {id}", name);
        return _repository.GetPost(name);
    }

    public IEnumerable<Post> GetPosts()
    {
        _logger.Log(LogLevel.Information, "Getting all Posts");
        return _repository.GetPosts();
    }

    public Post? AddPost(Post Post)
    {
        if (_repository.PostExists(Post.Name))
        {
            _logger.Log(LogLevel.Information, "Post with name {name} already exists", Post.Name);
            return null;
        }
        _logger.Log(LogLevel.Information, "Adding Post {Post}", Post);
        if (_repository.AddPost(Post))
        {
            return _repository.GetPost(Post.Name);
        }
        return null;
    }

    public Post? UpdatePost(Post Post)
    {
        _logger.Log(LogLevel.Information, "Updating Post {Post}", Post);
        return _repository.UpdatePost(Post) ? _repository.GetPost(Post.Name) : null;
    }

    public bool DeletePost(string name)
    { 
        _logger.Log(LogLevel.Information, "Deleting Post with name {name}", name);
        return _repository.DeletePost(name);
    }
}