using ForumServiceAPI.Controllers;
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

        _defaultForum = new Forum { Name = "Test", Description = "This is a forum for testing" };
        _mockLogic.Setup(repo => repo.GetForum(_defaultForum.Name)).Returns(_defaultForum);
        // This forum is not in the repository by default
        _secondForum = new Forum { Name = "Test2", Description = "This is another forum for testing" };
    }

    [Fact]
    public void GetForum_ExistingForum_ReturnsOkObjectAndForum()
    {
        OkObjectResult? returnedValue = _controller.GetForum(_defaultForum.Name) as OkObjectResult;

        Assert.IsType<OkObjectResult>(returnedValue);
        Assert.Equivalent(_defaultForum, returnedValue.Value);
    }

    [Fact]
    public void GetForum_NonExistentForum_ReturnsNotFound()
    {
        NotFoundResult? returnedValue = _controller.GetForum(_secondForum.Name) as NotFoundResult;

        Assert.IsType<NotFoundResult>(returnedValue);
    }

    [Fact]
    public void GetForums_ExistingForums_ReturnsOkObjectAndForums()
    {
        List<Forum> forumList = new List<Forum> { _defaultForum, _secondForum };
        _mockLogic.Setup(repo => repo.GetForums()).Returns(forumList);

        OkObjectResult? returnedValue = _controller.GetForums() as OkObjectResult;

        Assert.IsType<OkObjectResult>(returnedValue);
        Assert.Equivalent(forumList, returnedValue.Value);
    }

    [Fact]
    public void GetForums_NoExistingForum_ReturnsOkObjectResult()
    {
        List<Forum> forumList = new List<Forum> { };
        _mockLogic.Setup(repo => repo.GetForums()).Returns(forumList);

        OkObjectResult? returnedValue = _controller.GetForums() as OkObjectResult;

        Assert.IsType<OkObjectResult>(returnedValue);
        Assert.Equivalent(forumList, returnedValue.Value);
    }

    [Fact]
    public void PostForum_NewForum_ReturnsCreatedAndForum()
    {
        _mockLogic.Setup(repo => repo.AddForum(_secondForum)).Returns(_secondForum);

        CreatedResult? returnedValue = _controller.PostForum(_secondForum) as CreatedResult;

        Assert.IsType<CreatedResult>(returnedValue);
        Assert.Equal($"GetForum/{_secondForum.Name}", returnedValue.Location);
        Assert.Equivalent(_secondForum, returnedValue.Value);
    }

    [Fact]
    public void PostForum_ExistingForum_ReturnsBadRequest()
    {
        BadRequestResult? returnedValue = _controller.PostForum(_defaultForum) as BadRequestResult;

        Assert.IsType<BadRequestResult>(returnedValue);
    }

    [Fact]
    public void PutForum_NewForum_ReturnsNotFound()
    {
        NotFoundResult? returnedValue = _controller.PutForum(_secondForum) as NotFoundResult;

        Assert.IsType<NotFoundResult>(returnedValue);
    }

    [Fact]
    public void PutForum_ExistingForum_ReturnsOkObjectAndForum()
    {
        _mockLogic.Setup(repo => repo.UpdateForum(_defaultForum)).Returns(_defaultForum);

        OkObjectResult? returnedValue = _controller.PutForum(_defaultForum) as OkObjectResult;

        Assert.IsType<OkObjectResult>(returnedValue);
        Assert.Equivalent(_defaultForum, returnedValue.Value);
    }

    [Fact]
    public void DeleteForum_ExistingForum_ReturnsOk()
    {
        _mockLogic.Setup(repo => repo.DeleteForum(_defaultForum.Name)).Returns(true);

        OkResult? returnedValue = _controller.DeleteForum(_defaultForum.Name) as OkResult;

        Assert.IsType<OkResult>(returnedValue);
    }

    [Fact]
    public void DeleteForum_NonExistentForum_ReturnsNotFound()
    {
        _mockLogic.Setup(repo => repo.DeleteForum(_defaultForum.Name)).Returns(false);

        NotFoundResult? returnedValue = _controller.DeleteForum(_defaultForum.Name) as NotFoundResult;

        Assert.IsType<NotFoundResult>(returnedValue);
    }
    
    [Theory]
    [InlineData(3, 0)]
    [InlineData(60, 0)]
    [InlineData(3, 300)]
    [InlineData(60, 300)]
    public void PostForum_ForumValidStringLengths_ReturnsCreated(int nameStringLength, int descriptionStringLength)
    {
        _secondForum.Name = new string('a', nameStringLength);
        _secondForum.Description = new string('a', descriptionStringLength);
        _mockLogic.Setup(repo => repo.AddForum(_secondForum)).Returns(_secondForum);

        CreatedResult? returnedValue = _controller.PostForum(_secondForum) as CreatedResult;

        Assert.IsType<CreatedResult>(returnedValue);
    }
}