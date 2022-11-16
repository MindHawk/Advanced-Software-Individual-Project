namespace AccountServiceModels.Interfaces;

public interface IAccountRepository
{
    public Account? GetAccount(Guid id);
    public IEnumerable<Account> GetAccounts();
    public bool AddAccount(Account account);
    public bool UpdateAccount(Account account);
    public bool DeleteAccount(Guid id);
    public bool AccountExists(string name);
}