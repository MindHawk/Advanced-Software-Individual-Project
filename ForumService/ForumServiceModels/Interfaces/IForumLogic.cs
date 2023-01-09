namespace ForumServiceModels.Interfaces;

public interface IForumLogic
{
    public Forum? GetForum(string name);
    public IEnumerable<Forum> GetForums();
    public Forum? AddForum(Forum forum);
    public Forum? UpdateForum(Forum forum);
    public bool DeleteForum(string name, int id);
    public bool AddAccount(Account account);
    public bool DeleteAccount(Account account);
    public int GetAccountIdFromGoogleId(string googleId);
}