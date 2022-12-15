namespace PostServiceModels.Interfaces;

public interface IPostLogic
{
    public Post? GetPost(string name);
    public IEnumerable<Post> GetPosts();
    public Post? AddPost(Post Post);
    public Post? UpdatePost(Post Post);
    public bool DeletePost(string name);
}