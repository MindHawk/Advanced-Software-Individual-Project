namespace AccountServiceModels.Interfaces;

public interface IAccountRepository
{
    public Account? GetAccount(string name);
    public IEnumerable<Account> GetAccounts();
    public bool AddAccount(Account account);
    public bool UpdateAccount(Account account);
    public bool DeleteAccount(string name);
    public bool AccountExists(string name);
}