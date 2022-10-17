using ForumServiceDAL;
using ForumServiceModels;
using ForumServiceModels.Interfaces;

namespace ForumServiceLogic;

public class ForumLogic : IForumLogic
{
    private readonly IForumRepository _repository;
    public ForumLogic(IForumRepository repository)
    {
        _repository = repository;
    }
    public Forum GetForum(int id)
    {
        return _repository.GetForum(id);
    }

    public IEnumerable<Forum> GetForums()
    {
        return _repository.GetForums();
    }

    public void AddForum(Forum forum)
    {
        _repository.AddForum(forum);
    }

    public void UpdateForum(Forum forum)
    {
        _repository.UpdateForum(forum);
    }

    public void DeleteForum(int id)
    {
        _repository.DeleteForum(id);
    }
}