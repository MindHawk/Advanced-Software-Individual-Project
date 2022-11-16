namespace AccountServiceModels.Interfaces;

public interface IAccountLogic
{
    public Account? GetAccount(Guid id);
    public IEnumerable<Account> GetAccounts();
    public Account? AddAccount(Account account);
    public Account? UpdateAccount(Account account);
    public bool DeleteAccount(Guid id);
}