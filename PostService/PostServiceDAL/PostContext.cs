using PostServiceModels;
using Microsoft.EntityFrameworkCore;

namespace PostServiceDAL;

public class PostContext : DbContext
{
    public PostContext(DbContextOptions<PostContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }
}