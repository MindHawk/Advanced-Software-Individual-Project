namespace PostServiceModels.Interfaces;

public interface IPostRepository
{
    public Post? GetPost(int id);
    public IEnumerable<Post> GetPosts(string forumName);
    public bool AddPost(Post post);
    public bool UpdatePost(Post post);
    public bool DeletePost(int id);
    public bool PostExists(int id);
    public List<Comment> GetCommentsForPost(int postId); 
    public bool AddComment(Comment comment);
    public bool UpdateComment(Comment comment);
    public bool DeleteComment(int id);
    public bool CommentExists(int id);
    public Comment? GetComment(int id);
    /// <summary>
    /// This is an internal method used to check if a forum is valid
    /// </summary>
    public bool ForumExists(string name);
    /// <summary>
    /// This method should only be called by message events
    /// </summary>
    public bool AddForum(Forum forum);
    /// <summary>
    /// This method should only be called by message events
    /// </summary>
    public bool DeleteForum(Forum forum);
}