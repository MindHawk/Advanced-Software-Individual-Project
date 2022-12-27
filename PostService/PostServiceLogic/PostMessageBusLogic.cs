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
    
    public bool AddForum(ForumShared forumShared)
    {
        _logger.LogInformation("Adding forum to database");
        return _postRepository.AddForum(forumShared);
    }
}