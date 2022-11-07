using ForumServiceModels;
using ForumServiceModels.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumServiceDAL;

public class ForumRepository : IForumRepository
{
    private readonly ForumContext _context;
    public ForumRepository(ForumContext context)
    {
        _context = context;
        context.Database.Migrate();
    }
    public Forum? GetForum(int id)
    {
        return _context.Forums.Find(id);
    }

    public IEnumerable<Forum> GetForums()
    {
        return _context.Forums.ToList();
    }

    public Forum? AddForum(Forum forum)
    {
        _context.Forums.Add(forum);
        _context.SaveChanges();
        return GetForum(forum.Id);
    }

    public Forum? UpdateForum(Forum forum)
    {
        _context.Forums.Update(forum);
        _context.SaveChanges();
        return GetForum(forum.Id);
    }
    
    public bool DeleteForum(int id)
    {
        var forum = GetForum(id);
        if (forum != null)
        {
            _context.Forums.Remove(forum);
            _context.SaveChanges();
            return true;
        }
        return false;
    }
}