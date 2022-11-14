namespace AccountServiceModels.Interfaces;

public interface IAccountRepository
{
    public Account? GetForum(string name);
    public IEnumerable<Account> GetForums();
    public bool AddForum(Account account);
    public bool UpdateForum(Account account);
    public bool DeleteForum(string name);
    public bool ForumExists(string name);
}