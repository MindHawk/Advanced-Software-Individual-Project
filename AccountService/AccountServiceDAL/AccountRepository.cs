using AccountServiceModels;
using AccountServiceModels.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountServiceDAL;

public class AccountRepository : IAccountRepository
{
    private readonly AccountContext _context;
    public AccountRepository(AccountContext context)
    {
        _context = context;
        _context.Database.Migrate();
    }
    public Account? GetAccount(Guid id)
    {
        return _context.Accounts.Find(id);
    }

    public IEnumerable<Account> GetAccounts()
    {
        return _context.Accounts.ToList();
    }

    public bool AddAccount(Account account)
    {
        account.id = Guid.NewGuid();
        _context.Accounts.Add(account);
        return _context.SaveChanges() > 0;
    }

    public bool UpdateAccount(Account account)
    {
        _context.Accounts.Update(account);
        return _context.SaveChanges() > 0;
    }
    
    public bool DeleteAccount(Guid id)
    {
        Account? account = GetAccount(id);
        if (account == null) return false;
        _context.Accounts.Remove(account);
        return _context.SaveChanges() > 0;
    }
    
    public bool AccountExists(string name)
    {
        return _context.Accounts.Any(e => e.Name == name);
    }
}