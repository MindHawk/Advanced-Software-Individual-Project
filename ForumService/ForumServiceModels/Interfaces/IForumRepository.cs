namespace ForumServiceModels.Interfaces;

public interface IForumRepository
{
    public Forum? GetForum(string name);
    public IEnumerable<Forum> GetForums();
    public bool AddForum(Forum forum);
    public bool UpdateForum(Forum forum);
    public bool DeleteForum(string name);
    public bool ForumExists(string name);
}