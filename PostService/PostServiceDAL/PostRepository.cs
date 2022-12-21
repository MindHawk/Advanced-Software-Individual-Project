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
    public Post? GetPost(int id)
    {
        return _context.Posts.Find(id);
    }

    public IEnumerable<Post> GetPosts()
    {
        return _context.Posts.ToList();
    }

    public bool AddPost(Post post)
    {
        _context.Posts.Add(post);
        return _context.SaveChanges() > 0;
    }

    public bool UpdatePost(Post post)
    {
        _context.Posts.Update(post);
        return _context.SaveChanges() > 0;
    }
    
    public bool DeletePost(int id)
    {
        Post? post = GetPost(id);
        if (post == null) return false;
        _context.Posts.Remove(post);
        return _context.SaveChanges() > 0;
    }
    
    public bool PostExists(int id)
    {
        return _context.Posts.Any(e => e.Id == id);
    }

    public List<Comment> GetCommentsForPost(int postId)
    {
        return _context.Comments.Where(c => c.PostId == postId).ToList();
    }
}