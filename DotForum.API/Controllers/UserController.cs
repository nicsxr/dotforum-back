using DotForum.Application.Models.Common;
using DotForum.Application.Models.Requests.User;
using DotForum.Application.Models.Responses.User;
using DotForum.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotForum.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest model)
    {
        return Ok(await _userService.RegisterUser(model));
    }

    [HttpGet("")]
    [Authorize]
    public async Task<ActionResult<AppResponse<GetUserResponse>>> GetUser()
    {
        return Ok(await _userService.GetUser());
    }
}