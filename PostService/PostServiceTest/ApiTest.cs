using PostServiceAPI.Controllers;
using PostServiceModels;
using PostServiceModels.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace PostServiceTest;

public class ApiTest
{
    private readonly Mock<IPostLogic> _mockLogic;
    private readonly PostServiceController _controller;
    private readonly Post _defaultPost;
    private readonly Post _secondPost;

    public ApiTest()
    {
        Mock<ILogger<PostServiceController>> mockServiceLogger = new();
        _mockLogic = new Mock<IPostLogic>();
        _controller = new PostServiceController(mockServiceLogger.Object, _mockLogic.Object);

        _defaultPost = new Post { Name = "Test", Description = "This is a Post for testing" };
        _mockLogic.Setup(repo => repo.GetPost(_defaultPost.Name)).Returns(_defaultPost);
        // This Post is not in the repository by default
        _secondPost = new Post { Name = "Test2", Description = "This is another Post for testing" };
    }

    [Fact]
    public void GetPost_ExistingPost_ReturnsOkObjectAndPost()
    {
        OkObjectResult? returnedValue = _controller.GetPost(_defaultPost.Name) as OkObjectResult;

        Assert.IsType<OkObjectResult>(returnedValue);
        Assert.Equivalent(_defaultPost, returnedValue.Value);
    }

    [Fact]
    public void GetPost_NonExistentPost_ReturnsNotFound()
    {
        NotFoundResult? returnedValue = _controller.GetPost(_secondPost.Name) as NotFoundResult;

        Assert.IsType<NotFoundResult>(returnedValue);
    }

    [Fact]
    public void GetPosts_ExistingPosts_ReturnsOkObjectAndPosts()
    {
        List<Post> PostList = new List<Post> { _defaultPost, _secondPost };
        _mockLogic.Setup(repo => repo.GetPosts()).Returns(PostList);

        OkObjectResult? returnedValue = _controller.GetPosts() as OkObjectResult;

        Assert.IsType<OkObjectResult>(returnedValue);
        Assert.Equivalent(PostList, returnedValue.Value);
    }

    [Fact]
    public void GetPosts_NoExistingPost_ReturnsOkObjectResult()
    {
        List<Post> PostList = new List<Post> { };
        _mockLogic.Setup(repo => repo.GetPosts()).Returns(PostList);

        OkObjectResult? returnedValue = _controller.GetPosts() as OkObjectResult;

        Assert.IsType<OkObjectResult>(returnedValue);
        Assert.Equivalent(PostList, returnedValue.Value);
    }

    [Fact]
    public void PostPost_NewPost_ReturnsCreatedAndPost()
    {
        _mockLogic.Setup(repo => repo.AddPost(_secondPost)).Returns(_secondPost);

        CreatedResult? returnedValue = _controller.PostPost(_secondPost) as CreatedResult;

        Assert.IsType<CreatedResult>(returnedValue);
        Assert.Equal($"GetPost/{_secondPost.Name}", returnedValue.Location);
        Assert.Equivalent(_secondPost, returnedValue.Value);
    }

    [Fact]
    public void PostPost_ExistingPost_ReturnsBadRequest()
    {
        BadRequestResult? returnedValue = _controller.PostPost(_defaultPost) as BadRequestResult;

        Assert.IsType<BadRequestResult>(returnedValue);
    }

    [Fact]
    public void PutPost_NewPost_ReturnsNotFound()
    {
        NotFoundResult? returnedValue = _controller.PutPost(_secondPost) as NotFoundResult;

        Assert.IsType<NotFoundResult>(returnedValue);
    }

    [Fact]
    public void PutPost_ExistingPost_ReturnsOkObjectAndPost()
    {
        _mockLogic.Setup(repo => repo.UpdatePost(_defaultPost)).Returns(_defaultPost);

        OkObjectResult? returnedValue = _controller.PutPost(_defaultPost) as OkObjectResult;

        Assert.IsType<OkObjectResult>(returnedValue);
        Assert.Equivalent(_defaultPost, returnedValue.Value);
    }

    [Fact]
    public void DeletePost_ExistingPost_ReturnsOk()
    {
        _mockLogic.Setup(repo => repo.DeletePost(_defaultPost.Name)).Returns(true);

        OkResult? returnedValue = _controller.DeletePost(_defaultPost.Name) as OkResult;

        Assert.IsType<OkResult>(returnedValue);
    }

    [Fact]
    public void DeletePost_NonExistentPost_ReturnsNotFound()
    {
        _mockLogic.Setup(repo => repo.DeletePost(_defaultPost.Name)).Returns(false);

        NotFoundResult? returnedValue = _controller.DeletePost(_defaultPost.Name) as NotFoundResult;

        Assert.IsType<NotFoundResult>(returnedValue);
    }
    
    [Theory]
    [InlineData(3, 0)]
    [InlineData(60, 0)]
    [InlineData(3, 300)]
    [InlineData(60, 300)]
    public void PostPost_PostValidStringLengths_ReturnsCreated(int nameStringLength, int descriptionStringLength)
    {
        _secondPost.Name = new string('a', nameStringLength);
        _secondPost.Description = new string('a', descriptionStringLength);
        _mockLogic.Setup(repo => repo.AddPost(_secondPost)).Returns(_secondPost);

        CreatedResult? returnedValue = _controller.PostPost(_secondPost) as CreatedResult;

        Assert.IsType<CreatedResult>(returnedValue);
    }
}