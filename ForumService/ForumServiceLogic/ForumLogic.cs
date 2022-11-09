using ForumServiceDAL;
using ForumServiceModels;
using ForumServiceModels.Interfaces;
using Microsoft.Extensions.Logging;

namespace ForumServiceLogic;

public class ForumLogic : IForumLogic
{
    private readonly IForumRepository _repository;
    private readonly ILogger<IForumLogic> _logger;
    public ForumLogic(ILogger<IForumLogic> logger, IForumRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    public Forum? GetForum(int id)
    {
        _logger.Log(LogLevel.Information, "Getting forum with id {id}", id);
        return _repository.GetForum(id);
    }

    public IEnumerable<Forum> GetForums()
    {
        _logger.Log(LogLevel.Information, "Getting all forums");
        return _repository.GetForums();
    }

    public Forum? AddForum(Forum forum)
    {
        if (_repository.ForumExists(forum.Id))
        {
            _logger.Log(LogLevel.Information, "Forum with id {id} already exists", forum.Id);
            return null;
        }
        _logger.Log(LogLevel.Information, "Adding forum {forum}", forum);
        if (_repository.AddForum(forum))
        {
            return _repository.GetForum(forum.Id);
        }
        return null;
    }

    public Forum? UpdateForum(int id, Forum forum)
    {
        _logger.Log(LogLevel.Information, "Updating forum {forum}", forum);
        return _repository.UpdateForum(id, forum) ? _repository.GetForum(id) : null;
    }

    public bool DeleteForum(int id)
    { 
        _logger.Log(LogLevel.Information, "Deleting forum with id {id}", id);
        return _repository.DeleteForum(id);
    }
}