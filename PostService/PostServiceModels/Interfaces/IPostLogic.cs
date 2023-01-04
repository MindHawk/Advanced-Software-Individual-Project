namespace PostServiceModels.Interfaces;

public interface IPostLogic
{
    public (Post post, List<Comment> comments) GetPostWithComments(int postId);
    public List<Comment> GetCommentsForPost(int postId);
    public Post? GetPost(int id);
    public IEnumerable<Post>? GetPosts(string forumName);
    public Post? AddPost(Post post);
    public Post? UpdatePost(Post post);
    public bool DeletePost(int id);
    public Comment? AddComment(Comment comment);
    public Comment? UpdateComment(Comment comment);
    public bool DeleteComment(int id);
}