namespace AccountServiceModels.Interfaces;

public interface IAccountRepository
{
    public Account? GetAccount(int id);
    public IEnumerable<Account> GetAccounts();
    public bool AddAccount(Account account);
    public bool UpdateAccount(Account account);
    public bool DeleteAccount(int id);
    public bool AccountExists(string name);
    public int? GetAccountIdFromGoogleId(string googleId);
}