using PostServiceModels;
using PostServiceModels.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PostServiceDAL;

public class PostRepository : IPostRepository
{
    private readonly PostContext _context;
    public PostRepository(PostContext context)
    {
        _context = context;
        if (Environment.GetEnvironmentVariable("HOSTED_ENVIRONMENT") == "docker")
        {
            _context.Database.Migrate();
        }
    }
    public Post? GetPost(string name)
    {
        return _context.Posts.Find(name);
    }

    public IEnumerable<Post> GetPosts()
    {
        return _context.Posts.ToList();
    }

    public bool AddPost(Post Post)
    {
        _context.Posts.Add(Post);
        return _context.SaveChanges() > 0;
    }

    public bool UpdatePost(Post Post)
    {
        _context.Posts.Update(Post);
        return _context.SaveChanges() > 0;
    }
    
    public bool DeletePost(string name)
    {
        Post? Post = GetPost(name);
        if (Post == null) return false;
        _context.Posts.Remove(Post);
        return _context.SaveChanges() > 0;
    }
    
    public bool PostExists(string name)
    {
        return _context.Posts.Any(e => e.Name == name);
    }
}