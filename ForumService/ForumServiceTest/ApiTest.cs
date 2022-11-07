using ForumServiceAPI.Controllers;
using ForumServiceLogic;
using ForumServiceModels;
using ForumServiceModels.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ForumServiceTest;

public class ApiTest
{
    private readonly Mock<IForumLogic> _mockLogic;
    private readonly ForumServiceController _controller;
    private readonly Forum _defaultForum;
    private readonly Forum _secondForum;

    public ApiTest()
    {
        Mock<ILogger<ForumServiceController>> mockServiceLogger = new();
        _mockLogic = new Mock<IForumLogic>();
        _controller = new ForumServiceController(mockServiceLogger.Object, _mockLogic.Object);

        _defaultForum = new Forum{Id = 1, Name = "Test", Description = "This is a forum for testing"};
        _mockLogic.Setup(repo => repo.GetForum(1)).Returns(_defaultForum);
        // This forum is not in the repository by default
        _secondForum = new Forum{Id = 2, Name = "Test2", Description = "This is another forum for testing"};
    }

    [Fact]
    public void GetForum_ExistingForum_ReturnsOkResultAndForum()
    {
        OkObjectResult? returnedValue = _controller.GetForum(1) as OkObjectResult;
        
        Assert.IsType<OkObjectResult>(returnedValue);
        Assert.Equivalent(_defaultForum, returnedValue?.Value);
    }

    [Fact]
    public void GetForum_NonExistentForum_ReturnsNotFoundResult()
    {
        NotFoundResult? returnedValue = _controller.GetForum(2) as NotFoundResult;
        
        Assert.IsType<NotFoundResult>(returnedValue);
    }

    [Fact]
    public void GetForums_ExistingForums_ReturnsOkResultAndForums()
    {
        List<Forum> forumList = new List<Forum> { _defaultForum, _secondForum };
        _mockLogic.Setup(repo => repo.GetForums()).Returns(forumList);
        
        OkObjectResult? returnedValue = _controller.GetForums() as OkObjectResult;
        
        Assert.IsType<OkObjectResult>(returnedValue);
        Assert.Equivalent(forumList, returnedValue.Value);
    }
    
    [Fact]
    public void GetForums_NoExistingForum_ReturnsOkResult()
    {
        List<Forum> forumList = new List<Forum> {};
        _mockLogic.Setup(repo => repo.GetForums()).Returns(forumList);
        
        OkObjectResult? returnedValue = _controller.GetForums() as OkObjectResult;
        
        Assert.IsType<OkObjectResult>(returnedValue);
        Assert.Equivalent(forumList, returnedValue.Value);
    }
    
    [Fact]
    public void PostForum_NewForum_ReturnsCreatedResultAndForum()
    {
        _mockLogic.Setup(repo => repo.AddForum(_secondForum)).Returns(_secondForum);
        
        CreatedResult? returnedValue = _controller.PostForum(_secondForum) as CreatedResult;
        
        Assert.IsType<CreatedResult>(returnedValue);
        Assert.Equal("GetForum/2", returnedValue.Location);
        Assert.Equivalent(_secondForum, returnedValue?.Value);
    }
    
    [Fact]
    public void PostForum_ExistingForum_ReturnsBadRequest()
    {
        BadRequestResult? returnedValue = _controller.PostForum(_defaultForum) as BadRequestResult;
        
        Assert.IsType<BadRequestResult>(returnedValue);
    }
}