using PostServiceModels;
using Microsoft.EntityFrameworkCore;
using SharedDTOs;

namespace PostServiceDAL;

public class PostContext : DbContext
{
    public PostContext(DbContextOptions<PostContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<ForumShared> Forums { get; set; }
}