using ForumServiceLogic;
using ForumServiceMessageBusProducer;
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
        Mock<ForumMessageBusProducer> mockProducer = new();
        _mockRepo = new Mock<IForumRepository>();
        _logic = new ForumLogic(mockServiceLogger.Object, _mockRepo.Object, mockProducer.Object);

        _defaultForum = new Forum{Name = "Test", Description = "This is a forum for testing"};
        _mockRepo.Setup(repo => repo.GetForum(_defaultForum.Name)).Returns(_defaultForum);
        // This forum is not in the repository by default
        _secondForum = new Forum{Name = "Test2", Description = "This is another forum for testing"};
    }

    [Fact]
    public void GetForum_ExistingForum_ReturnsForum()
    {
        var returnedValue = _logic.GetForum(_defaultForum.Name);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(_defaultForum, returnedValue);
    }

    [Fact]
    public void GetForum_NonExistentForum_ReturnsNull()
    {
        var returnedValue = _logic.GetForum(_secondForum.Name);
        
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
        _mockRepo.Setup(repo => repo.AddForum(_secondForum)).Returns(true);
        _mockRepo.Setup(repo => repo.GetForum(_secondForum.Name)).Returns(_secondForum);
        
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
        string existingForumName = _defaultForum.Name;
        _secondForum.Name = existingForumName;
        _mockRepo.Setup(repo => repo.UpdateForum(_secondForum)).Returns(true);
        _mockRepo.Setup(repo => repo.GetForum(_secondForum.Name)).Returns(_secondForum);
        
        var returnedValue = _logic.UpdateForum(_secondForum);
        
        Assert.NotNull(returnedValue);
        Assert.Equivalent(existingForumName, returnedValue.Name);
        Assert.Equivalent(_secondForum.Description, returnedValue.Description);
        Assert.Equivalent(_secondForum.Name, returnedValue.Name);
    }
    
    [Fact]
    public void PutForum_NonExistentForum_ReturnsNull()
    {
        var returnedValue = _logic.UpdateForum(_secondForum);
        
        Assert.Null(returnedValue);
    }
}