using AccountServiceLogic;
using AccountServiceMessageBusProducer;
using AccountServiceModels;
using AccountServiceModels.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace AccountServiceTest;

public class LogicTest
{
    private readonly Mock<IAccountRepository> _mockRepo;
    private readonly Mock<AccountMessageBusProducer> _mockMessageBus;
    private readonly AccountLogic _logic;
    private readonly Account _defaultAccount;
    private readonly Account _secondAccount;

    public LogicTest()
    {
        Mock<ILogger<AccountLogic>> mockServiceLogger = new();
        _mockRepo = new Mock<IAccountRepository>();
        _mockMessageBus = new Mock<AccountMessageBusProducer>();
        _logic = new AccountLogic(mockServiceLogger.Object, _mockRepo.Object, _mockMessageBus.Object);

        _defaultAccount = new Account{Name = "Test", Id = 0};
        _mockRepo.Setup(repo => repo.GetAccount(_defaultAccount.Id)).Returns(_defaultAccount);
        // This Account is not in the repository by default
        _secondAccount = new Account{Name = "Test2", Id = 1};
    }

    [Fact]
    public void GetAccount_ExistingAccount_ReturnsAccount()
    {
        var returnedValue = _logic.GetAccount(_defaultAccount.Id);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(_defaultAccount, returnedValue);
    }

    [Fact]
    public void GetAccount_NonExistentAccount_ReturnsNull()
    {
        var returnedValue = _logic.GetAccount(_secondAccount.Id);
        
        Assert.Null(returnedValue);
    }

    [Fact]
    public void GetAccounts_ExistingAccounts_ReturnsAccounts()
    {
        List<Account> accountList = new List<Account> { _defaultAccount, _secondAccount };
        _mockRepo.Setup(repo => repo.GetAccounts()).Returns(accountList);
        
        var returnedValue = _logic.GetAccounts();
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(accountList, returnedValue);
    }
    
    [Fact]
    public void GetAccounts_NoExistingAccount_ReturnsEmptyAccounts()
    {
        List<Account> accountList = new List<Account>();
        _mockRepo.Setup(repo => repo.GetAccounts()).Returns(accountList);
        
        var returnedValue = _logic.GetAccounts();
        
        Assert.NotNull(returnedValue);
        Assert.Empty(returnedValue);
    }
    
    [Fact]
    public void PostAccount_NewAccount_ReturnsAccount()
    {
        _mockRepo.Setup(repo => repo.AddAccount(_secondAccount)).Returns(true);
        _mockRepo.Setup(repo => repo.GetAccount(_secondAccount.Id)).Returns(_secondAccount);
        
        var returnedValue = _logic.AddAccount(_secondAccount);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(_secondAccount, returnedValue);
    }
    
    [Fact]
    public void PostAccount_ExistingAccount_ReturnsNull()
    {
        var returnedValue = _logic.AddAccount(_defaultAccount);
        
        Assert.Null(returnedValue);
    }
    
    [Fact]
    public void PutAccount_ExistingAccount_ReturnsNewAccount()
    {
        string existingAccountName = _defaultAccount.Name;
        _secondAccount.Name = existingAccountName;
        _mockRepo.Setup(repo => repo.UpdateAccount(_secondAccount)).Returns(true);
        _mockRepo.Setup(repo => repo.GetAccount(_secondAccount.Id)).Returns(_secondAccount);
        
        var returnedValue = _logic.UpdateAccount(_secondAccount);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(existingAccountName, returnedValue.Name);
        Assert.Equivalent(_secondAccount.Name, returnedValue.Name);
    }
    
    [Fact]
    public void PutAccount_NonExistentAccount_ReturnsNull()
    {
        var returnedValue = _logic.UpdateAccount(_secondAccount);
        
        Assert.Null(returnedValue);
    }
}