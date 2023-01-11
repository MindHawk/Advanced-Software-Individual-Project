using PostServiceModels;
using Microsoft.EntityFrameworkCore;

namespace PostServiceDAL;

public class PostContext : DbContext
{
    public PostContext(DbContextOptions<PostContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Forum> Forums { get; set; }
    public DbSet<Account> Accounts { get; set; }
}