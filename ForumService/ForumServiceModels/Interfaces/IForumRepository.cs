namespace ForumServiceModels.Interfaces;

public interface IForumRepository
{
    public Forum? GetForum(string name);
    public IEnumerable<Forum> GetForums();
    public bool AddForum(Forum forum);
    public bool UpdateForum(Forum forum);
    public bool DeleteForum(string name);
    /// <summary>
    /// This method checks if a forum with the given name exists, even if it is deleted.
    /// This method should not be returnable through the API.
    /// </summary>
    public bool ForumExists(string name);
    public bool AddAccount(Account account);
}