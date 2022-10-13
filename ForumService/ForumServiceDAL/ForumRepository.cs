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
    public Forum getForum(int id)
    {
        return _context.Forums.Find(id);
    }

    public IEnumerable<Forum> getForums()
    {
        return _context.Forums.ToList();
    }

    public void addForum(Forum forum)
    {
        _context.Forums.Add(forum);
        _context.SaveChanges();
    }

    public void updateForum(Forum forum)
    {
        _context.Forums.Update(forum);
        _context.SaveChanges();
    }

    public void deleteForum(int id)
    {
        _context.Forums.Remove(getForum(id));
        _context.SaveChanges();
    }
}