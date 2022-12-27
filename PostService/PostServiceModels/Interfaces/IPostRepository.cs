namespace PostServiceModels.Interfaces;

public interface IPostRepository
{
    public Post? GetPost(int id);
    public IEnumerable<Post> GetPosts();
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
    public bool AddForum(ForumShared forum);
}