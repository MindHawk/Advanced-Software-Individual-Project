using AccountServiceDAL;
using AccountServiceModels;
using AccountServiceModels.Interfaces;
using Microsoft.Extensions.Logging;

namespace AccountServiceLogic;

public class AccountLogic : IAccountLogic
{
    private readonly IAccountRepository _repository;
    private readonly ILogger<IAccountLogic> _logger;
    public AccountLogic(ILogger<IAccountLogic> logger, IAccountRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    public Account? GetAccount(string name)
    {
        _logger.Log(LogLevel.Information, "Getting Account with name {id}", name);
        return _repository.GetAccount(name);
    }

    public IEnumerable<Account> GetAccounts()
    {
        _logger.Log(LogLevel.Information, "Getting all Accounts");
        return _repository.GetAccounts();
    }

    public Account? AddAccount(Account Account)
    {
        if (_repository.AccountExists(Account.Name))
        {
            _logger.Log(LogLevel.Information, "Account with name {name} already exists", Account.Name);
            return null;
        }
        _logger.Log(LogLevel.Information, "Adding Account {Account}", Account);
        if (_repository.AddAccount(Account))
        {
            return _repository.GetAccount(Account.Name);
        }
        return null;
    }

    public Account? UpdateAccount(Account Account)
    {
        _logger.Log(LogLevel.Information, "Updating Account {Account}", Account);
        return _repository.UpdateAccount(Account) ? _repository.GetAccount(Account.Name) : null;
    }

    public bool DeleteAccount(string name)
    { 
        _logger.Log(LogLevel.Information, "Deleting Account with name {name}", name);
        return _repository.DeleteAccount(name);
    }
}