using PostServiceLogic;
using PostServiceModels;
using PostServiceModels.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace PostServiceTest;

public class LogicTest
{
    private readonly Mock<IPostRepository> _mockRepo;
    private readonly PostLogic _logic;
    private readonly Post _defaultPost;
    private readonly Post _secondPost;

    public LogicTest()
    {
        Mock<ILogger<PostLogic>> mockServiceLogger = new();
        _mockRepo = new Mock<IPostRepository>();
        _logic = new PostLogic(mockServiceLogger.Object, _mockRepo.Object);

        _defaultPost = new Post{Id = 0, Content = "This is a post for testing"};
        _mockRepo.Setup(repo => repo.GetPost(_defaultPost.Id)).Returns(_defaultPost);
        // This Post is not in the repository by default
        _secondPost = new Post{Id = 1, Content = "This is another post for testing"};
    }

    [Fact]
    public void GetPost_ExistingPost_ReturnsPost()
    {
        var returnedValue = _logic.GetPost(_defaultPost.Id);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(_defaultPost, returnedValue);
    }

    [Fact]
    public void GetPost_NonExistentPost_ReturnsNull()
    {
        var returnedValue = _logic.GetPost(_secondPost.Id);
        
        Assert.Null(returnedValue);
    }

    [Fact]
    public void GetPosts_ExistingPosts_ReturnsPosts()
    {
        List<Post> PostList = new List<Post> { _defaultPost, _secondPost };
        _mockRepo.Setup(repo => repo.GetPosts()).Returns(PostList);
        
        var returnedValue = _logic.GetPosts();
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(PostList, returnedValue);
    }
    
    [Fact]
    public void GetPosts_NoExistingPost_ReturnsEmptyPosts()
    {
        List<Post> PostList = new List<Post>();
        _mockRepo.Setup(repo => repo.GetPosts()).Returns(PostList);
        
        var returnedValue = _logic.GetPosts();
        
        Assert.NotNull(returnedValue);
        Assert.Empty(returnedValue);
    }
    
    [Fact]
    public void PostPost_NewPost_ReturnsPost()
    {
        _mockRepo.Setup(repo => repo.AddPost(_secondPost)).Returns(true);
        _mockRepo.Setup(repo => repo.GetPost(_secondPost.Id)).Returns(_secondPost);
        
        var returnedValue = _logic.AddPost(_secondPost);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(_secondPost, returnedValue);
    }
    
    [Fact]
    public void PostPost_ExistingPost_ReturnsNull()
    {
        var returnedValue = _logic.AddPost(_defaultPost);
        
        Assert.Null(returnedValue);
    }
    
    [Fact]
    public void PutPost_ExistingPost_ReturnsNewPost()
    {
        int existingPostName = _defaultPost.Id;
        _secondPost.Id = existingPostName;
        _mockRepo.Setup(repo => repo.UpdatePost(_secondPost)).Returns(true);
        _mockRepo.Setup(repo => repo.GetPost(_secondPost.Id)).Returns(_secondPost);
        
        var returnedValue = _logic.UpdatePost(_secondPost);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(existingPostName, returnedValue.Id);
        Assert.Equivalent(_secondPost.Content, returnedValue.Content);
        Assert.Equivalent(_secondPost.Id, returnedValue.Id);
    }
    
    [Fact]
    public void PutPost_NonExistentPost_ReturnsNull()
    {
        var returnedValue = _logic.UpdatePost(_secondPost);
        
        Assert.Null(returnedValue);
    }
}