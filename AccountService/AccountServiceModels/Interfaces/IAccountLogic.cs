namespace AccountServiceModels.Interfaces;

public interface IAccountLogic
{
    public Account? GetForum(string name);
    public IEnumerable<Account> GetForums();
    public Account? AddForum(Account account);
    public Account? UpdateForum(Account account);
    public bool DeleteForum(string name);
}