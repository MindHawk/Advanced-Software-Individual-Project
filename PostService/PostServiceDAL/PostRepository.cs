using System.ComponentModel.Design;
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
    }
    public Post? GetPost(int id)
    {
        return _context.Posts.Find(id);
    }

    public IEnumerable<Post>? GetPosts(string forumName)
    {
        if (ForumExists(forumName))
        {
            return _context.Posts.Where(p => p.Forum == forumName && p.Deleted == false).ToList();
        }
        return null;
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
        post.Deleted = true;
        post.Content = "This post has been deleted";
        post.Author = -1;
        return UpdatePost(post);
    }
    
    public bool PostExists(int id)
    {
        return _context.Posts.Any(e => e.Id == id && e.Deleted == false);
    }

    public List<Comment> GetCommentsForPost(int postId)
    {
        return _context.Comments.Where(c => c.PostId == postId && c.Deleted == false).ToList();
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
        comment.Deleted = true;
        comment.Content = "This comment has been deleted";
        comment.Author = -1;
        return UpdateComment(comment);
    }

    public bool CommentExists(int id)
    {
        return _context.Comments.Any(e => e.Id == id && e.Deleted == false);
    }

    public Comment? GetComment(int id)
    {
        return _context.Comments.Find(id);
    }

    public bool ForumExists(string name)
    {
        return _context.Forums.Any(e => e.Name == name && e.Deleted == false);
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

    public bool AddAccount(Account account)
    {
        _context.Accounts.Add(account);
        return _context.SaveChanges() > 0;
    }
}