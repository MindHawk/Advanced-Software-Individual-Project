namespace PostServiceModels.Interfaces;

public interface IPostRepository
{
    public Post? GetPost(string name);
    public IEnumerable<Post> GetPosts();
    public bool AddPost(Post Post);
    public bool UpdatePost(Post Post);
    public bool DeletePost(string name);
    public bool PostExists(string name);
}