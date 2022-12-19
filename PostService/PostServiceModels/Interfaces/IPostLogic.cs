namespace PostServiceModels.Interfaces;

public interface IPostLogic
{
    public Post? GetPost(int id);
    public IEnumerable<Post> GetPosts();
    public Post? AddPost(Post post);
    public Post? UpdatePost(Post post);
    public bool DeletePost(int id);
}