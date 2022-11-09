using ForumServiceModels;
using ForumServiceModels.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

    public bool AddForum(Forum forum)
    {
        _context.Forums.Add(forum);
        return _context.SaveChanges() > 0;
    }

    public bool UpdateForum(int id, Forum forum)
    {
        forum.Id = id;
        _context.Forums.Update(forum);
        return _context.SaveChanges() > 0;
    }
    
    public bool DeleteForum(int id)
    {
        Forum? forum = GetForum(id);
        if (forum == null) return false;
        _context.Forums.Remove(forum);
        return _context.SaveChanges() > 0;
    }
    
    public bool ForumExists(int id)
    {
        return _context.Forums.Any(e => e.Id == id);
    }
}