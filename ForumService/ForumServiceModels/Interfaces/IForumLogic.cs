namespace ForumServiceModels.Interfaces;

public interface IForumLogic
{
    public Forum? GetForum(int id);
    public IEnumerable<Forum> GetForums();
    public Forum? AddForum(Forum forum);
    public Forum? UpdateForum(Forum forum);
    public bool DeleteForum(int id);
}