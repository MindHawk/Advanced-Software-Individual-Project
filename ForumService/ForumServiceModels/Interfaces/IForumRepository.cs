namespace ForumServiceModels.Interfaces;

public interface IForumRepository
{
    public Forum? GetForum(int id);
    public IEnumerable<Forum> GetForums();
    public bool AddForum(Forum forum);
    public bool UpdateForum(int id, Forum forum);
    public bool DeleteForum(int id);
    public bool ForumExists(int id);
}