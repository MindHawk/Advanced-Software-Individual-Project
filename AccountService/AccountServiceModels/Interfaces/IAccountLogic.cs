namespace AccountServiceModels.Interfaces;

public interface IAccountLogic
{
    public Account? GetAccount(int id);
    public IEnumerable<Account> GetAccounts();
    public Account? AddAccount(Account account);
    public Account? UpdateAccount(Account account);
    public bool DeleteAccount(int id);
    public int GetAccountIdFromGoogleId(string googleId);
}