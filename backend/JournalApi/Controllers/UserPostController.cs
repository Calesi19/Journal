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
        [FromQuery] GetPostsRequest request
    )
    {
        var userId = _tokenService.GetUserIdFromToken(Authorization);

        var posts = await _postRepository.FindByUserIdAsync(userId);

        var response = new ApiResponse<GetPostsResponse>
        {
            Response = new GetPostsResponse() { Posts = posts },
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
        };

        return Ok(response);
    }
}
