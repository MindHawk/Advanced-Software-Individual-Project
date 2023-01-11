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
            ForumShared forumShared = new() { Name = forum.Name, AdminId = forum.AdminId};
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

    public bool DeleteForum(string name, int id)
    {
        Forum? forum = _repository.GetForum(name);
        if (forum == null)
        {
            _logger.LogInformation("Attempted to delete forum that does not exist: {Name}", name);
            return false;
        }

        if (forum.AdminId != id)
        {
            _logger.LogInformation("User with id {Id} attempted to delete forum {Name} but is not admin", id, name);
            return false;
        }

        return _repository.DeleteForum(name);
    }

    public bool AddAccount(Account account)
    {
        return _repository.AddAccount(account);
    }

    public bool DeleteAccount(Account account)
    {
        return _repository.DeleteAccount(account);
    }

    public int GetAccountIdFromGoogleId(string googleId)
    {
        return _repository.GetAccountIdFromGoogleId(googleId) ?? -1;
    }
}