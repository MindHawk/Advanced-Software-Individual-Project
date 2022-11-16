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
    public Account? GetAccount(int id)
    {
        _logger.Log(LogLevel.Information, "Getting Account with id {id}", id);
        return _repository.GetAccount(id);
    }

    public IEnumerable<Account> GetAccounts()
    {
        _logger.Log(LogLevel.Information, "Getting all Accounts");
        return _repository.GetAccounts();
    }

    public Account? AddAccount(Account account)
    {
        if (_repository.AccountExists(account.Name))
        {
            _logger.Log(LogLevel.Information, "Account with name {name} already exists", account.Name);
            return null;
        }
        _logger.Log(LogLevel.Information, "Adding Account {Account}", account);
        if (_repository.AddAccount(account))
        {
            return _repository.GetAccount(account.Id);
        }
        return null;
    }

    public Account? UpdateAccount(Account account)
    {
        _logger.Log(LogLevel.Information, "Updating Account {Account}", account);
        return _repository.UpdateAccount(account) ? _repository.GetAccount(account.Id) : null;
    }

    public bool DeleteAccount(int id)
    {
        _logger.Log(LogLevel.Information, "Deleting Account with id {id}", id);
        return _repository.DeleteAccount(id);
    }
}