using ForumServiceLogic;
using ForumServiceModels;
using ForumServiceModels.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForumServiceTest;

public class LogicTest
{
    private readonly Mock<IForumRepository> _mockRepo;
    private readonly ForumLogic _logic;
    private readonly Forum _defaultForum;
    private readonly Forum _secondForum;

    public LogicTest()
    {
        Mock<ILogger<ForumLogic>> mockServiceLogger = new();
        _mockRepo = new Mock<IForumRepository>();
        _logic = new ForumLogic(mockServiceLogger.Object, _mockRepo.Object);

        _defaultForum = new Forum{Id = 1, Name = "Test", Description = "This is a forum for testing"};
        _mockRepo.Setup(repo => repo.GetForum(_defaultForum.Id)).Returns(_defaultForum);
        // This forum is not in the repository by default
        _secondForum = new Forum{Id = 2, Name = "Test2", Description = "This is another forum for testing"};
    }

    [Fact]
    public void GetForum_ExistingForum_ReturnsForum()
    {
        var returnedValue = _logic.GetForum(1);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(_defaultForum, returnedValue);
    }

    [Fact]
    public void GetForum_NonExistentForum_ReturnsNull()
    {
        var returnedValue = _logic.GetForum(2);
        
        Assert.Null(returnedValue);
    }

    [Fact]
    public void GetForums_ExistingForums_ReturnsForums()
    {
        List<Forum> forumList = new List<Forum> { _defaultForum, _secondForum };
        _mockRepo.Setup(repo => repo.GetForums()).Returns(forumList);
        
        var returnedValue = _logic.GetForums();
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(forumList, returnedValue);
    }
    
    [Fact]
    public void GetForums_NoExistingForum_ReturnsEmptyForums()
    {
        List<Forum> forumList = new List<Forum>();
        _mockRepo.Setup(repo => repo.GetForums()).Returns(forumList);
        
        var returnedValue = _logic.GetForums();
        
        Assert.NotNull(returnedValue);
        Assert.Empty(returnedValue);
    }
    
    [Fact]
    public void PostForum_NewForum_ReturnsForum()
    {
        _mockRepo.Setup(repo => repo.GetForum(_secondForum.Id)).Returns(_secondForum);
        
        var returnedValue = _logic.AddForum(_secondForum);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(_secondForum, returnedValue);
    }
    
    [Fact]
    public void PostForum_ExistingForum_ReturnsNull()
    {
        var returnedValue = _logic.AddForum(_defaultForum);
        
        Assert.Null(returnedValue);
    }
    
    [Fact]
    public void PutForum_ExistingForum_ReturnsNewForum()
    {
        int existingForumId = _defaultForum.Id;
        _mockRepo.Setup(repo => repo.GetForum(_secondForum.Id)).Returns(_secondForum);
        
        var returnedValue = _logic.UpdateForum(1, _secondForum);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(existingForumId, returnedValue.Id);
        Assert.Equivalent(_secondForum.Description, returnedValue.Description);
        Assert.Equivalent(_secondForum.Name, returnedValue.Name);
    }
    
    [Fact]
    public void PutForum_NonExistentForum_ReturnsNull()
    {
        var returnedValue = _logic.UpdateForum(2, _secondForum);
        
        Assert.Null(returnedValue);
    }
}