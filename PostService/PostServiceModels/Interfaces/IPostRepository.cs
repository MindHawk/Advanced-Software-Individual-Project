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
}