using ForumServiceDAL;
using ForumServiceMessageBusProducer;
using ForumServiceModels;
using ForumServiceModels.Interfaces;
using Microsoft.Extensions.Logging;

namespace ForumServiceLogic;

public class ForumLogic : IForumLogic
{
    private readonly IForumRepository _repository;
    private readonly ILogger<IForumLogic> _logger;
    private readonly ForumMessageBusProducer _producer;
    public ForumLogic(ILogger<ForumLogic> logger, IForumRepository repository, ForumMessageBusProducer producer)
    {
        _logger = logger;
        _repository = repository;
        _producer = producer;
    }
    public Forum? GetForum(string name)
    {
        _logger.Log(LogLevel.Information, "Getting forum with name {Name}", name);
        return _repository.GetForum(name);
    }

    public IEnumerable<Forum> GetForums()
    {
        _logger.Log(LogLevel.Information, "Getting all forums");
        return _repository.GetForums();
    }

    public Forum? AddForum(Forum forum)
    {
        if (_repository.ForumExists(forum.Name))
        {
            _logger.Log(LogLevel.Information, "Forum with name {Name} already exists", forum.Name);
            return null;
        }
        _logger.Log(LogLevel.Information, "Adding forum {Forum}", forum.Name);
        if (_repository.AddForum(forum))
        {
            ForumShared forumShared = new ForumShared{ Name = forum.Name, };
            _producer.SendForumCreatedMessage(forumShared);
            return _repository.GetForum(forum.Name);
        }
        return null;
    }

    public Forum? UpdateForum(Forum forum)
    {
        _logger.Log(LogLevel.Information, "Updating forum {Forum}", forum.Name);
        return _repository.UpdateForum(forum) ? _repository.GetForum(forum.Name) : null;
    }

    public bool DeleteForum(string name)
    { 
        _logger.Log(LogLevel.Information, "Deleting forum with name {Name}", name);
        return _repository.DeleteForum(name);
    }
}