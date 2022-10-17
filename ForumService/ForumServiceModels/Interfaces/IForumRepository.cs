namespace ForumServiceModels.Interfaces;

public interface IForumRepository
{
    public Forum GetForum(int id);
    public IEnumerable<Forum> GetForums();
    public void AddForum(Forum forum);
    public void UpdateForum(Forum forum);
    public void DeleteForum(int id);
}