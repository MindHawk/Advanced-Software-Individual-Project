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
        return _repository.GetForum(id);
    }

    public IEnumerable<Forum> GetForums()
    {
        return _repository.GetForums();
    }

    public Forum? AddForum(Forum forum)
    {
        return _repository.AddForum(forum);
    }

    public Forum? UpdateForum(Forum forum)
    {
        return _repository.UpdateForum(forum);
    }

    public bool DeleteForum(int id)
    { 
        return _repository.DeleteForum(id);
    }
}