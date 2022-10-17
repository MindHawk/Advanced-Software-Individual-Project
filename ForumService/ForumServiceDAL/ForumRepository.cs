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
    public Forum GetForum(int id)
    {
        return _context.Forums.Find(id);
    }

    public IEnumerable<Forum> GetForums()
    {
        return _context.Forums.ToList();
    }

    public void AddForum(Forum forum)
    {
        _context.Forums.Add(forum);
        _context.SaveChanges();
    }

    public void UpdateForum(Forum forum)
    {
        _context.Forums.Update(forum);
        _context.SaveChanges();
    }

    public void DeleteForum(int id)
    {
        _context.Forums.Remove(GetForum(id));
        _context.SaveChanges();
    }
}