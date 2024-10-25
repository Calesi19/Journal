using JournalApi.DTOs;
using JournalApi.Models;
using JournalApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JournalApi.Controllers;

[ApiController]
public class UserPostController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public UserPostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [Authorize]
    [HttpGet("accounts/posts")]
    public async Task<IActionResult> GetPosts(
        [FromHeader] string Authorization,
        [FromQuery] GetPostsRequest request
    )
    {
        var userId = Guid.Parse(User.FindFirst("nameid").Value);

        var posts = await _postRepository.FindByUserIdAsync(userId);

        var response = new ApiResponse<GetPostsResponse>
        {
            Response = new GetPostsResponse() { Posts = posts },
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPost("accounts/posts")]
    public async Task<IActionResult> CreatePost(
        [FromHeader] string Authorization,
        [FromBody] ApiRequest<CreatePostRequest> request
    )
    {
        var userId = Guid.Parse(User.FindFirst("nameid").Value);

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
