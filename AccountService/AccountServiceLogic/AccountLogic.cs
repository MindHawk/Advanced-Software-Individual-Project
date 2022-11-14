using ForumServiceDAL;
using ForumServiceModels;
using ForumServiceModels.Interfaces;
using Microsoft.Extensions.Logging;

namespace ForumServiceLogic;

public class AccountLogic : IAccountLogic
{
    private readonly IAccountRepository _repository;
    private readonly ILogger<IAccountLogic> _logger;
    public AccountLogic(ILogger<IAccountLogic> logger, IAccountRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    public Forum? GetForum(string name)
    {
        _logger.Log(LogLevel.Information, "Getting forum with name {id}", name);
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
            _logger.Log(LogLevel.Information, "Forum with name {name} already exists", forum.Name);
            return null;
        }
        _logger.Log(LogLevel.Information, "Adding forum {forum}", forum);
        if (_repository.AddForum(forum))
        {
            return _repository.GetForum(forum.Name);
        }
        return null;
    }

    public Forum? UpdateForum(Forum forum)
    {
        _logger.Log(LogLevel.Information, "Updating forum {forum}", forum);
        return _repository.UpdateForum(forum) ? _repository.GetForum(forum.Name) : null;
    }

    public bool DeleteForum(string name)
    { 
        _logger.Log(LogLevel.Information, "Deleting forum with name {name}", name);
        return _repository.DeleteForum(name);
    }
}