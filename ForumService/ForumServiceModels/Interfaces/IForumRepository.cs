namespace ForumServiceModels.Interfaces;

public interface IForumRepository
{
    public Forum getForum(int id);
    public IEnumerable<Forum> getForums();
    public void addForum(Forum forum);
    public void updateForum(Forum forum);
    public void deleteForum(int id);
}