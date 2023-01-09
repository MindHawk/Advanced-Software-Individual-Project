using Microsoft.Extensions.Logging;
using PostServiceModels;
using PostServiceModels.Interfaces;

namespace PostServiceLogic;

public class PostMessageBusLogic : IPostMessageBusLogic
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<PostMessageBusLogic> _logger;
    
    public PostMessageBusLogic(IPostRepository postRepository, ILogger<PostMessageBusLogic> logger)
    {
        _postRepository = postRepository;
        _logger = logger;
    }
    
    public bool AddForum(Forum forum)
    {
        _logger.LogInformation("Adding forum to database from message bus");
        return _postRepository.AddForum(forum);
    }

    public bool DeleteForum(Forum forum)
    {
        _logger.LogInformation("Deleting forum from database from message bus");
        return _postRepository.DeleteForum(forum);
    }
    
    public bool AddAccount(Account account)
    {
        _logger.LogInformation("Adding account to database from message bus");
        return _postRepository.AddAccount(account);
    }
}