namespace AccountServiceModels.Interfaces;

public interface IAccountLogic
{
    public Account? GetAccount(string name);
    public IEnumerable<Account> GetAccounts();
    public Account? AddAccount(Account account);
    public Account? UpdateAccount(Account account);
    public bool DeleteAccount(string name);
}