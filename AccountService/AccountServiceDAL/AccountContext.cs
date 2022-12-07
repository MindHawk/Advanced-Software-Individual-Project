using AccountServiceModels;
using Microsoft.EntityFrameworkCore;

namespace AccountServiceDAL;

public class AccountContext : DbContext
{
    public AccountContext(DbContextOptions<AccountContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
}