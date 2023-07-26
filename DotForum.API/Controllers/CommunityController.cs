using DotForum.Application.Models.Requests.Community;
using DotForum.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotForum.API.Controllers;

[ApiController]
[Route("api/community")]
public class CommunityController : ControllerBase
{
    private readonly ICommunityService _communityService;
    
    public CommunityController(ICommunityService communityService)
    {
        _communityService = communityService;
    }
    
    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody]CreateCommunityRequest request)
    {
        return Ok(await _communityService.CreateCommunity(request));
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetCommunity([FromQuery]GetCommunityRequest request)
    {
        return Ok(await _communityService.GetCommunity(request));
    }
    
    [HttpGet("following")]
    [Authorize]
    public async Task<IActionResult> GetFollowingCommunity()
    {
        return Ok(await _communityService.GetFollowingCommunities());
    }
    
    [HttpPost("follow")]
    [Authorize]
    public async Task<IActionResult> GetCommunity([FromBody]FollowCommunityRequest request)
    {
        return Ok(await _communityService.FollowCommunity(request));
    }
    
        
    [HttpPost("posts")]
    [Authorize]
    public async Task<IActionResult> GetCommunityPosts([FromBody]GetCommunityPostsRequest request)
    {
        return Ok(await _communityService.GetCommunityPosts(request));
    }
}