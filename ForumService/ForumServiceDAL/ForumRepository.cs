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
    }
    public Forum? GetForum(string name)
    {
        Forum? forum = _context.Forums.Find(name);
        return forum?.Deleted == true ? null : forum;
    }

    public IEnumerable<Forum> GetForums()
    {
        return _context.Forums.Where(f => f.Deleted == false).ToList();
    }

    public bool AddForum(Forum forum)
    {
        _context.Forums.Add(forum);
        forum.Deleted = false;
        return _context.SaveChanges() > 0;
    }

    public bool UpdateForum(Forum forum)
    {
        _context.Forums.Update(forum);
        return _context.SaveChanges() > 0;
    }
    
    public bool DeleteForum(string name)
    {
        Forum? forum = GetForum(name);
        if (forum == null) return false;
        forum.Deleted = true;
        return UpdateForum(forum);
    }
    
    public bool ForumExists(string name)
    {
        return _context.Forums.Any(e => e.Name == name);
    }

    public bool AddAccount(Account account)
    {
        _context.Accounts.Add(account);
        return _context.SaveChanges() > 0;
    }

    public bool DeleteAccount(Account account)
    {
        _context.Accounts.Remove(account);
        return _context.SaveChanges() > 0;
    }

    public int? GetAccountIdFromGoogleId(string googleId)
    {
        int id;
        try
        {
            id = _context.Accounts.First(e => e.GoogleId == googleId).Id;
        }
        catch (InvalidOperationException)
        {
            return null;
        }

        return id;
    }
}