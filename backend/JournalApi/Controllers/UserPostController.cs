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

    [HttpGet("accounts/posts")]
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

        var response = new ApiResponse<GetPostsResponse>
        {
            Response = new GetPostsResponse()
            {
                Posts = posts,
                PageNumber = parameters.PageNumber.Value,
                PageSize = parameters.PageSize.Value,
                TotalCount = totalCount,
            },
        };

        return Ok(response);
    }

    [HttpPost("accounts/posts")]
    public async Task<IActionResult> CreatePost(
        [FromHeader] string Authorization,
        [FromBody] ApiRequest<CreatePostRequest> request
    )
    {
        var userId = _tokenService.GetUserIdFromToken(Authorization);

        var newPost = new Post
        {
            UserId = userId,
            Content = request.Request.Content,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow,
        };

        var post = await _postRepository.AddAsync(newPost);

        var response = new ApiResponse<CreatePostResponse>
        {
            Response = new CreatePostResponse() { IsSuccess = true, PostId = post.Id },
            Message = "Post created successfully",
        };

        return Ok(response);
    }

    [HttpDelete("accounts/posts/{postId}")]
    public async Task<IActionResult> DeletePost(
        [FromHeader] string Authorization,
        [FromRoute] Guid postId
    )
    {
        var userId = _tokenService.GetUserIdFromToken(Authorization);

        var post = await _postRepository.FindByIdAsync(postId);

        if (post == null)
        {
            return NotFound();
        }

        if (post.UserId != userId)
        {
            return Unauthorized();
        }

        await _postRepository.DeleteAsync(postId);

        var response = new ApiResponse<CreatePostResponse>
        {
            Response = new CreatePostResponse() { IsSuccess = true, PostId = post.Id },
            Message = "Post deleted successfully",
        };

        return Ok(response);
    }

    [HttpPut("accounts/posts/{postId}")]
    public async Task<IActionResult> UpdatePost(
        [FromHeader] string Authorization,
        [FromRoute] Guid postId,
        [FromBody] ApiRequest<CreatePostRequest> request
    )
    {
        var userId = _tokenService.GetUserIdFromToken(Authorization);

        var post = await _postRepository.FindByIdAsync(postId);

        if (post == null)
        {
            return NotFound();
        }

        if (post.UserId != userId)
        {
            return Ok(new { PostUserId = post.UserId, UserId = userId });
        }

        var updatedPost = new Post
        {
            Id = postId,
            UserId = userId,
            Content = request.Request.Content,
            DateCreated = post.DateCreated,
            DateUpdated = DateTime.UtcNow,
        };

        await _postRepository.UpdateAsync(updatedPost);

        var response = new ApiResponse<CreatePostResponse>
        {
            Response = new CreatePostResponse() { IsSuccess = true, PostId = post.Id },
            Message = "Post updated successfully",
        };

        return Ok(response);
    }
}
