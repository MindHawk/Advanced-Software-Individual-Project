namespace PostServiceModels.Interfaces;

public interface IPostLogic
{
    public (Post post, List<Comment> comments) GetPostWithComments(int postId);
    public List<Comment> GetCommentsForPost(int postId);
    public Post? GetPost(int id);
    public IEnumerable<Post> GetPosts();
    public Post? AddPost(Post post);
    public Post? UpdatePost(Post post);
    public bool DeletePost(int id);
}