using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Post;
using DotForum.Application.Models.Responses.Post;
using DotForum.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotForum.API.Controllers;

[ApiController]
[Route("api/post")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;
    
    public PostController(IPostService postService, ICommentService commentService)
    {
        _postService = postService;
        _commentService = commentService;
    }
    
    [HttpPost("")]
    [Authorize]
    public async Task<ActionResult<AppResponse<CreatePostResponse>>> CreatePost([FromBody]CreatePostRequest request)
    {
        return Ok(await _postService.CreatePost(request));
    }
    
    [HttpDelete("")]
    [Authorize]
    public async Task<IActionResult> DeletePost([FromBody]DeletePostRequest request)
    {
        return Ok(await _postService.DeletePost(request));
    }
    
    [HttpGet("")]
    public async Task<ActionResult<AppResponse<GetPostResponse>>> GetPost([FromQuery] GetPostRequest request)
    {
        return Ok(await _postService.GetPost(request));
    }
    
    [HttpGet("comments")]
    public async Task<ActionResult<AppResponse<GetPostResponse>>> GetCommentsByPostId([FromQuery] GetPostCommentsRequest request)
    {
        return Ok(await _commentService.GetCommentsByPostId(request));
    }
    
    [HttpPost("vote")]
    [Authorize]
    public async Task<ActionResult<AppResponse<VotePostResponse>>> Upvote([FromBody] VotePostRequest request)
    {
        if (!ModelState.IsValid) return BadRequest();
        return Ok(await _postService.VotePost(request));
    }
}