﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostServiceDAL;
using PostServiceModels;
using PostServiceModels.Interfaces;
using Microsoft.Extensions.Logging;

namespace PostServiceLogic;

public class PostLogic : IPostLogic
{
    private readonly IPostRepository _repository;
    private readonly ILogger<IPostLogic> _logger;
    private readonly IProblemDetailsService _problemDetails;
    public PostLogic(ILogger<IPostLogic> logger, IPostRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public (Post post, List<Comment> comments) GetPostWithComments(int postId)
    {
        _logger.LogInformation("Getting post {Id} with comments", postId);
        var post = _repository.GetPost(postId);
        var comments = _repository.GetCommentsForPost(postId);
        if(post == null)
        {
            _logger.LogWarning("Post {Id} not found", postId);
        }
        return (post, comments);
    }

    public List<Comment> GetCommentsForPost(int postId)
    {
        return _repository.GetCommentsForPost(postId);
    }

    public Post? GetPost(int id)
    {
        _logger.Log(LogLevel.Information, "Getting post with id {Id}", id);
        var post = _repository.GetPost(id);
        if (post == null)
        {
            _logger.LogWarning("Post {Id} not found", id);
        }
        return post;
    }

    public IEnumerable<Post> GetPosts()
    {
        _logger.Log(LogLevel.Information, "Getting all Posts");
        return _repository.GetPosts();
    }

    public Post? AddPost(Post post)
    {
        if (_repository.PostExists(post.Id))
        {
            _logger.Log(LogLevel.Information, "Post with id {Id} already exists", post.Id);
            return null;
        }
        _logger.Log(LogLevel.Information, "Adding post with id {Id}", post.Id);
        if (_repository.AddPost(post))
        {
            return _repository.GetPost(post.Id);
        }
        return null;
    }

    public Post? UpdatePost(Post post)
    {
        _logger.Log(LogLevel.Information, "Updating post with id {Id}", post.Id);
        return _repository.UpdatePost(post) ? _repository.GetPost(post.Id) : null;
    }

    public bool DeletePost(int id)
    { 
        _logger.Log(LogLevel.Information, "Deleting post with id {Id}", id);
        return _repository.DeletePost(id);
    }
}