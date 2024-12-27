using JournalApi.DTOs;
using JournalApi.Models;
using JournalApi.Repositories;
using JournalApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JournalApi.Controllers;

[ApiController]
[Authorize]
public class UserPostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly ITokenService _tokenService;

    public UserPostController(IPostRepository postRepository, ITokenService tokenService)
    {
        _postRepository = postRepository;
        _tokenService = tokenService;
    }

    [HttpGet("/posts")]
    public async Task<IActionResult> GetPosts(
        [FromHeader] string Authorization,
        [FromQuery] QueryParameters parameters
    )
    {
        // Set default values if not provided
        parameters.PageNumber ??= 1;
        parameters.PageSize ??= 20;

        var userId = _tokenService.GetUserIdFromToken(Authorization);

        var posts = await _postRepository.FindByUserIdAsync(userId, parameters);

        var totalCount = await _postRepository.GetTotalPostCountAsync(userId, parameters);

        // Calculate total pages
        var totalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize.Value);

        // Calculate pages left
        var pagesLeft = Math.Max(totalPages - parameters.PageNumber.Value, 0);

        var response = new GetPostsResponse
        {
            Posts = posts,
            PageNumber = parameters.PageNumber.Value,
            PageSize = parameters.PageSize.Value,
            TotalCount = totalCount,
            PagesLeft = pagesLeft,
        };

        return Ok(response);
    }

    [HttpPost("/posts")]
    public async Task<IActionResult> CreatePost(
        [FromHeader] string Authorization,
        [FromBody] CreatePostRequest request
    )
    {
        var userId = _tokenService.GetUserIdFromToken(Authorization);

        var newPost = new Post
        {
            UserId = userId,
            Content = request.Content,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow,
        };

        var post = await _postRepository.AddAsync(newPost);

        var response = new CreatePostResponse { IsSuccess = true, PostId = post.Id };

        return Ok(response);
    }

    [HttpDelete("/posts/{postId}")]
    public async Task<IActionResult> DeletePost(
        [FromHeader] string Authorization,
        [FromRoute] Guid postId
    )
    {
        var userId = _tokenService.GetUserIdFromToken(Authorization);

        var post = await _postRepository.FindByIdAsync(postId);

        if (post == null)
        {
            var res = new ActionStatusResponse { IsSuccess = false, Message = "Post not found" };
            return NotFound(res);
        }

        if (post.UserId != userId)
        {
            var res = new ActionStatusResponse { IsSuccess = false, Message = "Unauthorized" };
            return Unauthorized(res);
        }

        await _postRepository.DeleteAsync(postId);

        var response = new CreatePostResponse { IsSuccess = true, PostId = post.Id };

        return Ok(response);
    }

    [HttpPut("/posts/{postId}")]
    public async Task<IActionResult> UpdatePost(
        [FromHeader] string Authorization,
        [FromRoute] Guid postId,
        [FromBody] CreatePostRequest request
    )
    {
        var userId = _tokenService.GetUserIdFromToken(Authorization);

        var post = await _postRepository.FindByIdAsync(postId);

        if (post == null)
        {
            var res = new ActionStatusResponse { IsSuccess = false, Message = "Post not found" };
            return NotFound(res);
        }

        if (post.UserId != userId)
        {
            var res = new ActionStatusResponse { IsSuccess = false, Message = "Unauthorized" };
            return Unauthorized(res);
        }

        var updatedPost = new Post
        {
            Id = postId,
            UserId = userId,
            Content = request.Content,
            DateCreated = post.DateCreated,
            DateUpdated = DateTime.UtcNow,
        };

        await _postRepository.UpdateAsync(updatedPost);

        var response = new CreatePostResponse { IsSuccess = true, PostId = post.Id };

        return Ok(response);
    }
}
