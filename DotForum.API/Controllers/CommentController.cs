using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.Comment;
using DotForum.Application.Models.Requests.Post;
using DotForum.Application.Models.Responses.Comment;
using DotForum.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotForum.API.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    
    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> CreateComment([FromBody]CreateCommentRequest request)
    {
        return Ok(await _commentService.CreateComment(request));
    }
    //
    // [HttpDelete("")]
    // [Authorize]
    // public async Task<IActionResult> DeletePost([FromBody]DeletePostRequest request)
    // {
    //     return Ok(await _postService.DeletePost(request));
    // }
    //
    //
    [HttpGet("")]
    public async Task<IActionResult> GetComment([FromQuery]GetCommentRequest request)
    {
        return Ok(await _commentService.GetComment(request));
    }
    
    [HttpGet("children")]
    public async Task<ActionResult<AppResponse<GetCommentsByPostIdResponse>>> GetChildComments([FromQuery]GetChildCommentsRequest request)
    {
        return Ok(await _commentService.GetChildComments(request));
    }
    
    [HttpPost("vote")]
    [Authorize]
    public async Task<ActionResult<AppResponse<EmptyResponse>>> Upvote([FromBody]VoteCommentRequest request)
    {
        if (!ModelState.IsValid) return BadRequest();
        return Ok(await _commentService.VoteComment(request));
    }
}