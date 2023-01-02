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

    public bool AddComment(Comment comment)
    {
        _context.Comments.Add(comment);
        return _context.SaveChanges() > 0;
    }

    public bool UpdateComment(Comment comment)
    {
        _context.Comments.Update(comment);
        return _context.SaveChanges() > 0;
    }

    public bool DeleteComment(int id)
    {
        Comment? comment = _context.Comments.Find(id);
        if (comment == null) return false;
        _context.Comments.Remove(comment);
        return _context.SaveChanges() > 0;
    }

    public bool CommentExists(int id)
    {
        return _context.Comments.Any(e => e.Id == id);
    }

    public Comment? GetComment(int id)
    {
        return _context.Comments.Find(id);
    }

    public bool ForumExists(string name)
    {
        return _context.Forums.Any(e => e.Name == name);
    }

    public bool AddForum(Forum forum)
    {
        _context.Forums.Add(forum);
        return _context.SaveChanges() > 0;
    }

    public bool DeleteForum(Forum forum)
    {
        forum.Deleted = true;
        _context.Forums.Update(forum);
        return _context.SaveChanges() > 0;
    }
}