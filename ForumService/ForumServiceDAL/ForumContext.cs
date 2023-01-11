using ForumServiceModels;
using Microsoft.EntityFrameworkCore;

namespace ForumServiceDAL;

public class ForumContext : DbContext
{
    public ForumContext(DbContextOptions<ForumContext> options) : base(options)
    {
    }

    public DbSet<Forum> Forums { get; set; }
    public DbSet<Account> Accounts { get; set; }
}