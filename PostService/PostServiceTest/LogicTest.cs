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

        _defaultPost = new Post{Name = "Test", Description = "This is a Post for testing"};
        _mockRepo.Setup(repo => repo.GetPost(_defaultPost.Name)).Returns(_defaultPost);
        // This Post is not in the repository by default
        _secondPost = new Post{Name = "Test2", Description = "This is another Post for testing"};
    }

    [Fact]
    public void GetPost_ExistingPost_ReturnsPost()
    {
        var returnedValue = _logic.GetPost(_defaultPost.Name);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(_defaultPost, returnedValue);
    }

    [Fact]
    public void GetPost_NonExistentPost_ReturnsNull()
    {
        var returnedValue = _logic.GetPost(_secondPost.Name);
        
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
        _mockRepo.Setup(repo => repo.GetPost(_secondPost.Name)).Returns(_secondPost);
        
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
        string existingPostName = _defaultPost.Name;
        _secondPost.Name = existingPostName;
        _mockRepo.Setup(repo => repo.UpdatePost(_secondPost)).Returns(true);
        _mockRepo.Setup(repo => repo.GetPost(_secondPost.Name)).Returns(_secondPost);
        
        var returnedValue = _logic.UpdatePost(_secondPost);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(existingPostName, returnedValue.Name);
        Assert.Equivalent(_secondPost.Description, returnedValue.Description);
        Assert.Equivalent(_secondPost.Name, returnedValue.Name);
    }
    
    [Fact]
    public void PutPost_NonExistentPost_ReturnsNull()
    {
        var returnedValue = _logic.UpdatePost(_secondPost);
        
        Assert.Null(returnedValue);
    }
}